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
            A = A.Transform(t);
            B = B.Transform(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            var c = A.Transform(projection).NormalizedToDisplay(width, height);
            var d = B.Transform(projection).NormalizedToDisplay(width, height);
            g.DrawLine(Pens.Black, c.X, c.Y, d.X, d.Y);
        }
    }
}
