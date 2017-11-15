using AffineTransformationsIn3D.Geometry;

namespace AffineTransformationsIn3D
{
    class Camera
    {
        public Vertex Position { get; set; }
        public double AngleY { get; set; }
        public double AngleX { get; set; }
        public Matrix Projection { get; set; }

        public Matrix Transformation
        {
            get
            {
                return Transformations.Translate(-Position.X, -Position.Y, -Position.Z)
                    * Transformations.RotateY(AngleY)
                    * Transformations.RotateX(AngleX)
                    * Projection;
            }
        }

        public Camera(Vertex position, double angleY, double angleX, Matrix projection)
        {
            Position = position;
            AngleY = angleY;
            AngleX = angleX;
            Projection = projection;
        }
    }
}
