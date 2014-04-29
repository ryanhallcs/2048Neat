using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sharp2048.Neat.Service;

namespace Sharp2048.Web.Controllers
{
    public class Sharp2048Controller : Controller
    {
        private readonly SharpNeat2048Service _neat2048Service;
        public Sharp2048Controller(SharpNeat2048Service neat2048Service)
        {
            _neat2048Service = neat2048Service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}