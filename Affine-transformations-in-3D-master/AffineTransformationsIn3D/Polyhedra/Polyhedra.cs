using System;
using System.Collections.Generic;
using System.Drawing;
using AffineTransformationsIn3D.Primitives;

namespace AffineTransformationsIn3D.Polyhedra
{
    interface Polyhedra
    {
        void Draw(Graphics g, Transformation projection, int width, int height);
        void Apply(Transformation t);
    }
}
