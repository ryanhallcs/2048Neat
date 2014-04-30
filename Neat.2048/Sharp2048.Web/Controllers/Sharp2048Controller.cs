using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Sharp2048.Neat.Service;
using Sharp2048.Web.Models;

namespace Sharp2048.Web.Controllers
{
    public class Sharp2048Controller : Controller
    {
        private readonly ISharpNeat2048Service _neat2048Service;
        public Sharp2048Controller(ISharpNeat2048Service neat2048Service)
        {
            _neat2048Service = neat2048Service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoadGenome()
        {
            return View(new LoadGenomeViewModel());
        }

        [HttpPost]
        public ActionResult LoadGenome(LoadGenomeViewModel model)
        {
            var savedGenome = _neat2048Service.SaveNewGenome(model.Description, model.LoadedBy, model.GenomeXml);

            if (savedGenome == null)
            {
                ModelState.AddModelError("GenomeXml", "Could not parse xml");
            }

            return View(model);
        }
    }
}