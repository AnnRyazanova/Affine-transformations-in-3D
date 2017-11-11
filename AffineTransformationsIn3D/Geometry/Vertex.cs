using System;

namespace AffineTransformationsIn3D.Geometry
{
    public struct Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Vertex(double x, double y, double z, double w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vertex operator *(Vertex v, Matrix m)
        {
            var result = v;
            for (int i = 0; i < 4; ++i)
            {
                result[i] = 0;
                for (int j = 0; j < 4; ++j)
                    result[i] += v[j] * m[j, i];
            }
            return result;
        }

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new IndexOutOfRangeException("Vertex has only 4 coordinates");
                }
            }
            set
            {
                switch (i)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new IndexOutOfRangeException("Vertex has only 4 coordinates");
                }
            }
        }
    }
}
