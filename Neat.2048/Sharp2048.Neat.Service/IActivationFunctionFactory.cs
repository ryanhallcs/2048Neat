using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpNeat.Network;

namespace Sharp2048.Neat.Service
{
    public interface IActivationFunctionFactory
    {
        IActivationFunction GetFunction(string activationFn);
        void Release(IActivationFunction function);
    }
}
