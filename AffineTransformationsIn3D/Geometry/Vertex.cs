using System.Drawing;

namespace AffineTransformationsIn3D.Geometry
{
    public class Vertex
    {
        public Vector Coordinate { get; set; }
        public Color Color { get; set; }
        public Vector Normal { get; set; }
        public Vector TextureCoordinate { get; set; }

        public Vertex(Vector coordinate) : this(coordinate, Color.White) { }

        public Vertex(Vector coordinate, Color color) : this(coordinate, color, new Vector(0, 0, 0)) { }

        public Vertex(Vector coordinate, Color color, Vector normal) : this(coordinate, color, normal, new Vector(0, 0, 0)) { }

        public Vertex(Vector coordinate, Color color, Vector normal, Vector textureCoordinate)
        {
            Coordinate = coordinate;
            Color = color;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }
    }
}
