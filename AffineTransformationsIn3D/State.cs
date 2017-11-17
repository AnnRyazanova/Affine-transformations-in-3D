using System.Drawing;

namespace AffineTransformationsIn3D
{
    class State
    {
        public Color Color { get; set; }
        public double Z { get; set; }

        public State(Color color, double z)
        {
            Color = color;
            Z = z;
        }

        public State()
        {
            Color = SystemColors.Control;
            Z = double.MinValue;
        }
    }
}
