using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sharp2048.Data;
using SharpNeat.Core;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;

namespace Sharp2048.Neat.Service
{
    public interface ISharpNeat2048Service
    {
        Genome GetGenome(Guid genomeId);
        Genome SaveNewGenome(string description, string loadedBy, string rawGenome);
        IBlackBox GetDecodedGenome(Guid genomeId);
    }

    public class SharpNeat2048Service : ISharpNeat2048Service
    {
        private readonly Sharp2048DataModelContainer _neatDb;
        private readonly IGenomeDecoder<NeatGenome, IBlackBox> _decoder;
        public SharpNeat2048Service(Sharp2048DataModelContainer neatDb, IGenomeDecoder<NeatGenome,IBlackBox> decoder)
        {
            _neatDb = neatDb;
            _decoder = decoder;
        }

        public Genome GetGenome(Guid genomeId)
        {
            return _neatDb.Genomes.SingleOrDefault(a => a.GenomeIdentifier == genomeId);
        }

        public Genome SaveNewGenome(string description, string loadedBy, string rawGenome)
        {
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawGenome)))
                using (var xml = XmlReader.Create(stream))
                {
                    NeatGenomeXmlIO.ReadGenome(xml, false);
                }
            }
            catch (Exception)
            {
                return null;
            }

            var newGenome = new Genome
                {
                    GenomeIdentifier = Guid.NewGuid(),
                    Description = description,
                    GenomeXml = rawGenome
                };
            _neatDb.Genomes.Add(newGenome);
            return GetGenome(newGenome.GenomeIdentifier);
        }

        public IBlackBox GetDecodedGenome(Guid genomeId)
        {
            var genome = GetGenome(genomeId);

            NeatGenome neatGenome = null;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(genome.GenomeXml)))
            using (var xml = XmlReader.Create(stream))
            {
                neatGenome = NeatGenomeXmlIO.ReadGenome(xml, false);
            }

            return _decoder.Decode(neatGenome);
        }
    }
}
