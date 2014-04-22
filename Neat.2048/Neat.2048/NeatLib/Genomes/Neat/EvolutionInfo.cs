using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petalz.Neat.Windows.NeatLib.Genomes.Neat
{
    public class EvolutionInfo
    {
        public EvolutionTypeEnum EvolutionType { get; set; }

        public ChangeTypeEnum ChangeType { get; set; }

        public uint ParentId1 { get; set; }

        public uint? ParentId2 { get; set; }
    }

    public enum EvolutionTypeEnum
    {
        Asexual = 1,
        Sexual = 2
    }

    public enum ChangeTypeEnum
    {
        AddConnection = 1,
        AddNode = 2,
        WeightChange = 3,
        DeleteNode = 4,
        DeleteConnection = 5,
        Mate = 6,
        NoChange = 7,
        NodeAuxStateChanged = 8
    }
}
