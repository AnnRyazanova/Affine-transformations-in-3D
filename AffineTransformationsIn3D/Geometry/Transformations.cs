using System;

namespace AffineTransformationsIn3D.Geometry
{
    public static class Transformations
    {
        public static Matrix RotateX(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new Matrix(
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, cos, -sin, 0 },
                    { 0, sin, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix RotateY(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new Matrix(
                new double[,]
                {
                    { cos, 0, sin, 0 },
                    { 0, 1, 0, 0 },
                    { -sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix RotateZ(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new Matrix(
                new double[,]
                {
                    { cos, -sin, 0, 0 },
                    { sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix Scale(double fx, double fy, double fz)
        {
            return new Matrix(
                new double[,] {
                    { fx, 0, 0, 0 },
                    { 0, fy, 0, 0 },
                    { 0, 0, fz, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix Translate(double dx, double dy, double dz)
        {
            return new Matrix(
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { dx, dy, dz, 1 },
                });
        }

        public static Matrix Identity()
        {
            return new Matrix(
                new double[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix ReflectX()
        {
            return Scale(-1, 1, 1);
        }

        public static Matrix ReflectY()
        {
            return Scale(1, -1, 1);
        }

        public static Matrix ReflectZ()
        {
            return Scale(1, 1, -1);
        }

        public static Matrix OrthogonalProjection()
        {
            return Identity();
        }

        public static Matrix RotateAroundPoint(Vertex point, 
            double angleX, double angleY, double angleZ)
        {
            return Translate(-point.X, -point.Y, -point.Z)
                * RotateX(angleX)
                * RotateY(angleY)
                * RotateZ(angleZ)
                * Translate(point.X, point.Y, point.Z);
        }

        public static Matrix RotateAroundLine(Vertex a, Vertex b, double angle)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            var dz = b.Z - a.Z;
            var angleY = 0 == dx ? 0 : -Math.Atan(dz / dx);
            var angleZ = 0 == dx ? Math.PI / 2 : Math.Atan(dy / dx);
            return Translate(-a.X, -a.Y, -a.Z)
                * RotateZ(angleZ)
                * RotateY(angleY)
                * RotateX(angle)
                * RotateY(-angleY)
                * RotateZ(-angleZ)
                * Translate(a.X, a.Y, a.Z);
        }
    }
}
