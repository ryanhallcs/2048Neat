using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using SharpNeat.Genomes.Neat;

namespace Sharp2048.Neat.Service
{
    public interface IGenomeGenerator
    {
        NeatGenome GenerateFromXmlString(string xml);

        List<NeatGenome> GenerateCompleteListFromXmlString(string xml);
        List<NeatGenome> GenerateSimpleListFromXmlString(string genomeXml);
    }

    public class GenomeGenerator : IGenomeGenerator
    {
        private readonly NeatGenomeFactory _neatGenomeFactory;

        public GenomeGenerator(NeatGenomeFactory factory)
        {
            _neatGenomeFactory = factory;
        }

        public NeatGenome GenerateFromXmlString(string genomeXml)
        {
            NeatGenome neatGenome = null;

            using (var stream = new MemoryStream(_getEncoding(genomeXml).GetBytes(genomeXml)))
            using (var xml = XmlReader.Create(stream))
            {
                neatGenome = NeatGenomeXmlIO.ReadGenome(xml, false, _neatGenomeFactory);
            }

            return neatGenome;
        }

        public List<NeatGenome> GenerateCompleteListFromXmlString(string genomeXml)
        {
            List<NeatGenome> result = null;

            using (var stream = new MemoryStream(_getEncoding(genomeXml).GetBytes(genomeXml)))
            using (var xml = XmlReader.Create(stream))
            {
                result = NeatGenomeXmlIO.ReadCompleteGenomeList(xml, false, _neatGenomeFactory);
            }

            return result;
        }

        public List<NeatGenome> GenerateSimpleListFromXmlString(string genomeXml)
        {
            List<NeatGenome> result = null;
            using (var stream = new MemoryStream(_getEncoding(genomeXml).GetBytes(genomeXml)))
            using (var xml = XmlReader.Create(stream))
            {
                result = NeatGenomeXmlIO.ReadSimpleGenomeList(xml, false, _neatGenomeFactory);
            }

            return result;
        }

        private Encoding _getEncoding(string genomeXml)
        {
            return Encoding.GetEncoding(Regex.Match(genomeXml, "encoding=\"([^\"]+)\"").Groups[1].Value);
        }
    }
}
