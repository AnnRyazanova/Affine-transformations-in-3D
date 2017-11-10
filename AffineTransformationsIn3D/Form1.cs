using System.Windows.Forms;
using System.Collections.Generic;
using System;
using AffineTransformationsIn3D.Primitives;
using System.IO;

namespace AffineTransformationsIn3D
{
    public partial class Form1 : Form
    {
        private IPrimitive Model
        {
            get
            {
                var scene = sceneView1.Scene;
                return scene[scene.Count - 1];
            }

            set
            {
                var scene = sceneView1.Scene;
                scene.RemoveAt(scene.Count - 1);
                scene.Add(value);
                ScenesRefresh();
            }
        }

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
            scene.Add(Polyhedron.MakeTetrahedron(0.5f));
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

        private void ScenesRefresh()
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
            Model.Apply(
                Transformation.Scale(scalingX, scalingY, scalingZ));
            ScenesRefresh();
        }

        private void Rotate(object sender, EventArgs e)
        {
            double rotatingX = (double)numericUpDown4.Value / 180 * Math.PI;
            double rotatingY = (double)numericUpDown5.Value / 180 * Math.PI;
            double rotatingZ = (double)numericUpDown6.Value / 180 * Math.PI;
            Model.Apply(Transformation.RotateX(rotatingX)
                * Transformation.RotateY(rotatingY)
                * Transformation.RotateZ(rotatingZ));
            ScenesRefresh();
        }

        private void Translate(object sender, EventArgs e)
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            Model.Apply(
                Transformation.Translate(translatingX, translatingY, translatingZ));
            ScenesRefresh();
        }

        private void Reflect(object sender, EventArgs e)
        {
            Transformation reflection;
            if (radioButton1.Checked)
                reflection = Transformation.ReflectX();
            else if (radioButton2.Checked)
                reflection = Transformation.ReflectY();
            else if (radioButton3.Checked)
                reflection = Transformation.ReflectZ();
            else throw new Exception("Unreachable statement");
            Model.Apply(reflection);
            ScenesRefresh();
        }

        private void RotateAroundCenter(object sender, EventArgs e)
        {
            double rotX = (double)numericUpDown10.Value;
            double rotY = (double)numericUpDown11.Value;
            double rotZ = (double)numericUpDown12.Value;
            Point3D p = Model.Center;
            Model.Apply(Transformation.Translate(-p.X, -p.Y, -p.Z)
                * Transformation.RotateX(rotX / 180 * Math.PI)
                * Transformation.RotateY(rotY / 180 * Math.PI)
                * Transformation.RotateZ(rotZ / 180 * Math.PI)
                * Transformation.Translate(p.X, p.Y, p.Z));
            ScenesRefresh();
        }

        private void TranslateAroundLine(object sender, EventArgs e)
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
            var scalingTetrahedron = Transformation.Translate(-cent.X, -cent.Y, -cent.Z);
            line.Apply(scalingTetrahedron);

            int rot_x = 0;
            while (Math.Abs(line.A.Y) > 0.01 || Math.Abs(line.B.Y) > 0.01)
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
            while (Math.Abs(line.A.X) > 0.01 || Math.Abs(line.B.X) > 0.01)
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

            double rotatingX_1 = ((double)((360 - (rot_x))) / 180 * Math.PI);
            double rotatingY_1 = ((double)((360 - (rot_x))) / 180 * Math.PI);
            double rotatingZ_1 = ((double)(numericUpDown19.Value) / 180 * Math.PI);
            var scalingTetrahedron_1 = Transformation.RotateX(rotatingX_1)
                * Transformation.RotateY(rotatingY_1)
                * Transformation.RotateZ(rotatingZ_1);
            Model.Apply(scalingTetrahedron_1);

            scalingTetrahedron = Transformation.Translate(cent.X, cent.Y, cent.Z);
            Model.Apply(scalingTetrahedron);
            
            ScenesRefresh();
        }

        private void ChangeModel(object sender, EventArgs e)
        {
            var dialog = new FormChangeModel();
            if (DialogResult.OK != dialog.ShowDialog()) return;
            if (null == dialog.SelectedModel) return;
            Model = dialog.SelectedModel;
        }

        private double F(double x, double y)
        {
            return (x*x * y) / (x*x*x*x + y*y);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<IPrimitive> scene = new List<IPrimitive>();

            var p1 = new Point3D(0, 0, F(0,0));
            scene.Add(p1);

            for (double i = 0.1; i < 10; i+=0.01)
                for (double j = 0.1; j < 10; j+=0.01)
                {
                    var p2 = new Point3D(i, j, F(i, j));
                    scene.Add(new Line(p1, p2));
                    p1 = p2;
                }

            scene.Add(Polyhedron.MakeTetrahedron(0.5f));
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

            ScenesRefresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string info = "# File Created: " + DateTime.Now.ToString() + "\r\n";
                    
                    foreach (var point in ((Polyhedron)Model).Points)
                        info += "v " + Math.Round(point.X, 2, MidpointRounding.AwayFromZero) 
                                     + " " + Math.Round(point.Y, 2, MidpointRounding.AwayFromZero) 
                                     + " " + Math.Round(point.Z, 2, MidpointRounding.AwayFromZero) + "\r\n";
                    info += "# " + ((Polyhedron)Model).Points.Count + " vertices\r\n";


                    foreach (var seq in ((Polyhedron)Model).PointsSequence)
                    {
                        info += "f ";
                        for (int i = 0; i < seq.Count; ++i)
                            info += seq[i] + " ";
                        info += "\r\n";
                    }
                    info += "# " + ((Polyhedron)Model).Facets.Count + " polygons\r\n";

                    File.WriteAllText(saveDialog.FileName, info);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        
    }
}
