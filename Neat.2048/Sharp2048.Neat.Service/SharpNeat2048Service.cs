﻿using System;
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
            var genome = _trySaveSingle(rawGenome) 
                ?? _trySaveSingleList(rawGenome)
                ?? _trySaveSingleFullList(rawGenome);

            if (genome == null)
            {
                return null;
            }

            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder))
            {
                NeatGenomeXmlIO.Write(writer, genome, false);
            }

            var newGenome = new Genome
            {
                GenomeIdentifier = Guid.NewGuid(),
                Description = description,
                GenomeXml = stringBuilder.ToString()
            };
            _neatDb.Genomes.Add(newGenome);
            return GetGenome(newGenome.GenomeIdentifier);
        }

        private NeatGenome _trySaveSingle(string rawGenome)
        {
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawGenome)))
                using (var xml = XmlReader.Create(stream))
                {
                    return NeatGenomeXmlIO.ReadGenome(xml, false);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private NeatGenome _trySaveSingleList(string rawGenome)
        {
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawGenome)))
                using (var xml = XmlReader.Create(stream))
                {
                    return NeatGenomeXmlIO.ReadSimpleGenomeList(xml, false).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private NeatGenome _trySaveSingleFullList(string rawGenome)
        {
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawGenome)))
                using (var xml = XmlReader.Create(stream))
                {
                    return NeatGenomeXmlIO.ReadCompleteGenomeList(xml, false, new NeatGenomeFactory(16, 4)).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
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