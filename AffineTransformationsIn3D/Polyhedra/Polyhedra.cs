using System;
using System.Collections.Generic;
using System.Drawing;
using AffineTransformationsIn3D.Primitives;

namespace AffineTransformationsIn3D.Polyhedra
{
    interface Polyhedra : IPrimitive
    {
		Point3D Center
		{
			get;
		}
	}
}
