
using System;

namespace AffineTransformationsIn3D.Primitives
{
    class Transformation
    {
        private float[,] matrix = new float[4,4];

        public float[,] Matrix { get { return matrix; } }

        public Transformation() { }
        
        public Transformation(float[,] matrix)
        {
            this.matrix = matrix;
        }

        public static Transformation RotateX(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Transformation(
                new float[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, cos, -sin, 0 },
                    { 0, sin, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformation RotateY(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Transformation(
                new float[,]
                {
                    { cos, 0, sin, 0 },
                    { 0, 1, 0, 0 },
                    { -sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformation RotateZ(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Transformation(
                new float[,]
                {
                    { cos, -sin, 0, 0 },
                    { sin, cos, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformation Scale(float fx, float fy, float fz)
        {
            return new Transformation( 
                new float[,] { 
                    { fx, 0, 0, 0 },
                    { 0, fy, 0, 0 },
                    { 0, 0, fz, 0 },
                    { 0, 0, 0, 1 }
                });
        }
        
        public static Transformation Translate(float dx, float dy, float dz)
        {
            return new Transformation(
                new float[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { dx, dy, dz, 1 },
                });
        }

        public static Transformation Identity()
        {
            return new Transformation(
                new float[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformation OrthogonalProjection()
        {
            return Identity();
        }

        public static Transformation operator *(Transformation t1, Transformation t2)
        {
            Transformation result = new Transformation();
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    result.matrix[i, j] = 0;
                    for (int k = 0; k < 4; ++k)
                        result.matrix[i, j] += t1.matrix[i, k] * t2.matrix[k, j];
                }
            return result;
        }
    }
}
