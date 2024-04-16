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
            //LoadSTL();

            LoadSTEP();

            
            design1.ZoomFit();
            design1.SetView(devDept.Eyeshot.viewType.Isometric);

            string lineTypeName = "axesLineType";
            design1.LineTypes.Add(lineTypeName, new float[] { .5f, -.12f, .05f, -.12f });

            string axesLayer = "Axes";
            design1.Layers.Add(new Layer(axesLayer, Color.DeepPink, lineTypeName, 1, true));

            design1.SelectionFilterMode = selectionFilterType.Vertex
                              | selectionFilterType.Edge
                              | selectionFilterType.Face;

            //design1.Selection.Color = Color.DarkGreen;
            base.OnLoad(e);
        }

        private void LoadSTEP()
        {
            devDept.Eyeshot.Translators.ReadSTEP readSTEP = new devDept.Eyeshot.Translators.ReadSTEP(@"../../SampleFile/Part1.STEP");
            readSTEP.DoWork();

            var entity = readSTEP.Entities[0];
            Brep brep = entity as Brep;

            brep.Selected = true;
            brep.ClearFacesSelection();
            brep.SelectionMode = selectionFilterType.Face;

            design1.Entities.Add(brep, Color.Aqua);

            Console.WriteLine($"모델 면 수 {brep.Faces.Length}");

            for (int i = 0; i < brep.Faces.Length; i++)
            {
                if (i % 2 == 0)
                    continue;

                // 홀수 인덱스 면만 선택
                brep.SetFaceSelection(i, true);
            }

            design1.Invalidate();
        }

        private void LoadSTL()
        {
            //devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/파트1.STL");
            devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/멀티 바디.STL");
            readSTL.DoWork();

            _entity = readSTL.Entities[0];
            var mesh = (Mesh)_entity;

            //_entity.Selected = true;

            //mesh.SetFaceSelection(32, 2, true);
            var meshfaces = mesh.Faces;

            //mesh.Selected = true;

            design1.Entities.Add(_entity, Color.Bisque);
        }
    }
}