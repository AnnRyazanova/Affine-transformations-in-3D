using System;
using System.Collections.Generic;
using System.Drawing;
using AffineTransformationsIn3D.Primitives;

namespace AffineTransformationsIn3D.Polyhedra
{
    class Tetrahedron : IPrimitive
    {
        // первые три точки - основание тетраэдра, четвертая точка - его вершина
        private List<Point3D> points = new List<Point3D>();

        private List<Facet> facets = new List<Facet>();

        public List<Point3D> Points { get { return points; } set { points = value; } }
        public List<Facet> Facets { get { return facets; } set { facets = value; } }

        public Tetrahedron(List<Point3D> points)
        {
            if (points.Count != 4) return;
            
            this.points = points;

            // Основание тетраэдра
            var f1 = new Facet();
            f1.FacetAddPoint(points[0]);
            f1.FacetAddPoint(points[1]);
            f1.FacetAddPoint(points[2]);
            facets.Add(f1);

            // Левая грань
            var f2 = new Facet();
            f2.FacetAddPoint(points[1]);
            f2.FacetAddPoint(points[3]);
            f2.FacetAddPoint(points[0]);
            facets.Add(f2);

            // Правая грань
            var f3 = new Facet();
            f3.FacetAddPoint(points[2]);
            f3.FacetAddPoint(points[3]);
            f3.FacetAddPoint(points[1]);
            facets.Add(f3);

            // Передняя грань
            var f4 = new Facet();
            f4.FacetAddPoint(points[0]);
            f4.FacetAddPoint(points[3]);
            f4.FacetAddPoint(points[2]);
            facets.Add(f4);
        }

        private static List<Point3D> FindPoints(float size)
        {
            float h = (float)Math.Sqrt(2.0 / 3.0) * size;
            List<Point3D> creatingPoints = new List<Point3D>();

            creatingPoints.Add(new Point3D(-size / 2, 0, h *(float)(1.0 / 3.0)));
            creatingPoints.Add(new Point3D(0, 0, -h * (float)(2.0 / 3.0)));
            creatingPoints.Add(new Point3D(size / 2, 0, h * (float)(1.0 / 3.0)));
            creatingPoints.Add(new Point3D(0, h, 0));

            return creatingPoints;
        }

        public Tetrahedron(float size) : this(FindPoints(size)) { }


        public void Apply(Transformation t)
        {
            foreach (var facet in Facets)
                facet.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            if (Points.Count != 4) return;

            foreach (var facet in Facets)
                facet.Draw(g, projection, width, height);
        }

        public object Clone()
        {
            return new Tetrahedron(Points);
        }

    }
}
