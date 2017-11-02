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
    }
    
}
