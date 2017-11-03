using System;
using System.Collections.Generic;
using System.Drawing;
using AffineTransformationsIn3D.Primitives;

namespace AffineTransformationsIn3D.Polyhedra
{
    class Icosahedron : Polyhedra
    {
        // кол-во вершин = 12
        private List<Point3D> points = new List<Point3D>();

        // кол-во граней = 20
        private List<Facet> facets = new List<Facet>();

        public List<Point3D> Points { get { return points; } set { points = value; } }
        public List<Facet> Facets { get { return facets; } set { facets = value; } }

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

		public Icosahedron(List<Point3D> points)
        {
            if (points.Count != 12) return;

            this.points = points;

            // середина
            var f1 = new Facet();  f1.FacetAddPoint(points[0]);  f1.FacetAddPoint(points[1]);  f1.FacetAddPoint(points[2]);  facets.Add(f1);
            var f2 = new Facet();  f2.FacetAddPoint(points[1]);  f2.FacetAddPoint(points[2]);  f2.FacetAddPoint(points[3]);  facets.Add(f2);
            var f3 = new Facet();  f3.FacetAddPoint(points[2]);  f3.FacetAddPoint(points[3]);  f3.FacetAddPoint(points[4]);  facets.Add(f3);
            var f4 = new Facet();  f4.FacetAddPoint(points[3]);  f4.FacetAddPoint(points[4]);  f4.FacetAddPoint(points[5]);  facets.Add(f4);
            var f5 = new Facet();  f5.FacetAddPoint(points[4]);  f5.FacetAddPoint(points[5]);  f5.FacetAddPoint(points[6]);  facets.Add(f5);
            var f6 = new Facet();  f6.FacetAddPoint(points[5]);  f6.FacetAddPoint(points[6]);  f6.FacetAddPoint(points[7]);  facets.Add(f6);
            var f7 = new Facet();  f7.FacetAddPoint(points[6]);  f7.FacetAddPoint(points[7]);  f7.FacetAddPoint(points[8]);  facets.Add(f7);
            var f8 = new Facet();  f8.FacetAddPoint(points[7]);  f8.FacetAddPoint(points[8]);  f8.FacetAddPoint(points[9]);  facets.Add(f8);
            var f9 = new Facet();  f9.FacetAddPoint(points[8]);  f9.FacetAddPoint(points[9]);  f9.FacetAddPoint(points[0]);  facets.Add(f9);
            var f10 = new Facet(); f10.FacetAddPoint(points[9]); f10.FacetAddPoint(points[0]); f10.FacetAddPoint(points[1]); facets.Add(f10);

            // верхняя часть
            var f11 = new Facet(); f11.FacetAddPoint(points[0]); f11.FacetAddPoint(points[10]); f11.FacetAddPoint(points[2]); facets.Add(f11);
            var f12 = new Facet(); f12.FacetAddPoint(points[2]); f12.FacetAddPoint(points[10]); f12.FacetAddPoint(points[4]); facets.Add(f12);
            var f13 = new Facet(); f13.FacetAddPoint(points[4]); f13.FacetAddPoint(points[10]); f13.FacetAddPoint(points[6]); facets.Add(f13);
            var f14 = new Facet(); f14.FacetAddPoint(points[6]); f14.FacetAddPoint(points[10]); f14.FacetAddPoint(points[8]); facets.Add(f14);
            var f15 = new Facet(); f15.FacetAddPoint(points[8]); f15.FacetAddPoint(points[10]); f15.FacetAddPoint(points[0]); facets.Add(f15);

            // нижняя часть
            var f16 = new Facet(); f16.FacetAddPoint(points[1]); f16.FacetAddPoint(points[11]); f16.FacetAddPoint(points[3]); facets.Add(f16);
            var f17 = new Facet(); f17.FacetAddPoint(points[3]); f17.FacetAddPoint(points[11]); f17.FacetAddPoint(points[5]); facets.Add(f17);
            var f18 = new Facet(); f18.FacetAddPoint(points[5]); f18.FacetAddPoint(points[11]); f18.FacetAddPoint(points[7]); facets.Add(f18);
            var f19 = new Facet(); f19.FacetAddPoint(points[7]); f19.FacetAddPoint(points[11]); f19.FacetAddPoint(points[9]); facets.Add(f19);
            var f20 = new Facet(); f20.FacetAddPoint(points[9]); f20.FacetAddPoint(points[11]); f20.FacetAddPoint(points[1]); facets.Add(f20);
        }

        private static List<Point3D> FindPoints(float size)
        {
            // радиус описанной сферы
            float R = (size * (float)Math.Sqrt(2.0 * (5.0 + Math.Sqrt(5.0)))) / 4;

            // радиус вписанной сферы
            float r = (size * (float)Math.Sqrt(3.0) * (float)(3.0 + Math.Sqrt(5.0))) / 12;

            List<Point3D> creatingPoints = new List<Point3D>();

			for (int i = 0; i < 5; ++i)
			{
				creatingPoints.Add(new Point3D(r * (float)Math.Cos(2 * Math.PI / 5 * i),
											   R / 2,
											   r * (float)Math.Sin(2 * Math.PI / 5 * i)));
				creatingPoints.Add(new Point3D(r * (float)Math.Cos(2 * Math.PI / 5 * i + 2 * Math.PI / 10),
											   -R / 2,
											   r * (float)Math.Sin(2 * Math.PI / 5 * i + 2 * Math.PI / 10)));
			}

			creatingPoints.Add(new Point3D(0, R, 0));
			creatingPoints.Add(new Point3D(0, -R, 0));

			/*
            creatingPoints.Add(new Point3D(r, R / 2, 0));          
            creatingPoints.Add(new Point3D(r / 2, - R / 2, r));                 
            creatingPoints.Add(new Point3D(0, R / 2, r));           
            creatingPoints.Add(new Point3D(- r / 2, - R / 2, r));                                      
            creatingPoints.Add(new Point3D(-r, R / 2, r / 2));          
            creatingPoints.Add(new Point3D(-r, - R / 2, - r / 2));                 
            creatingPoints.Add(new Point3D(-r / 2, R / 2, - r));           
            creatingPoints.Add(new Point3D(0, - R / 2, -r));                                       
            creatingPoints.Add(new Point3D(r / 2, R / 2, -r));         
            creatingPoints.Add(new Point3D(r, - R / 2, -r / 2));                 
            creatingPoints.Add(new Point3D(0, R, r));           
            creatingPoints.Add(new Point3D(0, -R, -r));   
			*/

			return creatingPoints;
        }

        public Icosahedron(float size) : this(FindPoints(size)) { }

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
