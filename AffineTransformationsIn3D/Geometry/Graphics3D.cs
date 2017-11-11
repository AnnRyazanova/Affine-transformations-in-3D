using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class Graphics3D
    {
        private static double POINT_SIZE = 5;

        private Graphics graphics;

        public Matrix Transformation { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Graphics3D(Graphics graphics, Matrix projection, double width, double height)
        {
            this.graphics = graphics;
            Transformation = projection;
            Width = width;
            Height = height;
        }

        private PointF NormalizedToScreen(Vertex v)
        {
            return new PointF(
                (float)((v.X + 1) / 2 * Width), 
                (float)((-v.Y + 1) / 2 * Height));
        }

        public void DrawLine(Vertex a, Vertex b)
        {
            DrawLine(a, b, Pens.Black);
        }

        public void DrawPoint(Vertex a, Brush brush)
        {
            var A = NormalizedToScreen(a * Transformation);
            var rectangle = new RectangleF(
                (float)(A.X - POINT_SIZE / 2), 
                (float)(A.Y - POINT_SIZE / 2), 
                (float)POINT_SIZE, 
                (float)POINT_SIZE);
            graphics.FillRectangle(brush, rectangle);
        }

        public void DrawLine(Vertex a, Vertex b, Pen pen)
        {
            var A = NormalizedToScreen(a * Transformation);
            var B = NormalizedToScreen(b * Transformation);
            graphics.DrawLine(pen, A, B);
        }
    }
}
