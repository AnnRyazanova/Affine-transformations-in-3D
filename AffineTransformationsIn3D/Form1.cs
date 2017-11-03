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
		private Polyhedra.Polyhedra curPolyhedron = new Tetrahedron(0.5f);

        public Form1()
        {
            InitializeComponent();
            List<IPrimitive> scene = new List<IPrimitive>();
            var a = new Point3D(0, 0, 0);
            var b = new Point3D(0.8, 0, 0);
            var c = new Point3D(0, 0.8, 0);
            var d = new Point3D(0, 0, 0.8);
            scene.Add(a);
            scene.Add(b);
            scene.Add(c);
            scene.Add(d);
            scene.Add(new Line(a, b));
            scene.Add(new Line(a, c));
            scene.Add(new Line(a, d));
			scene.Add(curPolyhedron);
            sceneView1.Scene = scene;
            sceneView2.Scene = scene;
            sceneView3.Scene = scene;
            sceneView4.Scene = scene;
            sceneView1.Projection = Transformation.OrthogonalProjection();
            sceneView2.Projection = Transformation.OrthogonalProjection()
                * Transformation.RotateY(Math.PI / 2);
            sceneView3.Projection = Transformation.OrthogonalProjection()
                * Transformation.RotateX(-Math.PI / 2);
            sceneView4.Projection = Transformation.OrthogonalProjection()
                * Transformation.RotateY(Math.PI / 4)
                * Transformation.RotateX(-Math.PI / 4);
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
            double scalingX = (double)numericUpDown1.Value;
            double scalingY = (double)numericUpDown2.Value;
            double scalingZ = (double)numericUpDown3.Value;
            var scalingPolyhedron = Transformation.Scale(scalingX, scalingY, scalingZ);
			// curTetrahedron.Apply(scalingTetrahedron);
			curPolyhedron.Apply(scalingPolyhedron);
            scenesRefresh();
        }

        private void Rotate(object sender, EventArgs e)
        {
            double rotatingX = (double)numericUpDown4.Value / 180 * Math.PI;
            double rotatingY = (double)numericUpDown5.Value / 180 * Math.PI;
            double rotatingZ = (double)numericUpDown6.Value / 180 * Math.PI;
            var rotatingPolyhedron = Transformation.RotateX(rotatingX)
                * Transformation.RotateY(rotatingY)
                * Transformation.RotateZ(rotatingZ);
			curPolyhedron.Apply(rotatingPolyhedron);
            scenesRefresh();
        }

        private void Translate(object sender, EventArgs e)
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            var scalingPolyhedron = Transformation.Translate(translatingX, translatingY, translatingZ);
			curPolyhedron.Apply(scalingPolyhedron);
            scenesRefresh();
        }

        private void Reflect(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
            {
                MessageBox.Show("Выберите, относительно чего отразить многогранник");
                return;
            }
            Transformation reflection = null;
            if (radioButton1.Checked)
                reflection = Transformation.ReflectX();
            else if (radioButton2.Checked)
                reflection = Transformation.ReflectY();
            else if (radioButton3.Checked)
                reflection = Transformation.ReflectZ();
			curPolyhedron.Apply(reflection);
            scenesRefresh();
        }


        private void Translate_around(double rot1, double rot2, double rot3)
        {
            Point3D p = curPolyhedron.Center;
            

            double translatingX = -p.X;
            double translatingY = -p.Y;
            double translatingZ = -p.Z;

            var scalingTetrahedron = Transformation.Translate(translatingX, translatingY, translatingZ);
            curPolyhedron.Apply(scalingTetrahedron);

            double rotatingX = rot1 / 180 * Math.PI;
            double rotatingY = rot2 / 180 * Math.PI;
            double rotatingZ = rot3 / 180 * Math.PI;
            scalingTetrahedron = Transformation.RotateX(rotatingX)
                * Transformation.RotateY(rotatingY)
                * Transformation.RotateZ(rotatingZ);
            curPolyhedron.Apply(scalingTetrahedron);

            translatingX = p.X;
            translatingY = p.Y;
            translatingZ = p.Z;

            scalingTetrahedron = Transformation.Translate(translatingX, translatingY, translatingZ);
            curPolyhedron.Apply(scalingTetrahedron);
        }

        private void Translate_around_line()
        {
            Point3D p1 = new Point3D();
            Point3D p2 = new Point3D();

			p1.X = (double)numericUpDown13.Value;
			p1.Y = (double)numericUpDown14.Value;
			p1.Z = (double)numericUpDown15.Value;

			p2.X = (double)numericUpDown16.Value;
			p2.Y = (double)numericUpDown17.Value;
			p2.Z = (double)numericUpDown18.Value;

			Point3D cent = new Point3D();
            cent.X += (p1.X + p2.X) / 2;
            cent.Y += (p1.Y + p2.Y) / 2;
            cent.Z += (p1.Z + p2.Z) / 2;

			var line = new Line(p1, p2);
			sceneView1.Scene.Add(line);
			
			var scalingTetrahedron = Transformation.Translate(-cent.X, -cent.Y, -cent.Y);
			line.Apply(scalingTetrahedron);
			int rot_x = 0;
			while (Math.Abs(line.A.Y) > 0.01)
			{
                double rotatingX = 1.0 / 180 * Math.PI;
                double rotatingY = 0.0 / 180 * Math.PI;
                double rotatingZ = 0.0 / 180 * Math.PI;
				scalingTetrahedron = Transformation.RotateX(rotatingX)
					* Transformation.RotateY(rotatingY)
					* Transformation.RotateZ(rotatingZ);
				line.Apply(scalingTetrahedron);


				rot_x++;
			}

			int rot_y = 0;
			while (Math.Abs(line.A.X) > 0.01)
			{
				double rotatingX = 0.0 / 180 * Math.PI;
                double rotatingY = 1.0 / 180 * Math.PI;
                double rotatingZ = 0.0 / 180 * Math.PI;
				scalingTetrahedron = Transformation.RotateX(rotatingX)
					* Transformation.RotateY(rotatingY)
					* Transformation.RotateZ(rotatingZ);
				line.Apply(scalingTetrahedron);
				
				rot_y++;
			}


            double rotatingX_1 = ((double)(360-rot_x%360) / 180 * Math.PI);
            double rotatingY_1 = ((double)(360-rot_y %360) / 180 * Math.PI);
            double rotatingZ_1 = ((double)(360-((int)numericUpDown19.Value%360)) / 180 * Math.PI);
			var scalingTetrahedron_1 = Transformation.RotateX(rotatingX_1)
				* Transformation.RotateY(rotatingY_1)
				* Transformation.RotateZ(rotatingZ_1);
			curPolyhedron.Apply(scalingTetrahedron);

			scalingTetrahedron = Transformation.Translate(cent.X, cent.Y, cent.Y);
            curPolyhedron.Apply(scalingTetrahedron);

			scenesRefresh();
		}



		private void button5_Click(object sender, EventArgs e)
        {
            Translate_around((double)numericUpDown10.Value, (double)numericUpDown11.Value, (double)numericUpDown12.Value);
            scenesRefresh();
        }

		private void button6_Click(object sender, EventArgs e)
		{
			Translate_around_line();
		}

		private void radioButton5_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton5.Checked)
			{
				sceneView1.Scene.Remove(curPolyhedron);
				curPolyhedron = new Icosahedron(0.5);
				sceneView1.Scene.Add(curPolyhedron);
				scenesRefresh();
			}
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton4.Checked)
			{
				sceneView1.Scene.Remove(curPolyhedron);
				curPolyhedron = new Tetrahedron(0.5);
				sceneView1.Scene.Add(curPolyhedron);
				scenesRefresh();
			}
		}
	}


    
}
