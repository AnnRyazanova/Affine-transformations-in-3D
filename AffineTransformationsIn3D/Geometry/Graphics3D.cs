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

        private Vector? SpaceToNormalized(Vector v)
        {
            var t = Normailze(v * Transformation);
			if (t.Z < -1 || t.Z > 1) return null;
			return t;
		}

        private Vector Normailze(Vector v)
        {
            return new Vector(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }

        public Vector? NormalizedToScreen(Vector? v)
        {
			if (!v.HasValue) return null;
            return new Vector(
                (v.Value.X + 1) / 2 * Width, 
                (-v.Value.Y + 1) / 2 * Height,
                v.Value.Z);
        }

        // перевод координат из пространственных в экранные
        private Vector? SpaceToScreenCoordinate(Vector spaceVertex)
        {
            return NormalizedToScreen(SpaceToNormalized(spaceVertex));
        }

        private bool SpaceToScreen(ref Vertex vertex)
        {
            var t = SpaceToScreenCoordinate(vertex.Coordinate);
			if (!t.HasValue) return false;
			vertex.Coordinate = t.Value;
			return true;
        }

        public void DrawLine(Vector a, Vector b)
        {
            DrawLine(a, b, Pens.Black);
        }

        private bool ShouldBeDrawn(Vector vertex)
        {
            return ((vertex.X >= 0 && vertex.X < Width) &&
                   (vertex.Y >= 0 && vertex.Y < Height) &&
                   (vertex.Z < 1) && (vertex.Z > -1));
        }

        public void DrawPoint(Vector a, Color color)
        {
            var A = NormalizedToScreen(SpaceToNormalized(a));
            if (A.HasValue && ShouldBeDrawn(A.Value))
                ColorBuffer.SetPixel((int)Math.Ceiling(A.Value.X), (int)Math.Ceiling(A.Value.Y), color);
        }

        public void DrawLine(Vector a, Vector b, Pen pen)
        {
            var A = NormalizedToScreen(SpaceToNormalized(a));
            var B = NormalizedToScreen(SpaceToNormalized(b));
			if (!A.HasValue || !B.HasValue) return;
			if (ShouldBeDrawn(A.Value))
                graphics.DrawLine(pen, (float)A.Value.X, (float)A.Value.Y, (float)B.Value.X, (float)B.Value.Y);
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
			if (!SpaceToScreen(ref a) || !SpaceToScreen(ref b) || !SpaceToScreen(ref c))
				return;

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
