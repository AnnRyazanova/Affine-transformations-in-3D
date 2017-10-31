using System;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    interface IPrimitive: ICloneable
    {
        void Draw(Graphics g, Transformation projection, int width, int height);
        void Apply(Transformation t);
    }
}
