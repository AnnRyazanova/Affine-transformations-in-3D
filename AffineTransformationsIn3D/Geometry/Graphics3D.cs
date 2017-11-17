using System;
using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class Graphics3D
    {
        private static double POINT_SIZE = 5;

        private Graphics graphics;


        public Bitmap ColorBuffer { get; set; }
        private double[,] ZBuffer { get; set; }

        public Matrix Transformation { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }


        public Graphics3D(Graphics graphics, Matrix transformation, double width, double height)
        {
            this.graphics = graphics;
            Transformation = transformation;
            Width = width;
            Height = height;

            ColorBuffer = new Bitmap((int)Math.Ceiling(Width), (int)Math.Ceiling(Height));
            ZBuffer = new double[(int)Math.Ceiling(Width), (int)Math.Ceiling(Height)];


            for (int j = 0; j < (int)Math.Ceiling(Height); ++j)
                for (int i = 0; i < (int)Math.Ceiling(Width); ++i)
                    ZBuffer[i, j] = double.MinValue;

        }

        public Vector ClipToNormalized(Vector v)
        {
            return new Vector(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }

        public Vector NormalizedToScreen(Vector v)
        {
            return new Vector(
                (v.X + 1) / 2 * Width, 
                (-v.Y + 1) / 2 * Height,
                v.Z);
        }

        public void DrawLine(Vector a, Vector b)
        {
            DrawLine(a, b, Pens.Black);
        }

        public static bool IsHuge(double f)
        {
            return 10e6 < Math.Abs(f);
        }

        public static bool IsHuge(Vector v)
        {
            return IsHuge(v.X) || IsHuge(v.Y);
        }

        public void DrawPoint(Vector a, Brush brush)
        {
            var t = ClipToNormalized(a * Transformation);
            if (t.Z < -1 || t.Z > 1) return;
            var A = NormalizedToScreen(t);
            if (IsHuge(A)) return;
            var rectangle = new RectangleF(
                (float)(A.X - POINT_SIZE / 2), 
                (float)(A.Y - POINT_SIZE / 2), 
                (float)POINT_SIZE, 
                (float)POINT_SIZE);
            graphics.FillRectangle(brush, rectangle);
        }

        public void DrawLine(Vector a, Vector b, Pen pen)
        {
            var t = ClipToNormalized(a * Transformation);
            if (t.Z < -1 || t.Z > 1) return;
            var A = NormalizedToScreen(t);
            var u = ClipToNormalized(b * Transformation);
            if (u.Z < -1 || u.Z > 1) return;
            var B = NormalizedToScreen(u);
            if (IsHuge(A) || IsHuge(B)) return;
            graphics.DrawLine(pen, (float)A.X, (float)A.Y, (float)B.X, (float)B.Y);
        }

        // перевод координат из пространственных в экранные
        private bool spaceToScreenCoordinate(Vector spaceVertex, ref Vector screenVertex)
        {
            var t = ClipToNormalized(spaceVertex * Transformation);
            if (t.Z < -1 || t.Z > 1) return false;
            screenVertex = NormalizedToScreen(t);
            return true;
        }

        private void swap(ref Vector p1, ref Vector p2)
        {
            Vector t = p1;
            p1 = p2;
            p2 = t;
        }

        public void DrawTriangle(Vector a, Vector b, Vector c, Color color)
        {
            if (spaceToScreenCoordinate(a, ref a) &&
                spaceToScreenCoordinate(b, ref b) &&
                spaceToScreenCoordinate(c, ref c))
            {

                if (a.Y > b.Y)
                    swap(ref a, ref b);

                if (a.Y > c.Y)
                    swap(ref a, ref c);

                if (b.Y > c.Y)
                    swap(ref b, ref c);

                
                // issue : нужно ли округлять? 
                for (int y = (int)Math.Ceiling(a.Y); y <= (int)Math.Ceiling(b.Y); ++y)
                {
                    if (y < 0 || y > (Height - 1))
                        continue;

                    double leftX = a.X + (b.X - a.X) * ((y - a.Y) / (b.Y - a.Y + 1));
                    double rightX = a.X + (c.X - a.X) * ((y - a.Y) / (c.Y - a.Y + 1));
                    double leftZ = a.Z + (b.Z - a.Z) * ((y - a.Y) / (b.Y - a.Y + 1));
                    double rightZ = a.Z + (c.Z - a.Z) * ((y - a.Y) / (c.Y - a.Y + 1));

                    Vector left = new Vector(leftX, y, leftZ);
                    Vector right = new Vector(rightX, y, rightZ);

                    if (leftX > rightX)
                        swap(ref left, ref right);

                    for (int x = (int)left.X; x <= right.X; ++x)
                    {
                        if (x < 0 || x > (Width - 1))
                            continue;

                        double z = left.Z + (right.Z - left.Z) * ((x - left.X) / (right.X - left.X + 1));

                        if (z > 1 || z < -1)
                            continue;

                        if (z > ZBuffer[x, y])
                        {
                            ZBuffer[x, y] = z;
                          
                            ColorBuffer.SetPixel(x, y, color);
                        }
                    }

                }


                for (int y = (int)Math.Ceiling(b.Y); y <= (int)Math.Ceiling(c.Y); ++y)
                {
                    if (y < 0 || y > (Height - 1))
                        continue;

                    double leftX = a.X + (c.X - a.X) * ((y - a.Y) / (c.Y - a.Y + 1));
                    double rightX = b.X + (c.X - b.X) * ((y - b.Y) / (c.Y - b.Y + 1));
                    double leftZ = a.Z + (c.Z - a.Z) * ((y - a.Y) / (c.Y - a.Y + 1));
                    double rightZ = b.Z + (c.Z - b.Z) * ((y - b.Y) / (c.Y - b.Y + 1));

                    Vector left = new Vector(leftX, y, leftZ);
                    Vector right = new Vector(rightX, y, rightZ);

                    if (leftX > rightX)
                        swap(ref left, ref right);

                    for (int x = (int)left.X; x <= right.X; ++x)
                    {
                        if (x < 0 || x > (Width - 1))
                            continue;

                        double z = left.Z + (right.Z - left.Z) * ((x - left.X) / (right.X - left.X + 1));

                        if (z > 1 || z < -1)
                            continue;

                        if (z > ZBuffer[x, y])
                        {
                            ZBuffer[x, y] = z;
                            ColorBuffer.SetPixel(x, y, color);
                        }
                    }

                }

            }

        }

    }
}
