using System.Collections.Generic;
using System.Drawing;

namespace AffineTransformationsIn3D.Primitives
{
    class Polyhedron : IPrimitive
    {
        private IList<Polygon> faces;

        public IList<Polygon> Faces
        {
            get { return faces; }
            set { faces = value; }
        }

        public Polyhedron(IList<Polygon> faces)
        {
            this.faces = faces;
        }

        public void Apply(Transformation t)
        {
            foreach (var f in faces)
                f.Apply(t);
        }

        public void Draw(Graphics g, Transformation projection, int width, int height)
        {
            foreach (var f in faces)
                f.Draw(g, projection, width, height);
        }
    }
}
