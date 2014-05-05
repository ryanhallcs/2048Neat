using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sharp2048.Data;
using Sharp2048.Neat.Service.Models;
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
        DirectionEnum ProcessMove(int[,] state, Guid genomeId);
    }

    public class SharpNeat2048Service : ISharpNeat2048Service
    {
        private readonly Sharp2048DataModelContainer _neatDb;
        private readonly IGenomeDecoder<NeatGenome, IBlackBox> _decoder;
        private readonly IGenomeGenerator _generator;
        public SharpNeat2048Service(Sharp2048DataModelContainer neatDb, IGenomeDecoder<NeatGenome, IBlackBox> decoder, IGenomeGenerator generator)
        {
            _neatDb = neatDb;
            _decoder = decoder;
            _generator = generator;
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
            _neatDb.SaveChanges();
            return GetGenome(newGenome.GenomeIdentifier);
        }

        private NeatGenome _trySaveSingle(string rawGenome)
        {
            try
            {
                return _generator.GenerateFromXmlString(rawGenome);
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
                return _generator.GenerateSimpleListFromXmlString(rawGenome).FirstOrDefault();
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
                return _generator.GenerateCompleteListFromXmlString(rawGenome).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IBlackBox GetDecodedGenome(Guid genomeId)
        {
            var genome = GetGenome(genomeId);

            var neatGenome = _generator.GenerateFromXmlString(genome.GenomeXml);

            return _decoder.Decode(neatGenome);
        }

        public DirectionEnum ProcessMove(int[,] state, Guid genomeId)
        {
            var phenome = GetDecodedGenome(genomeId);
            phenome.ResetState();
            var size = state.GetLength(0);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    phenome.InputSignalArray[i*size + j] = state[i, j];

            var crMax = phenome.OutputSignalArray[0];
            var result = DirectionEnum.Left;
            if (phenome.OutputSignalArray[1] > crMax)
            {
                result = DirectionEnum.Right;
                crMax = phenome.OutputSignalArray[1];
            }
            if (phenome.OutputSignalArray[2] > crMax)
            {
                result = DirectionEnum.Down;
                crMax = phenome.OutputSignalArray[2];
            }
            if (phenome.OutputSignalArray[3] > crMax)
            {
                result = DirectionEnum.Up;
            }

            return result;
        }
    }
}
