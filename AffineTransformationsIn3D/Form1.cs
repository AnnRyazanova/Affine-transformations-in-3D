using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AffineTransformationsIn3D.Primitives;

namespace AffineTransformationsIn3D
{
    public partial class Form1 : Form
    {
        private Graphics graphics;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(2048, 2048);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
            Point3D p = new Point3D(100, 100, 0);
            Transformation projection = Transformation.Projection();
            p.Draw(graphics, projection);
            pictureBox1.Refresh();
        }
    }
}
