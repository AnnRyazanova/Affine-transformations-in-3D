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

        private bool shouldBeDrawn(Vector vertex)
        {
            return ((vertex.X >= 0 && vertex.X < Width) &&
                   (vertex.Y >= 0 && vertex.Y < Height) &&
                   (vertex.Z < 1) && (vertex.Z > -1));
        }

        public void DrawPoint(Vector a, Color color)
        {
            var t = ClipToNormalized(a * Transformation);
            if (t.Z < -1 || t.Z > 1) return;
            var A = NormalizedToScreen(t);
            //if (IsHuge(A)) return;
            /*
            var rectangle = new RectangleF(
                (float)(A.X - POINT_SIZE / 2),
                (float)(A.Y - POINT_SIZE / 2),
                (float)POINT_SIZE,
                (float)POINT_SIZE);
                */
            //graphics.FillRectangle(brush, rectangle);

            if (shouldBeDrawn(A))
                ColorBuffer.SetPixel((int)Math.Ceiling(A.X), (int)Math.Ceiling(A.Y), color);
        }

        public void DrawLine(Vector a, Vector b, Pen pen)
        {
            var t = ClipToNormalized(a * Transformation);
            if (t.Z < -1 || t.Z > 1) return;
            var A = NormalizedToScreen(t);
            var u = ClipToNormalized(b * Transformation);
            if (u.Z < -1 || u.Z > 1) return;
            var B = NormalizedToScreen(u);
           // if (IsHuge(A) || IsHuge(B)) return;
           if (shouldBeDrawn(A))
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

        private void Swap<T>(ref T c1, ref T c2)
        {
            T t = c1;
            c1 = c2;
            c2 = t;
        }

        private double Interpolation(int y, double x0, double x1, double y0, double y1)
        {
            return x0 + (x1 - x0) * ((y - y0) / (y1 - y0 + 1));
        }


        private void Clip(ref double component)
        {
            if (component < 0)
                component = 0;
            else if (component > 255)
                component = 255;
        }

        private Color Interpolation(int y, Color color1, Color color2, double y0, double y1)
        {
            float R1 = color1.R;
            float G1 = color1.G;
            float B1 = color1.B;

            float R2 = color2.R;
            float G2 = color2.G;
            float B2 = color2.B;

            double R = Interpolation(y, R1, R2, y0, y1);
            double G = Interpolation(y, G1, G2, y0, y1);
            double B = Interpolation(y, B1, B2, y0, y1);

            Clip(ref R);
            Clip(ref G);
            Clip(ref B);

            return Color.FromArgb((int)Math.Round(R), (int)Math.Round(G), (int)Math.Round(B));
        }

        public void DrawTriangle(Vector a, Vector b, Vector c, Color color1, Color color2, Color color3)
        {
            if (spaceToScreenCoordinate(a, ref a) &&
                spaceToScreenCoordinate(b, ref b) &&
                spaceToScreenCoordinate(c, ref c))
            {

                if (a.Y > b.Y)
                    Swap(ref a, ref b);

                if (a.Y > c.Y)
                    Swap(ref a, ref c);

                if (b.Y > c.Y)
                    Swap(ref b, ref c);

                
                for (int y = (int)Math.Ceiling(a.Y); y <= (int)Math.Ceiling(b.Y); ++y)
                {
                    if (y < 0 || y > (Height - 1))
                        continue;

                    double leftX, leftZ, rightX, rightZ;
                    leftX = Interpolation(y, a.X, b.X, a.Y, b.Y);
                    leftZ = Interpolation(y, a.Z, b.Z, a.Y, b.Y);
                    

                    Color leftColor = Interpolation(y, color1, color2, a.Y, b.Y);

                    rightX = Interpolation(y, a.X, c.X, a.Y, c.Y);
                    rightZ = Interpolation(y, a.Z, c.Z, a.Y, c.Y);

                    Color rightColor = Interpolation(y, color1, color3, a.Y, c.Y);

                    Vector left = new Vector(leftX, y, leftZ);
                    Vector right = new Vector(rightX, y, rightZ);

                    if (leftX > rightX)
                    {
                        Swap(ref left, ref right);
                        Swap(ref leftColor, ref rightColor);
                    }

                    for (int x = (int)left.X; x <= right.X; ++x)
                    {
                        if (x < 0 || x > (Width - 1))
                            continue;

                        double z = Interpolation(x, left.Z, right.Z, left.X, right.X);
                        Color color = Interpolation(x, leftColor, rightColor, left.X, right.X);

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

                    double leftX, leftZ, rightX, rightZ;
                    leftX = Interpolation(y, a.X, c.X, a.Y, c.Y);
                    leftZ = Interpolation(y, a.Z, c.Z, a.Y, c.Y);

                    Color leftColor = Interpolation(y, color1, color3, a.Y, c.Y);

                    rightX = Interpolation(y, b.X, c.X, b.Y, c.Y);
                    rightZ = Interpolation(y, b.Z, c.Z, b.Y, c.Y);
                    
                    Color rightColor = Interpolation(y, color2, color3, b.Y, c.Y);

                    Vector left = new Vector(leftX, y, leftZ);
                    Vector right = new Vector(rightX, y, rightZ);

                    if (leftX > rightX)
                    {
                        Swap(ref left, ref right);
                        Swap(ref leftColor, ref rightColor);
                    }

                    for (int x = (int)left.X; x <= right.X; ++x)
                    {
                        if (x < 0 || x > (Width - 1))
                            continue;
                        
                        double z = Interpolation(x, left.Z, right.Z, left.X, right.X);
                        Color color = Interpolation(x, leftColor, rightColor, left.X, right.X);

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
