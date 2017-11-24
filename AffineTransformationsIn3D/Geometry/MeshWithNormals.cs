using System;
using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class MeshWithNormals : Mesh
    {
        public Vector[][] Normals { get; set; }

        public MeshWithNormals(Vector[] vertices, int[][] indices, Vector[][] normals) : base(vertices, indices)
        {
            Normals = normals;
        }

        public override void Draw(Graphics3D graphics)
        {
            Random r = new Random(42);
            foreach (var facet in Indices)
            {
                int k = r.Next(0, 256);
                int k2 = r.Next(0, 256);
                int k3 = r.Next(0, 256);

                for (int i = 1; i < facet.Length - 1; ++i)
                {
                    var a = new Vertex(Vertices[facet[0]], new Vector(), Color.FromArgb(k, k2, k3));
                    var b = new Vertex(Vertices[facet[i]], new Vector(), Color.FromArgb(k2, k, k3));
                    var c = new Vertex(Vertices[facet[i + 1]], new Vector(), Color.FromArgb(k3, k2, k));
                    graphics.DrawTriangle(a, b, c);
                }
            }
        }
        

    }
}
