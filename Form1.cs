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
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/파트1.STL");
            devDept.Eyeshot.Translators.ReadSTL readSTL = new devDept.Eyeshot.Translators.ReadSTL(@"../../SampleFile/멀티 바디.STL");
            readSTL.DoWork();

            // 파일을 읽을 때, 엔티티를 여러개 가지고 오는 경우도 있나?? 멀티 바디 형태일 때도 1개의 엔티티만 존재
            for (int i = 0; i < readSTL.Entities.Length; i++)
            {
                Console.WriteLine(readSTL.Entities[i].MaterialName);
            }

            var entity = readSTL.Entities[0];

            design1.Entities.Add(entity, Color.Bisque);
        }
    }
}
