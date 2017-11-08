using System.Collections.Generic;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    class Facet : IPrimitive
    {
        private IList<Point3D> points = new List<Point3D>();

        public IList<Point3D> Points { get { return points; } set { points = value; } }

        public Point3D Center
        {
            get
            {
                var center = new Point3D();
                foreach (var p in points)
                {
                    center.X += p.X;
                    center.Y += p.Y;
                    center.Z += p.Z;
                }
                center.X /= points.Count;
                center.Y /= points.Count;
                center.Z /= points.Count;
                return center;
            }
        }

        public Facet(){}

        public Facet(IList<Point3D> points)
        {
            this.points = points;
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
