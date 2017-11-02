using System;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    class Point3D : IPrimitive
    {
        private static float POINT_SIZE = 6.0f;

        private float[] coords = new float[] { 0, 0, 0, 1 };

        public float X { get { return coords[0]; } set { coords[0] = value; } }
        public float Y { get { return coords[1]; } set { coords[1] = value; } }
        public float Z { get { return coords[2]; } set { coords[2] = value; } }

        public Point3D() { }

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

        public void Apply(Transformation t)
        {
            float[] newCoords = new float[4];
            for (int i = 0; i < 4; ++i)
            {
                newCoords[i] = 0;
                for (int j = 0; j < 4; ++j)
                    newCoords[i] += coords[j] * t.Matrix[j, i];
            }
            coords = newCoords;
        }

        public Point3D Transform(Transformation t)
        {
            var p = new Point3D(X, Y, Z);
            p.Apply(t);
            return p;
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            var projected = Transform(projection);
            if (Z < -1 || 1 < Z) return;
            var x = (projected.X + 1) / 2 * width;
            var y = (-projected.Y + 1) / 2 * height;
            g.FillRectangle(Brushes.Black,
                x - POINT_SIZE / 2, y - POINT_SIZE / 2,
                POINT_SIZE, POINT_SIZE);
        }

        /*
         * Преобразует координаты из ([-1, 1], [-1, 1], [-1, 1]) в ([0, width), [0, height), [-1, 1]).
         */
        public Point3D NormalizedToDisplay(int width, int height)
        {
            var x = (X + 1) / 2 * width;
            var y = (-Y + 1) / 2 * height;
            return new Point3D(x, y, Z);
        }
    }
}
