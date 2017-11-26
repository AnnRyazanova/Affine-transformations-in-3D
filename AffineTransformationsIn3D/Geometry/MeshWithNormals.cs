using System;

namespace AffineTransformationsIn3D.Geometry
{
    public class MeshWithNormals : Mesh
    {
        public Vector[] Normals { get; set; }

        public MeshWithNormals(Vector[] vertices, int[][] indices, Vector[] normals) : base(vertices, indices)
        {
            Normals = normals;
        }

        public override void Draw(Graphics3D graphics)
        {
            Random r = new Random(42);
            foreach (var facet in Indices)
            {
                var firstColor = NextColor(r);
                for (int i = 1; i < facet.Length - 1; ++i)
                {
                    var a = new Vertex(Vertices[facet[0]], Normals[facet[0]], firstColor);
                    var b = new Vertex(Vertices[facet[i]], Normals[facet[i]], NextColor(r));
                    var c = new Vertex(Vertices[facet[i + 1]], Normals[facet[i + 1]], NextColor(r));
                    graphics.DrawTriangle(a, b, c);
                }
            }
        }
    }
}
