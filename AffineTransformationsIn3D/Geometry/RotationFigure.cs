using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class RotationFigure : Mesh
    {
        public RotationFigure(IList<Vector> initial, int axis, int density)
            : base(Construct(initial, axis, density))
        {
        }

        private static List<Vector> normal { get; set; }

        private static Tuple<Vector[], int[][]> Construct(IList<Vector> initial, int axis, int density)
        {
            Debug.Assert(0 <= axis && axis < 3);
            var n = initial.Count;
            var vertices = new Vector[n * density];
            var indices = new int[density * (n - 1)][];
            Func<double, Matrix> rotation;
            switch (axis)
            {
                case 0: rotation = Transformations.RotateX; break;
                case 1: rotation = Transformations.RotateY; break;
                default: rotation = Transformations.RotateZ; break;
            }

            for (int i = 0; i < density; ++i)
                for (int j = 0; j < n; ++j)
                    vertices[i * n + j] = initial[j] * rotation(2 * Math.PI * i / density);


            for (int i = 0; i < density; ++i)
                for (int j = 0; j < n - 1; ++j)
                    indices[i * (n - 1) + j] = new int[4] {
                        i * n + j,
                        (i + 1) % density * n + j,
                        (i + 1) % density * n + j + 1,
                        i * n + j + 1 };
            return new Tuple<Vector[], int[][]>(vertices, indices);
        }

        public override void Draw(Graphics3D graphics)
        {
            Vector vec = new Vector(-1, 1, -1);
            foreach (var facet in Indices)
            {
                Vector normal = Vertices[facet[0]] - Vertices[facet[1]];
                normal = Vector.CrossProduct(normal, Vertices[facet[1]] - Vertices[facet[2]]);

                if (Vector.Dist(normal, Vertices[facet[0]]) < Vector.Dist(-normal, Vertices[facet[0]]))
                    normal = -normal;

                if (Vector.AngleBet(vec, normal)<(Math.PI/2))
                {
                    for (int i = 0; i < facet.Length - 1; ++i)
                    {
                        var a = new Vertex(Vertices[facet[i]], new Vector(), Color.Black);
                        var b = new Vertex(Vertices[facet[i + 1]], new Vector(), Color.Black);
                        graphics.DrawLine(a, b);
                    }
                    var first = new Vertex(Vertices[facet[0]], new Vector(), Color.Black);
                    var last = new Vertex(Vertices[facet[facet.Length - 1]], new Vector(), Color.Black);
                    graphics.DrawLine(first, last);
                }
            }
        }
    }
}
