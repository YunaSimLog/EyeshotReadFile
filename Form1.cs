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
        Brep _brep;

        enum LoadType
        {
            STL, STEP
        }

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            
            base.OnLoad(e);
        }

        private void LoadSTEP()
        {
            devDept.Eyeshot.Translators.ReadSTEP readSTEP = new devDept.Eyeshot.Translators.ReadSTEP(@"../../SampleFile/Part1.STEP");
            readSTEP.DoWork();

            var entity = readSTEP.Entities[0];
            _brep = entity as Brep;

            _brep.Selected = true;
            _brep.ClearFacesSelection();
            _brep.SelectionMode = selectionFilterType.Face;

            design1.Entities.Add(_brep, Color.Aqua);

            Console.WriteLine($"모델 면 수 {_brep.Faces.Length}");

            for (int i = 0; i < _brep.Faces.Length; i++)
            {
                if (i % 2 == 0)
                    continue;

                // 홀수 인덱스 면만 선택
                _brep.SetFaceSelection(i, true);
            }
        }

        private void LoadSTL()
        {
            //devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/파트1.STL");
            devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/멀티 바디.STL");
            readSTL.DoWork();

            _entity = readSTL.Entities[0];
            var mesh = (Mesh)_entity;


            //mesh.SetFaceSelection(32, 2, true);
            var meshfaces = mesh.Faces;

            //mesh.Selected = true;

            design1.Entities.Add(_entity, Color.Bisque);
        }

        private void btnShowAxes_Click(object sender, EventArgs e)
        {
            if (_brep == null)
                return;

            string lineTypeName = "axesLineType";
            design1.LineTypes.Add(lineTypeName, new float[] { .5f, -.12f, .05f, -.12f });

            string axesLayer = "Axes";
            design1.Layers.Add(new Layer(axesLayer, Color.DeepPink, lineTypeName, 1, true));

            design1.SelectionFilterMode = selectionFilterType.Vertex
                              | selectionFilterType.Edge
                              | selectionFilterType.Face;

            List<Entity> entities = new List<Entity>();

            foreach (Brep.Face face in _brep.Faces)
            {
                AnalyticSurf analyticSurf = face.Surface;
                CylindricalSurf cylindricalSurf = analyticSurf as CylindricalSurf;
                if (cylindricalSurf == null)
                    continue;

                Point3D boxMin, boxMax;
                boxMax= face.Tessellation.BoxMax;
                boxMin = face.Tessellation.BoxMin;
                Size3D boxSize = face.Tessellation.BoxSize;

                Segment3D segment = new Segment3D(cylindricalSurf.Plane.Origin, cylindricalSurf.Plane.Origin + cylindricalSurf.Plane.AxisZ);

                double k = 0.5;

                double myBoxMinX = boxMin.X - (boxSize.X * k);
                double myBoxMinY = boxMin.Y - (boxSize.Y * k);
                double myBoxMinZ = boxMin.Z - (boxSize.Z * k);

                double myBoxMaxX = boxMax.X + (boxSize.X * k);
                double myBoxMaxY = boxMax.Y + (boxSize.Y * k);
                double myBoxMaxZ = boxMax.Z + (boxSize.Z * k);

                double p1 = segment.Project(new Point3D(myBoxMinX, myBoxMinY, myBoxMinZ));
                Point3D pp1 = segment.PointAt(p1);

                double p2 = segment.Project(new Point3D(myBoxMaxX, myBoxMaxY, myBoxMaxZ));
                Point3D pp2 = segment.PointAt(p2);

                Line line = new Line(pp1, pp2);
                entities.Add(line);
            }

            design1.Entities.AddRange(entities, axesLayer);
            design1.Invalidate();
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            string ctrlName = ((Button)sender).Name;

            LoadType loadType = LoadType.STEP;
            if (ctrlName == btnLoadSTL.Name)
                loadType = LoadType.STL;

            LoadData(loadType);
        }

        private void LoadData(LoadType loadType)
        {
            design1.Entities.Clear();

            switch (loadType)
            {
                case LoadType.STL:
                    LoadSTL();
                    break;
                case LoadType.STEP:
                    LoadSTEP();
                    break;
                default:
                    break;
            }

            design1.ZoomFit();
            design1.SetView(devDept.Eyeshot.viewType.Isometric);
            design1.Invalidate();
        }

    }
}