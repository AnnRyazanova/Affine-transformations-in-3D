using System;
using System.Linq;

namespace AffineTransformationsIn3D.Primitives
{
    class Transformation
    {
        private float[,] matrix = new float[4,4];

        public Transformation() { }
        
        public Transformation(float[,] matrix)
        {
            this.matrix = matrix;
        }

        public float[,] Matrix { get { return matrix; } }

        public static Transformation Scale(float fx, float fy, float fz)
        {
            return new Transformation( new float[,] { { fx, 0, 0, 0 },
                                                      { 0, fy, 0, 0 },
                                                      { 0, 0, fz, 0 },
                                                      { 0, 0, 0, 1 }} );
        }

        public static Transformation Projection()
        {
            return new Transformation(new float[,] { { 1, 0, 0, 0 },
                                                      { 0, 1, 0, 0 },
                                                      { 0, 0, 1, 0 },
                                                      { 0, 0, 0, 1 }});
        }
        
        /*
        public static Transformation Translate(float dx, float dy)
        {
            return new Transformation( 
                 1, 0, 0,
                 0, 1, 0,
                dx, dy, 1
            );
            
            return 0;
        }
        */
        public static Transformation operator *(Transformation t1, Transformation t2)
        {
            Transformation result = new Transformation();
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    float value = 0;
                    for (int k = 0; k < 4; ++k)
                        value += t1.matrix[i, k] * t2.matrix[k, j];
                    result.matrix[i, j] = value;
                }
            return result;
        }
    }
}
