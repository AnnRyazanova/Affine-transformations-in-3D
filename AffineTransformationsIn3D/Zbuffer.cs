using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

using AffineTransformationsIn3D.Geometry;

namespace AffineTransformationsIn3D
{
    class Zbuffer
    {
        private int width;
        private int height;
        private Graphics3D graphics;

        public double [,] DepthBuffer { get; set; }



        public Zbuffer(int w, int h, Graphics3D g)
        {
            width = w;
            height = h;
            graphics = g;

            DepthBuffer = new double[width, height];
            

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {
                    DepthBuffer[i, j] = double.MinValue;
                    // ColorBuffer установить цвет SystemColors.Control 
                }
        }


        // (y1 - y2) * x + (x2 - x1) * y + (x1 * y2 - x2 * y1) = 0
        // где (x, y) - координаты текущей точки
        // (x1, y1) и (x2, y2) - координаты данной edge
        private double findX(Vector a, Vector b, double curY)
        {
            return -1 * ((b.X - a.X) * curY + (a.X * b.Y - b.X * a.Y)) / (a.Y - b.Y);
        }

        private void rasterizeFace(List<Vector> vertices)
        {
            var minY = vertices.Min(vertex => vertex.Y);
            var maxY = vertices.Max(vertex => vertex.Y);
            
            vertices.Sort(delegate (Vector a, Vector b) {
                if (a.Y.Equals(b.Y))
                    return a.X.CompareTo(b.X);
                else
                    return a.Y.CompareTo(b.Y);
            });
            
            // растеризация
        }

        // перевод координат из пространственных в экранные
        private void spaceToScreenCoordinate(Vector[] spaceVertices, out Vector[] screenVertices)
        { 
            screenVertices = new Vector[spaceVertices.Length];
            int index = 0;

            foreach (var vertex in spaceVertices)
            {
                var t = graphics.ClipToNormalized(vertex * graphics.Transformation);
                if (t.Z < -1 || t.Z > 1) return;
                screenVertices[index++] = (graphics.NormalizedToScreen(t));
            }
        }

        // buildingZbuff(вершины, индексы вершин по граням, Graphics3D)
        public void buildingZbuff(Vector[] vertices, int[][] indices)
        {
            Vector[] screenVertices;
            spaceToScreenCoordinate(vertices, out screenVertices);

            // цикл по всем граням
            for (int i = 0; i < indices.Length; ++i)
            {
                // Индексы вершин текущей грани
                var facet = indices[i];
                // Вершины текущей грани
                List<Vector> vertexInFacet = new List<Vector>();

                foreach (var index in facet)
                    vertexInFacet.Add(screenVertices[index]);

                rasterizeFace(vertexInFacet);

                // растеризация
                // в зависимости от Z размещаем(или не размещаем) в Z-буффере
            }
        }




    }
}
