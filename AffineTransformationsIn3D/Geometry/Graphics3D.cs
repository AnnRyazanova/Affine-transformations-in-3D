using System;
using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class Graphics3D
    {
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

        private Vector SpaceToNormalized(Vector v)
        {
            return Normailze(v * Transformation);
        }

        private Vector Normailze(Vector v)
        {
            return new Vector(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }

        private Vector NormalizedToScreen(Vector v)
        {
            return new Vector(
                (v.X + 1) / 2 * Width, 
                (-v.Y + 1) / 2 * Height,
                v.Z);
        }

        // перевод координат из пространственных в экранные
        private Vector SpaceToScreenCoordinate(Vector spaceVertex)
        {
            return NormalizedToScreen(SpaceToNormalized(spaceVertex));
        }

        private void SpaceToScreen(ref Vertex vertex)
        {
            vertex.Coordinate = SpaceToScreenCoordinate(vertex.Coordinate);
        }

        public void DrawLine(Vector a, Vector b)
        {
            DrawLine(a, b, Color.Black);
        }

        private bool ShouldBeDrawn(Vector vertex)
        {
            return ((vertex.X >= 0 && vertex.X < Width) &&
                   (vertex.Y >= 0 && vertex.Y < Height) &&
                   (vertex.Z < 1) && (vertex.Z > -1));
        }

        public void DrawPoint(Vector a, Color color)
        {
            var t = SpaceToNormalized(a);
            if (t.Z < -1 || t.Z > 1) return;
            var A = NormalizedToScreen(t);
            if (ShouldBeDrawn(A))
                ColorBuffer.SetPixel((int)Math.Ceiling(A.X), (int)Math.Ceiling(A.Y), color);
        }

        public void DrawLine(Vector a, Vector b, Color color)
        {
            a = SpaceToScreenCoordinate(a);
            b = SpaceToScreenCoordinate(b);
            var x0 = (int)a.X;
            var y0 = (int)a.Y;
            var x1 = (int)b.X;
            var y1 = (int)b.Y;
            /* https://github.com/fragkakis/bresenham/blob/master/src/main/java/org/fragkakis/Bresenham.java */
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            int currentX = x0;
            int currentY = y0;
            while (true)
            {
                if (0 <= currentX && currentX < Width && 0 <= currentY && currentY < Height)
                {
                    double z = Interpolate(a.Z, b.Z, dx < dy ? (currentY - a.Y) / dy : (currentX - a.X) / dx);
                    if (-1 < z && z < 1 && ZBuffer[currentX, currentY] < z)
                    {
                        ColorBuffer.SetPixel(currentX, currentY, color);
                        ZBuffer[currentX, currentY] = z;
                    }
                }
                if (currentX == x1 && currentY == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    currentX = currentX + sx;
                }
                if (e2 < dx)
                {
                    err = err + dx;
                    currentY = currentY + sy;
                }
            }
        }

        private double Interpolate(double x0, double x1, double f)
        {
            return x0 + (x1 - x0) * f;
        }

        private long Interpolate(long x0, long x1, double f)
        {
            return x0 + (long)((x1 - x0) * f);
        }

        private Color Interpolate(Color a, Color b, double f)
        {
            var R = Interpolate(a.R, b.R, f);
            var G = Interpolate(a.G, b.G, f);
            var B = Interpolate(a.B, b.B, f);
            return Color.FromArgb((byte)R, (byte)G, (byte)B);
        }

        private Vector Interpolate(Vector a, Vector b, double f)
        {
            return new Vector(
                Interpolate(a.X, b.X, f),
                Interpolate(a.Y, b.Y, f),
                Interpolate(a.Z, b.Z, f));
        }

        private void Interpolate(Vertex a, Vertex b, double f, ref Vertex v) {
            v.Coordinate = Interpolate(a.Coordinate, b.Coordinate, f);
            v.Normal = Interpolate(a.Normal, b.Normal, f);
            v.Color = Interpolate(a.Color, b.Color, f);
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        public void DrawTriangle(Vertex a, Vertex b, Vertex c)
        {
            SpaceToScreen(ref a);
            SpaceToScreen(ref b);
            SpaceToScreen(ref c);

            if (a.Coordinate.Y > b.Coordinate.Y)
                Swap(ref a, ref b);
            if (a.Coordinate.Y > c.Coordinate.Y)
                Swap(ref a, ref c);
            if (b.Coordinate.Y > c.Coordinate.Y)
                Swap(ref b, ref c);

            Vertex left = new Vertex();
            Vertex right = new Vertex();
            Vertex point = new Vertex();

            for (double y = a.Coordinate.Y; y < c.Coordinate.Y; ++y)
            {
                // Тут не должно быть этой проверки.
                // Если какая то вершина находится за границей экрана, то она должна быть отсечена в Clip пространстве.
                if (y < 0 || y > (Height - 1))
                    continue;

                bool topHalf = y < b.Coordinate.Y;

                var a0 = a;
                var a1 = c;
                Interpolate(a0, a1, (y - a0.Coordinate.Y) / (a1.Coordinate.Y - a0.Coordinate.Y), ref left);

                var b0 = topHalf ? a : b;
                var b1 = topHalf ? b : c;
                Interpolate(b0, b1, (y - b0.Coordinate.Y) / (b1.Coordinate.Y - b0.Coordinate.Y), ref right);

                if (left.Coordinate.X > right.Coordinate.X) Swap(ref left, ref right);

                for (double x = left.Coordinate.X; x < right.Coordinate.X; ++x)
                {
                    // Тут не должно быть этой проверки.
                    // Если какая то вершина находится за границей экрана, то она должна быть отсечена в Clip пространстве.
                    if (x < 0 || x > (Width - 1))
                        continue;

                    Interpolate(left, right, (x - left.Coordinate.X) / (right.Coordinate.X - left.Coordinate.X), ref point);

                    if (point.Coordinate.Z > 1 || point.Coordinate.Z < -1)
                        continue;

                    if (point.Coordinate.Z > ZBuffer[(int)x, (int)y])
                    {
                        ZBuffer[(int)x, (int)y] = point.Coordinate.Z;
                          
                        ColorBuffer.SetPixel((int)x, (int)y, point.Color);
                    }
                }
            }
        }
    }
}
