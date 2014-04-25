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
.Create(@"Y:\ryanhallcs\2048neat\Neat.2048\Sharp2048.Neat.Gui\bin\Debug\champ_2432,00_20140425_170351.gnm.xml"))
            {
                genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, new NeatGenomeFactory(16, 4)).First();
            }

            var decoder = new NeatGenomeDecoder(NetworkActivationScheme.CreateCyclicFixedTimestepsScheme(4));
            var box = decoder.Decode(genome);

            var driver = new SharpNeatGenomeDriver(box, @"http://gabrielecirulli.github.io/2048/");
            driver.AutoRun();
        }
    }
}
