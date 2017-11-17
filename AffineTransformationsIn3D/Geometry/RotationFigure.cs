using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
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
              List<Vector> normal = new List<Vector>();
              Vector vec = new Vector(0, 1, 1, 1);
              for (int i = 0; i < Indices.Length; ++i)
              {
                normal.Add(Vertices[Indices[i][1]]);
                normal[i] = Vector.CrossProduct(normal[i], Vertices[Indices[i][2]]);
  
               {
                     if (Vector.AngleBet(vec, normal[i])<(Math.PI/2))
                     {
                         for (int j = 0; j < Indices[i].Length - 1; ++j)
                             graphics.DrawLine(Vertices[Indices[i][j]], Vertices[Indices[i][j + 1]]);

                         graphics.DrawLine(Vertices[Indices[i][0]], Vertices[Indices[i][Indices[i].Length - 1]]);
                     }
                 }
             }
        }
    }
}
