using AffineTransformationsIn3D.Geometry;
using System.Drawing;
using System.Windows.Forms;

namespace AffineTransformationsIn3D
{
    class SceneView : Control
    {
        private Matrix projection = Transformations.OrthogonalProjection();

        public Mesh Mesh { get; set; }

        public Matrix Projection
        {
            get { return projection; }
            set
            {
                projection = value;
                Invalidate();
            }
        }

        public SceneView() : base()
        {
            var flags = ControlStyles.AllPaintingInWmPaint
                      | ControlStyles.DoubleBuffer
                      | ControlStyles.UserPaint;
            SetStyle(flags, true);
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.Control);
            e.Graphics.DrawLines(Pens.Black, new Point[]
                {
                    new Point(1, 1),
                    new Point(1, Height - 1),
                    new Point(Width - 1, Height - 1),
                    new Point(Width - 1, 1),
                    new Point(1, 1)
                });
            if (null == Mesh) return;
            var graphics3D = new Graphics3D(e.Graphics, Projection, Width, Height);
            var x = new Vertex(0.8, 0, 0);
            var y = new Vertex(0, 0.8, 0);
            var z = new Vertex(0, 0, 0.8);
            graphics3D.DrawLine(new Vertex(0, 0, 0), x, new Pen(Color.Red, 2));
            graphics3D.DrawPoint(x, Brushes.Red);
            graphics3D.DrawLine(new Vertex(0, 0, 0), y, new Pen(Color.Green, 2));
            graphics3D.DrawPoint(y, Brushes.Green);
            graphics3D.DrawLine(new Vertex(0, 0, 0), z, new Pen(Color.Blue, 2));
            graphics3D.DrawPoint(z, Brushes.Blue);
            Mesh.Draw(graphics3D);
        }
    }
}
