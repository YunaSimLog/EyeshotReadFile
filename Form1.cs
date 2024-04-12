using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadFileSample
{
    public partial class Form1 : Form
    {
        Entity _entity;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            //devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/파트1.STL");
            devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/멀티 바디.STL");
            readSTL.DoWork();

            _entity = readSTL.Entities[0];
            //_entity.Selected = true;

            design1.Entities.Add(_entity, Color.Bisque);
            design1.ZoomFit();
            design1.SetView(devDept.Eyeshot.viewType.Isometric);

            string lineTypeName = "axesLineType";
            design1.LineTypes.Add(lineTypeName, new float[] { .5f, -.12f, .05f, -.12f });

            string axesLayer = "Axes";
            design1.Layers.Add(new Layer(axesLayer, Color.DeepPink, lineTypeName, 1, true));

            design1.Selection.Color = Color.DarkGreen;
            base.OnLoad(e);
        }
    }
}
