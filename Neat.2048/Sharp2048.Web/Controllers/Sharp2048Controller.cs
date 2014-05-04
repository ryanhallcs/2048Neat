using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
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
            model = model ?? new LoadGenomeViewModel();

            var savedGenome = _neat2048Service.SaveNewGenome(model.Description, model.LoadedBy, model.GenomeXml);

            if (savedGenome == null)
            {
                ModelState.AddModelError("GenomeXml", "Could not parse xml into a valid NeatGenome");
            }
            if (String.IsNullOrEmpty(model.LoadedBy))
            {
                ModelState.AddModelError("LoadedBy", "Loaded By cannot be null");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Play", new {genomeId = savedGenome.GenomeIdentifier});
            }

            return View(model);
        }

        public ActionResult Play(Guid? genomeId)
        {
            return View(genomeId);
        }

        public ActionResult ProcessNextState(string state, Guid? genomeId)
        {
            var serviceState = _convertJsonState(state);
            if (!genomeId.HasValue || serviceState == null)
            {
                return new EmptyResult();
            }
            var nextMove = _neat2048Service.ProcessMove(serviceState, genomeId.Value);

            return new JsonResult {Data = new { result = nextMove.ToString()}};
        }

        private int[,] _convertJsonState(string jsonState)
        {
            GridViewModel gridModel = null;
            try
            {
                gridModel = JsonConvert.DeserializeObject<GridViewModel>(jsonState);
            }
            catch (Exception)
            {
                return null;
            }
            var result = new int[gridModel.Size,gridModel.Size];
            for (int i = 0; i < gridModel.Size; i++)
                for (int j = 0; j < gridModel.Size; j++)
                    if (gridModel.Cells[i, j] != null)
                        result[i, j] = gridModel.Cells[i, j].Value;
            return result;
        }
    }
}