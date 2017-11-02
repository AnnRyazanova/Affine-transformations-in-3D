using System.Collections.Generic;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    class Polygon: IPrimitive
    {
        private IList<Line> edges;

        public Polygon(IList<Line> edges)
        {
            this.edges = edges;
        }

        public Polygon(IList<Point3D> points)
        {
            edges = new List<Line>(points.Count);
            for (int i = 0; i < edges.Count; ++i)
                edges[i] = new Line(points[i], points[(i + 1) % points.Count]);
        }

        public void Apply(Transformation t)
        {
            foreach (var e in edges)
                e.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            foreach (var e in edges)
                e.Draw(g, projection, width, height);
        }
    }
}
