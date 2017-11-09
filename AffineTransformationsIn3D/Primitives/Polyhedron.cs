using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    public struct Triple
    {
        public int first;
        public int second;
        public int third;

        public Triple(int f, int s, int t)
        {
            first = f;
            second = s;
            third = t;
        }
    };


    public class Polyhedron : IPrimitive
    {
        private IList<Point3D> points = new List<Point3D>();
        
        private IList<Facet> facets = new List<Facet>();

        private IList<Triple> pointsSequence = new List<Triple>();

        public IList<Point3D> Points { get { return points; } }
        public IList<Facet> Facets { get { return facets; } }
        public IList<Triple> PointsSequence { get { return pointsSequence; } }

        public Point3D Center
        {
            get
            {
                Point3D center = new Point3D(0, 0, 0);
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

        public Polyhedron(IList<Point3D> points, IList<Facet> facets)
        {
            this.points = points;
            this.facets = facets;
        }

        public Polyhedron(IList<Point3D> points, IList<Facet> facets, IList<Triple> pointsSequence)
        {
            this.points = points;
            this.facets = facets;
            this.pointsSequence = pointsSequence;
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

        

        public static IPrimitive MakeIcosahedron(double size)
        {
            var pointsSequence = new List<Triple>();
            var points = new List<Point3D>();
            var facets = new List<Facet>();
            
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
            {
                facets.Add(new Facet(new Point3D[] { points[i], points[(i + 1) % 10], points[(i + 2) % 10] }));
                Triple triple = new Triple(i, (i + 1) % 10, (i + 2) % 10);
                pointsSequence.Add(triple);
            }

            for (int i = 0; i < 5; ++i)
            {
                // верхняя часть
                facets.Add(new Facet(new Point3D[] { points[2 * i], points[10], points[(2 * (i + 1)) % 10] }));
                Triple triple = new Triple(2 * i, 10, (2 * (i + 1)) % 10);
                pointsSequence.Add(triple);
                // нижняя часть
                facets.Add(new Facet(new Point3D[] { points[2 * i + 1], points[11], points[(2 * (i + 1) + 1) % 10] }));
                triple = new Triple(2 * i + 1, 11, (2 * (i + 1) + 1) % 10);
                pointsSequence.Add(triple);
            }

            return new Polyhedron(points, facets, pointsSequence);
        }

        public void IcosahedronFacet()
        {
            // середина
            for (int i = 0; i < 10; ++i)
                facets.Add(new Facet(new Point3D[] { points[i], points[(i + 1) % 10], points[(i + 2) % 10] }));

            for (int i = 0; i < 5; ++i)
            {
                // верхняя часть
                facets.Add(new Facet(new Point3D[] { points[2 * i], points[10], points[(2 * (i + 1)) % 10] }));
                // нижняя часть
                facets.Add(new Facet(new Point3D[] { points[2 * i + 1], points[11], points[(2 * (i + 1) + 1) % 10] }));
            }
        }

        public static IPrimitive MakeTetrahedron(double size)
        {
            var points = new List<Point3D>();
            var facets = new List<Facet>();
            var pointsSequence = new List<Triple>();

            double h = Math.Sqrt(2.0 / 3.0) * size;
            points = new List<Point3D>();

            points.Add(new Point3D(-size / 2, 0, h / 3));
            points.Add(new Point3D(0, 0, -h * 2 / 3));
            points.Add(new Point3D(size / 2, 0, h / 3));
            points.Add(new Point3D(0, h, 0));

            // Основание тетраэдра
            facets.Add(new Facet(new Point3D[] { points[0], points[1], points[2] }));
            Triple triple = new Triple(0, 1, 2);
            pointsSequence.Add(triple);

            // Левая грань
            facets.Add(new Facet(new Point3D[] { points[1], points[3], points[0] }));
            triple = new Triple(1, 3, 0);
            pointsSequence.Add(triple);
            
            // Передняя грань
            facets.Add(new Facet(new Point3D[] { points[0], points[3], points[2] }));
            triple = new Triple(0, 3, 2);
            pointsSequence.Add(triple);

            // Правая грань
            facets.Add(new Facet(new Point3D[] { points[2], points[3], points[1] }));
            triple = new Triple(2, 3, 1);
            pointsSequence.Add(triple);

            return new Polyhedron(points, facets, pointsSequence);
        }

        public static IPrimitive MakeRotationFigure(IList<Point3D> initial, int axis, int density)
        {
            Debug.Assert(0 <= axis && axis < 3);
            var points = new List<Point3D>();
            var facets = new List<Facet>();
            points.AddRange(initial);
            var rotatedPoints = new List<Point3D>();
            Func<double, Transformation> rotation;
            switch (axis)
            {
                case 0: rotation = Transformation.RotateX; break;
                case 1: rotation = Transformation.RotateY; break;
                default: rotation = Transformation.RotateZ; break;
            }
            for (int i = 0; i < density; ++i)
            {
                double angle = 2 * Math.PI * i / (density - 1);
                foreach (var p in initial)
                    rotatedPoints.Add(p.Transform(rotation(angle)));
                points.AddRange(rotatedPoints);
                rotatedPoints.Clear();
            }
            var n = initial.Count;
            for (int i = 0; i < density; ++i)
                for (int j = 0; j < n - 1; ++j)
                    facets.Add(new Facet(new Point3D[] {
                        points[i * n + j], points[(i + 1) % density * n + j],
                        points[(i + 1) % density * n + j + 1], points[i * n + j + 1] }));
            return new Polyhedron(points, facets);
        }
    }
}

