using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sharp2048.Data;
using Sharp2048.Neat.Service;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using SharpNeat.Phenomes;

namespace Sharp2048.Web
{
    public class Sharp2048WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<TypeNameComponentSelector>());

            // Controllers
            container.Register(
                Classes.FromAssemblyContaining<Sharp2048WindsorInstaller>().BasedOn<IController>().LifestyleTransient());

            // Sharp Neat service
            container.Register(
                Component.For<ISharpNeat2048Service>().ImplementedBy<SharpNeat2048Service>().LifestyleTransient());
            container.Register(
                Classes.FromAssemblyContaining<IActivationFunction>().BasedOn<IActivationFunction>().Configure(c => c.Named(c.Implementation.Name)));
            container.Register(
                Classes.FromAssemblyContaining<IGenome2048Ai>().BasedOn<IGenome2048Ai>().Configure(c => c.Named(c.Implementation.FullName)));
            container.Register(Component.For<IActivationFunctionFactory>().AsFactory(c => c.SelectedWith<TypeNameComponentSelector>()));
            container.Register(Component.For<IEvaluatorFactory>().AsFactory(c => c.SelectedWith<TypeNameComponentSelector>()));
            container.Register(Component.For<IActivationFunctionLibrary>().ImplementedBy<DbActivationFunctionLibrary>());
            container.Register(Component.For<NeatGenomeFactory>()
                .DependsOn(Dependency.OnAppSettingsValue("inputNeuronCount", "InputNeuronCount"))
                .DependsOn(Dependency.OnAppSettingsValue("outputNeuronCount", "OutputNeuronCount")));
            container.Register(Component.For<IGenomeGenerator>().ImplementedBy<GenomeGenerator>());
            
            // DB
            container.Register(Component.For<Sharp2048DataModelContainer>().LifestyleTransient());

            // Neat Installation
            container.Register(Component.For<IGenomeDecoder<NeatGenome, IBlackBox>>().ImplementedBy<NeatGenomeDecoder>());
            container.Register(
                Component.For<NetworkActivationScheme>().UsingFactoryMethod(
                    (t, c) => NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(1)));
        }
    }

    public class TypeNameComponentSelector : DefaultTypedFactoryComponentSelector
    {
        protected override string GetComponentName(System.Reflection.MethodInfo method, object[] arguments)
        {
            if (method.Name == "Create" && arguments.Count() > 0)
            {
                return (string)arguments[0];
            }
            return base.GetComponentName(method, arguments);
        }
    }
}