using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    interface Primitive
    {
        void Draw(Graphics g, Transformation proection);
        Point3D Apply(Transformation t);
    }
}
