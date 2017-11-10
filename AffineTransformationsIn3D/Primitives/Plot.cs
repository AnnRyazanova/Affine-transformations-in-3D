using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AffineTransformationsIn3D.Primitives
{
	public class Plot : IPrimitive
	{
		private IList<Point3D> points = new List<Point3D>();
		private IList<Line> lines = new List<Line>();

		public IList<Point3D> Points { get { return points; } }
		public IList<Line> Lines { get { return lines; } }

		private double F(double x, double y)
		{
			return (x * x * y) / (x * x * x * x + y * y);
		}

		public Point3D Center
		{
			get
			{
				var x = (Lines[0].A.X + Lines[0].B.X) / 2;
				var y = (Lines[0].A.Y + Lines[0].B.Y) / 2;
				var z = (Lines[0].A.Z + Lines[0].B.Z) / 2;
				return new Point3D(x, y, z);
			}
		}

		public Plot()
		{
			points.Add(new Point3D(0, 0, F(0, 0)));

			for (double i = 0.1; i < 1; i += 0.03)
				for (double j = 0.1; j < 1; j += 0.03)
				{
					points.Add(new Point3D(i, j, F(i, j)));
					lines.Add(new Line(points[points.Count - 2], points[points.Count - 1]));
				}
		}

		public void Apply(Transformation t)
		{
			foreach (var point in Lines)
				point.Apply(t);
		}

		public void Draw(Graphics g, Transformation projection, int width, int height)
		{
			for (int i = 0; i < lines.Count; i++)
			{
				var c = lines[i].A.Transform(projection).NormalizedToDisplay(width, height);
				var d = lines[i].B.Transform(projection).NormalizedToDisplay(width, height);
				g.DrawLine(Pens.Black, (float)c.X, (float)c.Y, (float)d.X, (float)d.Y);
			}
		}
	}
};

