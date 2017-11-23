using System;

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
            base.Draw(graphics);
        }
        

    }
}
