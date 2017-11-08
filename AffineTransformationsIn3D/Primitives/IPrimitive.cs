using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    interface IPrimitive
    {
        Point3D Center { get; }

        void Draw(Graphics g, Transformation projection, int width, int height);
        void Apply(Transformation t);
    }
}
