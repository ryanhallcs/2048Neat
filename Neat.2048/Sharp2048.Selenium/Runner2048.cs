using NUnit.Framework;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.Neat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sharp2048.Selenium
{
    [TestFixture]
    public class Runner2048
    {
        [Test]
        public void Run()
        {
            NeatGenome genome = null;
            using (XmlReader xr = 
                XmlReader
.Create(@"..\..\..\Sharp2048.Neat.Gui\bin\Release\champ_5621,10_20140426_191319.gnm.xml"))
            {
                genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, new NeatGenomeFactory(16, 4)).First();
            }

            var decoder = new NeatGenomeDecoder(NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(1));
            var box = decoder.Decode(genome);

            var driver = new SharpNeatGenomeDriver(box, @"http://gabrielecirulli.github.io/2048/");
            driver.AutoRun();
        }
    }
}
