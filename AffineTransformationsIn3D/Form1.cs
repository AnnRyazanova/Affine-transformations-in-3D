using System.Windows.Forms;
using System.Collections.Generic;
using System;
using AffineTransformationsIn3D.Primitives;
using AffineTransformationsIn3D.Polyhedra;
using System.Drawing;

namespace AffineTransformationsIn3D
{
    public partial class Form1 : Form
    {
        private Tetrahedron curTetrahedron;
        //private Icosahedron curIcosahedron;

        public Form1()
        {
            InitializeComponent();
            List<IPrimitive> scene = new List<IPrimitive>();
            var a = new Point3D(0, 0, 0);
            var b = new Point3D(0.8f, 0, 0);
            var c = new Point3D(0, 0.8f, 0);
            var d = new Point3D(0, 0, 0.8f);
            scene.Add(a);
            scene.Add(b);
            scene.Add(c);
            scene.Add(d);
            scene.Add(new Line(a, b));
            scene.Add(new Line(a, c));
            scene.Add(new Line(a, d));
            curTetrahedron = new Tetrahedron(0.5f);
            scene.Add(curTetrahedron);
            //curIcosahedron = new Icosahedron(0.5f);
            //scene.Add(curIcosahedron);
            sceneView1.Scene = scene;
            sceneView2.Scene = scene;
            sceneView3.Scene = scene;
            sceneView4.Scene = scene;
            sceneView1.Projection = Transformation.OrthogonalProjection();
            sceneView2.Projection = Transformation.OrthogonalProjection()
                * Transformation.RotateY((float)Math.PI / 2);
            sceneView3.Projection = Transformation.OrthogonalProjection()
                * Transformation.RotateX(-(float)Math.PI / 2);
            sceneView4.Projection = Transformation.OrthogonalProjection()
                * Transformation.RotateY((float)Math.PI / 4)
                * Transformation.RotateX(-(float)Math.PI / 4);
        }

        private void scenesRefresh()
        {
            sceneView1.Refresh();
            sceneView2.Refresh();
            sceneView3.Refresh();
            sceneView4.Refresh();
        }

        private void Scale(object sender, EventArgs e)
        {
            float scalingX = (float)numericUpDown1.Value;
            float scalingY = (float)numericUpDown2.Value;
            float scalingZ = (float)numericUpDown3.Value;
            var scalingTetrahedron = Transformation.Scale(scalingX, scalingY, scalingZ);
            curTetrahedron.Apply(scalingTetrahedron);
            scenesRefresh();
        }

        private void Rotate(object sender, EventArgs e)
        {
            float rotatingX = (float)((double)numericUpDown4.Value / 180 * Math.PI);
            float rotatingY = (float)((double)numericUpDown5.Value / 180 * Math.PI);
            float rotatingZ = (float)((double)numericUpDown6.Value / 180 * Math.PI);
            var scalingTetrahedron = Transformation.RotateX(rotatingX)
                * Transformation.RotateY(rotatingY)
                * Transformation.RotateZ(rotatingZ);
            curTetrahedron.Apply(scalingTetrahedron);
            scenesRefresh();
        }

        private void Translate(object sender, EventArgs e)
        {
            float translatingX = (float)numericUpDown7.Value;
            float translatingY = (float)numericUpDown8.Value;
            float translatingZ = (float)numericUpDown9.Value;
            var scalingTetrahedron = Transformation.Translate(translatingX, translatingY, translatingZ);
            curTetrahedron.Apply(scalingTetrahedron);
            scenesRefresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transformation reflection = new Transformation();

            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
            {
                MessageBox.Show("Выберите, относительно чего отразить многогранник");
                return;
            }
            if (radioButton1.Checked)
                reflection = Transformation.ReflectX();
            else if (radioButton2.Checked)
                reflection = Transformation.ReflectY();
            else if (radioButton3.Checked)
                reflection = Transformation.ReflectZ();

            curTetrahedron.Apply(reflection);
            scenesRefresh();
        }


        private void Translate_around()
        {
            Point3D p = new Point3D( 0, 0, 0 );
            for (int i = 0; i < 4; i++)
            {
                p.X += curTetrahedron.Points[i].X;
                p.Y += curTetrahedron.Points[i].Y;
                p.Z += curTetrahedron.Points[i].Z;
            }

            p.X /= 4;
            p.Y /= 4;
            p.Z /= 4;

            float translatingX = -p.X;
            float translatingY = -p.Y;
            float translatingZ = -p.Z;

            var scalingTetrahedron = Transformation.Translate(translatingX, translatingY, translatingZ);
            curTetrahedron.Apply(scalingTetrahedron);

            float rotatingX = (float)((double)numericUpDown10.Value / 180 * Math.PI);
            float rotatingY = (float)((double)numericUpDown11.Value / 180 * Math.PI);
            float rotatingZ = (float)((double)numericUpDown12.Value / 180 * Math.PI);
            scalingTetrahedron = Transformation.RotateX(rotatingX)
                * Transformation.RotateY(rotatingY)
                * Transformation.RotateZ(rotatingZ);
            curTetrahedron.Apply(scalingTetrahedron);

            translatingX = p.X;
            translatingY = p.Y;
            translatingZ = p.Z;

            scalingTetrahedron = Transformation.Translate(translatingX, translatingY, translatingZ);
            curTetrahedron.Apply(scalingTetrahedron);
        }

        /*private void Translate_around_line()
        {
            Point3D p1 = new Point3D(0, 0, 0);
            Point3D p2 = new Point3D(0, 100, 0);

            Point3D cent = new Point3D();
            cent.X += (p1.X + p2.X) / 2;
            cent.Y += (p1.Y + p2.Y) / 2;
            cent.Z += (p1.Z + p2.Z) / 2;

            List<IPrimitive> scene = new List<IPrimitive>();
            scene.Add(p1);
            scene.Add(p1);
            curTetrahedron = new Tetrahedron(0.5f);
            scene.Add(curTetrahedron);

            var scalingTetrahedron = Transformation.Translate(cent.X, cent.Y, cent.Y);
            curTetrahedron.Apply(scalingTetrahedron);

            Math.Acos()

            float rotatingX = (float)((double)numericUpDown10.Value / 180 * Math.PI);
            float rotatingY = (float)((double)numericUpDown11.Value / 180 * Math.PI);
            float rotatingZ = (float)((double)numericUpDown12.Value / 180 * Math.PI);
            scalingTetrahedron = Transformation.RotateX(rotatingX)
                * Transformation.RotateY(rotatingY)
                * Transformation.RotateZ(rotatingZ);
            curTetrahedron.Apply(scalingTetrahedron);

            translatingX = p.X;
            translatingY = p.Y;
            translatingZ = p.Z;

            scalingTetrahedron = Transformation.Translate(translatingX, translatingY, translatingZ);
            curTetrahedron.Apply(scalingTetrahedron);
            
        }*/



        private void button5_Click(object sender, EventArgs e)
        {
            Translate_around();
            scenesRefresh();
        }
    }


    
}
