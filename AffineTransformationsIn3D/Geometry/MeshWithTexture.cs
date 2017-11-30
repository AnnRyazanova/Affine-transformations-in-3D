using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class MeshWithTexture : Mesh
    {
        private Bitmap texture;
        private PointF[] textureCoordinates;

        public MeshWithTexture(Vector[] vertices, int[][] indices, PointF[] textureCoordinates) : base(vertices, indices)
        {
            this.textureCoordinates = textureCoordinates;
        }
    }
}
