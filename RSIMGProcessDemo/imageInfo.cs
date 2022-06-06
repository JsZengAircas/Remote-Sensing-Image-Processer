using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class imageInfo : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public imageInfo()
        {
            InitializeComponent();
        }
        public static bool ImaInfoSta = true;
        private void imageInfo_Load(object sender, EventArgs e)
        {
            RSImage op = new Test.RSImage();//创建对象

            richTextBox1.Text = op.getImageInfo(this.richTextBox1);
        }

        private void imageInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            ImaInfoSta = false;
        }
    }
}
