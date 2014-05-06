using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp2048.Data;
using SharpNeat.Network;
using SharpNeat.Utility;

namespace Sharp2048.Neat.Service
{
    public class DbActivationFunctionLibrary : IActivationFunctionLibrary
    {
        private readonly Sharp2048DataModelContainer _neatDb;
        private readonly IActivationFunctionFactory _activationFunctionFactory;

        public DbActivationFunctionLibrary(Sharp2048DataModelContainer neatDb, IActivationFunctionFactory activationFunctionFactory)
        {
            _neatDb = neatDb;
            _activationFunctionFactory = activationFunctionFactory;
        }

        public IActivationFunction GetFunction(int id)
        {
            var dbFunction = _neatDb.ActivationFunctions.SingleOrDefault(a => a.ActivationFunctionId == id);

            if (dbFunction == null)
            {
                return null;
            }

            return _activationFunctionFactory.Create(dbFunction.Lookup);
        }

        public ActivationFunctionInfo GetRandomFunction(FastRandom rng)
        {
            var allFns = _neatDb.ActivationFunctions.ToList();

            var selected = allFns[rng.Next(allFns.Count)];

            return new ActivationFunctionInfo(selected.ActivationFunctionId, 1.0 / allFns.Count, _activationFunctionFactory.Create(selected.Lookup));
        }

        public IList<ActivationFunctionInfo> GetFunctionList()
        {
            var cnt = _neatDb.ActivationFunctions.Count();
            return
                _neatDb.ActivationFunctions.Select(
                    a =>
                    new ActivationFunctionInfo(a.ActivationFunctionId, 1.90/cnt,
                                               _activationFunctionFactory.Create(a.Lookup))).ToList();
        }
    }
}
