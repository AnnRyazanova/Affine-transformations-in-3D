using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class MeshWithTexture : Mesh
    {
        Bitmap texture;
        PointF[] textureCoordinates;

        public MeshWithTexture(Vector[] vertices, int[][] indices, PointF[][] textureCoordinates) : base(vertices, indices)
        {
            this.textureCoordinates = textureCoordinates;
        }
    }
}
