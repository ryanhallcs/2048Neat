using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sharp2048.Web.Models
{
    public class LoadGenomeViewModel
    {
        public string GenomeXml { get; set; }

        public string Description { get; set; }

        public string LoadedBy { get; set; }
    }
}