using System;
using System.Collections.Generic;
using System.Drawing;
using AffineTransformationsIn3D.Primitives;

namespace AffineTransformationsIn3D.Polyhedra
{
    class Tetrahedron : Polyhedra
    {
        // первые три точки - основание тетраэдра, четвертая точка - его вершина
        private List<Point3D> points = new List<Point3D>();

        private List<Facet> facets = new List<Facet>();

        public List<Point3D> Points { get { return points; } }
        public List<Facet> Facets { get { return facets; } }

		public Point3D Center
        {
            get
            {
				Point3D p = new Point3D(0, 0, 0);
				for (int i = 0; i < 4; i++)
				{
					p.X += Points[i].X;
					p.Y += Points[i].Y;
					p.Z += Points[i].Z;
				}
				p.X /= 4;
				p.Y /= 4;
				p.Z /= 4;
				return p;
            }
        }

        public Tetrahedron(float size) 
        {
            float h = (float)Math.Sqrt(2.0 / 3.0) * size;
            points = new List<Point3D>();

            points.Add(new Point3D(-size / 2, 0, h * (float)(1.0 / 3.0)));
            points.Add(new Point3D(0, 0, -h * (float)(2.0 / 3.0)));
            points.Add(new Point3D(size / 2, 0, h * (float)(1.0 / 3.0)));
            points.Add(new Point3D(0, h, 0));

            // Основание тетраэдра
            facets.Add(new Facet(new Point3D[] { points[0], points[1], points[2] }));
            // Левая грань
            facets.Add(new Facet(new Point3D[] { points[1], points[3], points[0] }));
            // Правая грань
            facets.Add(new Facet(new Point3D[] { points[2], points[3], points[1] }));
            // Передняя грань
            facets.Add(new Facet(new Point3D[] { points[0], points[3], points[2] }));
        }

        public void Apply(Transformation t)
        {
            foreach (var point in Points)
                point.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            foreach (var facet in Facets)
                facet.Draw(g, projection, width, height);
        }
    }
}
