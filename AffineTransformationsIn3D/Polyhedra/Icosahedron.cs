using System;
using System.Collections.Generic;
using System.Drawing;
using AffineTransformationsIn3D.Primitives;

namespace AffineTransformationsIn3D.Polyhedra
{
    class Icosahedron : IPolyhedron
    {
        // кол-во вершин = 12
        private List<Point3D> points = new List<Point3D>();

        // кол-во граней = 20
        private List<Facet> facets = new List<Facet>();

        public List<Point3D> Points { get { return points; } }
        public List<Facet> Facets { get { return facets; } }

		public Point3D Center
		{
			get
			{
				Point3D p = new Point3D(0, 0, 0);
				for (int i = 0; i < 12; i++)
				{
					p.X += Points[i].X;
					p.Y += Points[i].Y;
					p.Z += Points[i].Z;
				}
				p.X /= 12;
				p.Y /= 12;
				p.Z /= 12;
				return p;
			}
		}

        public Icosahedron(double size)
        {
            // радиус описанной сферы
            double R = (size * Math.Sqrt(2.0 * (5.0 + Math.Sqrt(5.0)))) / 4;

            // радиус вписанной сферы
            double r = (size * Math.Sqrt(3.0) * (3.0 + Math.Sqrt(5.0))) / 12;

            points = new List<Point3D>();

            for (int i = 0; i < 5; ++i)
            {
                points.Add(new Point3D(
                    r * Math.Cos(2 * Math.PI / 5 * i),
                    R / 2,
                    r * Math.Sin(2 * Math.PI / 5 * i)));
                points.Add(new Point3D(
                    r * Math.Cos(2 * Math.PI / 5 * i + 2 * Math.PI / 10),
                    -R / 2,
                    r * Math.Sin(2 * Math.PI / 5 * i + 2 * Math.PI / 10)));
            }

            points.Add(new Point3D(0, R, 0));
            points.Add(new Point3D(0, -R, 0));

            // середина
            for (int i = 0; i < 10; ++i)
                facets.Add(new Facet(new Point3D[] { points[i], points[(i + 1) % 10], points[(i + 2) % 10] }));

            for (int i = 0; i < 5; ++i)
            {
                // верхняя часть
                facets.Add(new Facet(new Point3D[] { points[2 * i], points[10], points[(2 * (i + 1)) % 10] }));
                // нижняя часть
                facets.Add(new Facet(new Point3D[] { points[2 * i + 1], points[10], points[(2 * (i + 1) + 1) % 10] }));
            }
        }

        public void Apply(Transformation t)
        {
            foreach (var point in Points)
                point.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            if (Points.Count != 12) return;
			
            foreach (var facet in Facets)
                facet.Draw(g, projection, width, height);
        }
    }
}
