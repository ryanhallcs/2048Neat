using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp2048.Data;
using SharpNeat.Genomes.Neat;

namespace Sharp2048.Neat.Service
{
    public class SharpNeat2048Service
    {
        private readonly Sharp2048DataModelContainer _neatDb;
        public SharpNeat2048Service(Sharp2048DataModelContainer neatDb)
        {
            _neatDb = neatDb;
        }

        public Genome GetGenome(Guid genomeId)
        {
            return _neatDb.Genomes.SingleOrDefault(a => a.GenomeIdentifier == genomeId);
        }

        public Genome SaveNewGenome(Guid id, string description, string rawGenome)
        {
            throw new NotImplementedException();
        }
    }
}
