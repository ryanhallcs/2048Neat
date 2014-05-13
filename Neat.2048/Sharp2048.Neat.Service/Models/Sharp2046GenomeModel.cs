using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpNeat.Genomes.Neat;

namespace Sharp2048.Neat.Service.Models
{
    public class Sharp2046GenomeModel
    {
        public Guid GenomeId { get; set; }

        public NeatGenome Genome { get; set; }

        public string Description { get; set; }

        public Type EvaluatorType { get; set; }
    }
}
