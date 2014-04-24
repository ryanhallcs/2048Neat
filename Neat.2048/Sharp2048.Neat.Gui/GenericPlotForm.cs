using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

using SharpNeat.View;


namespace SharpNeatGUI
{
    public partial class GenericPlotForm : Form
    {
        public delegate PointPair[] PlotPointSource();

        PointPairList _ppl;
        PlotPointSource _plotPointSourceDelegate;

        public GenericPlotForm(PlotPointSource plotPointSourceDelegate)
        {
            InitializeComponent();
            _plotPointSourceDelegate = plotPointSourceDelegate;
            _ppl = new PointPairList();
            zed.GraphPane.AddCurve("", _ppl, Color.Black, SymbolType.None);
        }

        public void UpdatePlot()
        {
            var dataPointArr = _plotPointSourceDelegate();
            int count = dataPointArr.Length;

            if(_ppl.Count != count)
            {
                _ppl.Capacity = count;
                _ppl.Clear();
                for(int i=0; i<count; i++) 
                {
                    _ppl.Add(0, 0);
                }
            }

            for(int i=0; i<count; i++) 
            {
                _ppl[i].X = dataPointArr[i].X;
                _ppl[i].Y = dataPointArr[i].Y;
                _ppl[i].Tag = dataPointArr[i].Tag;
            }

            zed.AxisChange();
            Refresh();
        }
    }
}
