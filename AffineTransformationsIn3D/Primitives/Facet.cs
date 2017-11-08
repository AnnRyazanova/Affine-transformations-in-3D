using System.Collections.Generic;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    public class Facet : IPrimitive
    {
        private IList<Point3D> points = new List<Point3D>();
        private IList<Line> lines = new List<Line>();

        public IList<Point3D> Points { get { return points; } }
        public IList<Line> Lines { get { return lines; } }

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

        public Facet(IList<Point3D> points)
        {
            this.points = points;
            for (int i = 0; i < points.Count - 1; ++i)
                lines.Add(new Line(points[i], points[i + 1]));
            lines.Add(new Line(points[points.Count - 1], points[0]));
        }

        public void Apply(Transformation t)
        {
            foreach (var point in Points)
                point.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            foreach (var line in lines)
                line.Draw(g, projection, width, height);
        }
    }
}
