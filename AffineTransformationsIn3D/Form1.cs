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
		private Icosahedron curIcosahedron;
		private Polyhedra.Polyhedra curPolyhedron;

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
			curPolyhedron = new Tetrahedron(0.5f);
			//curPolyhedron = new Icosahedron(0.5f);
			scene.Add(curPolyhedron);
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
            var scalingPolyhedron = Transformation.Scale(scalingX, scalingY, scalingZ);
			// curTetrahedron.Apply(scalingTetrahedron);
			curPolyhedron.Apply(scalingPolyhedron);
            scenesRefresh();
        }

        private void Rotate(object sender, EventArgs e)
        {
            float rotatingX = (float)((double)numericUpDown4.Value / 180 * Math.PI);
            float rotatingY = (float)((double)numericUpDown5.Value / 180 * Math.PI);
            float rotatingZ = (float)((double)numericUpDown6.Value / 180 * Math.PI);
            var rotatingPolyhedron = Transformation.RotateX(rotatingX)
                * Transformation.RotateY(rotatingY)
                * Transformation.RotateZ(rotatingZ);
			curPolyhedron.Apply(rotatingPolyhedron);
            scenesRefresh();
        }

        private void Translate(object sender, EventArgs e)
        {
            float translatingX = (float)numericUpDown7.Value;
            float translatingY = (float)numericUpDown8.Value;
            float translatingZ = (float)numericUpDown9.Value;
            var scalingPolyhedron = Transformation.Translate(translatingX, translatingY, translatingZ);
			curPolyhedron.Apply(scalingPolyhedron);
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
			
			curPolyhedron.Apply(reflection);
            scenesRefresh();
        }


        private void Translate_around(double rot1, double rot2, double rot3)
        {
            Point3D p = curPolyhedron.Center;
            

            float translatingX = -p.X;
            float translatingY = -p.Y;
            float translatingZ = -p.Z;

            var scalingTetrahedron = Transformation.Translate(translatingX, translatingY, translatingZ);
            curPolyhedron.Apply(scalingTetrahedron);

            float rotatingX = (float)(rot1 / 180 * Math.PI);
            float rotatingY = (float)(rot2 / 180 * Math.PI);
            float rotatingZ = (float)(rot3 / 180 * Math.PI);
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

			p1.X = (float)numericUpDown13.Value;
			p1.Y = (float)numericUpDown14.Value;
			p1.Z = (float)numericUpDown15.Value;

			p2.X = (float)numericUpDown16.Value;
			p2.Y = (float)numericUpDown17.Value;
			p2.Z = (float)numericUpDown18.Value;

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
				float rotatingX = (float)((double)1 / 180 * Math.PI);
				float rotatingY = (float)((double)0 / 180 * Math.PI);
				float rotatingZ = (float)((double)0 / 180 * Math.PI);
				scalingTetrahedron = Transformation.RotateX(rotatingX)
					* Transformation.RotateY(rotatingY)
					* Transformation.RotateZ(rotatingZ);
				line.Apply(scalingTetrahedron);


				rot_x++;
			}

			int rot_y = 0;
			while (Math.Abs(line.A.X) > 0.01)
			{
				float rotatingX = (float)((double)0 / 180 * Math.PI);
				float rotatingY = (float)((double)1 / 180 * Math.PI);
				float rotatingZ = (float)((double)0 / 180 * Math.PI);
				scalingTetrahedron = Transformation.RotateX(rotatingX)
					* Transformation.RotateY(rotatingY)
					* Transformation.RotateZ(rotatingZ);
				line.Apply(scalingTetrahedron);
				
				rot_y++;
			}


			float rotatingX_1 = (float)((double)(360-rot_x%360) / 180 * Math.PI);
			float rotatingY_1 = (float)((double)(360-rot_y %360) / 180 * Math.PI);
			float rotatingZ_1 = (float)((double)(360-((int)numericUpDown19.Value%360)) / 180 * Math.PI);
			var scalingTetrahedron_1 = Transformation.RotateX(rotatingX_1)
				* Transformation.RotateY(rotatingY_1)
				* Transformation.RotateZ(rotatingZ_1);
			curTetrahedron.Apply(scalingTetrahedron);

			scalingTetrahedron = Transformation.Translate(cent.X, cent.Y, cent.Y);
			curTetrahedron.Apply(scalingTetrahedron);

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
	}


    
}
