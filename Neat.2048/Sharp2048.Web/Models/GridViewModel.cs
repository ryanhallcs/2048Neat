using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sharp2048.Web.Models
{
    public class GridViewModel
    {
        public int Size { get; set; }

        public CellViewModel[] Cells { get; set; }
    }

    public class CellViewModel
    {
        public RowViewModel[] Rows { get; set; }
    }

    public class RowViewModel
    {
        public PositionViewModel[] Position { get; set; }
    }

    public class PositionViewModel
    {
        public int X { get; set; }

        public int Y { get; set; }
    }
}