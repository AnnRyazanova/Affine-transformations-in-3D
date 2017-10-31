using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    class Line : IPrimitive
    {
        private Point3D a;
        private Point3D b;

        public Point3D A { get { return a; } set { a = value; } }
        public Point3D B { get { return b; } set { b = value; } }

        public Line(Point3D a, Point3D b)
        {
            A = a;
            B = b;
        }

        public void Apply(Transformation t)
        {
            A.Apply(t);
            B.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            var projectedA = (Point3D)a.Clone();
            projectedA.Apply(projection);
            var projectedB = (Point3D)b.Clone();
            projectedB.Apply(projection);
            var x0 = (projectedA.X + 1) / 2 * width;
            var y0 = (-projectedA.Y + 1) / 2 * height;
            var x1 = (projectedB.X + 1) / 2 * width;
            var y1 = (-projectedB.Y + 1) / 2 * height;
            g.DrawLine(Pens.Black, x0, y0, x1, y1);
        }

        public object Clone()
        {
            return new Line(A, B);
        }
    }
}
