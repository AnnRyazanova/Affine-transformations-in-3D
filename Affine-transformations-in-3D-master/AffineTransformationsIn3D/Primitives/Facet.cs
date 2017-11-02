using System;
using System.Collections.Generic;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    class Facet : IPrimitive
    {
        private List<Point3D> points = new List<Point3D>();

        public List<Point3D> Points { get { return points; } set { points = value; } }

        public Facet(){}

        public Facet(List<Point3D> points)
        {
            this.points = points;
        }

        public void FacetAddPoint(Point3D p)
        {
            points.Add(p);
        }

        public void Apply(Transformation t)
        {
            foreach (var point in Points)
                point.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            if (Points.Count == 1)
                Points[0].Draw(g, projection, width, height);
            else
            {
                for (int i = 0; i < Points.Count - 1; ++i)
                {
                    var line = new Line(Points[i], Points[i + 1]);
                    line.Draw(g, projection, width, height);
                }
                (new Line(Points[Points.Count - 1], Points[0])).Draw(g, projection, width, height);
            }
        }
    }
}
