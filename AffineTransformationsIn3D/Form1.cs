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
            scene.Add(new Tetrahedron(0.3f));
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
    }
}
