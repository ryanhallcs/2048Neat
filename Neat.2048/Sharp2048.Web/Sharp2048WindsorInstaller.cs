using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sharp2048.Data;
using Sharp2048.Neat.Service;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;

namespace Sharp2048.Web
{
    public class Sharp2048WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Controllers
            container.Register(
                Classes.FromAssemblyContaining<Sharp2048WindsorInstaller>().BasedOn<IController>().LifestyleTransient());

            // Sharp Neat service
            container.Register(
                Component.For<ISharpNeat2048Service>().ImplementedBy<SharpNeat2048Service>().LifestyleTransient());

            // DB
            container.Register(Component.For<Sharp2048DataModelContainer>().LifestyleTransient());

            // Neat Installation
            container.Register(Component.For<IGenomeDecoder<NeatGenome, IBlackBox>>().ImplementedBy<NeatGenomeDecoder>());
            container.Register(
                Component.For<NetworkActivationScheme>().UsingFactoryMethod(
                    (t, c) => NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(1)));
        }
    }
}