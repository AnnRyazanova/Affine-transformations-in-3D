using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    class Point3D : Primitive
    {
        private static float POINT_SIZE = 6;

        private float[] coords = new float[] { 0, 0, 0, 1 };

        public float X { get { return coords[0]; } set { coords[0] = value; } }
        public float Y { get { return coords[1]; } set { coords[1] = value; } }
        public float Z { get { return coords[2]; } set { coords[2] = value; } }

        public Point3D() { }

        public Point3D(Point3D p)
        {
            X = p.X;
            Y = p.Y;
            Z = p.Z;
        }

        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        private Point3D(float[] arr)
        {
            coords = arr;
        }

        public static Point3D FromPoint(Point point)
        {
            return new Point3D(point.X, point.Y, 0);
        }

        public void Draw(Graphics g, Transformation proection)
        {
            var p = this.Apply(proection);
           // if (p.Z >= -1 || p.Z <= 1)
                g.FillRectangle(Brushes.Black, p.X - POINT_SIZE / 2, p.Y - POINT_SIZE / 2, POINT_SIZE, POINT_SIZE);
        }

        public Point3D Apply(Transformation t)
        {
            float[] newCoords = new float[4];
            for (int i = 0; i < 4; ++i)
            {
                newCoords[i] = 0;
                for (int j = 0; j < 4; ++j)
                    newCoords[i] += coords[j] * t.Matrix[j, i];
            }
            coords = newCoords;
            return new Point3D(newCoords);
        }
    }
}
