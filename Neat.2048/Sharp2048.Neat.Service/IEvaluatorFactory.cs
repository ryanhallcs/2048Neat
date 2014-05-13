using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp2048.Neat.Service
{
    public interface IEvaluatorFactory
    {
        IGenome2048Ai Create(string type);
        void Release(IGenome2048Ai ai);
    }
}
