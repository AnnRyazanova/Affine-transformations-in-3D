using AffineTransformationsIn3D.Geometry;
using System.Drawing;
using System.Windows.Forms;

namespace AffineTransformationsIn3D
{
    class SceneView : Control
    {
        public Camera ViewCamera { get; set; }
        public Mesh Mesh { get; set; }

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
            var graphics3D = new Graphics3D(ViewCamera, Width, Height);
            var zero = new Vector(0, 0, 0);
            var x = new Vector(0.8, 0, 0);
            var y = new Vector(0, 0.8, 0);
            var z = new Vector(0, 0, 0.8);
            graphics3D.DrawLine(
                new Vertex(zero, new Vector(), Color.Red), 
                new Vertex(x, new Vector(), Color.Red));
            graphics3D.DrawPoint(new Vertex(x, new Vector(), Color.Red));
            graphics3D.DrawLine(
                new Vertex(zero, new Vector(), Color.Green), 
                new Vertex(y, new Vector(), Color.Green));
            graphics3D.DrawPoint(new Vertex(y, new Vector(), Color.Green));
            graphics3D.DrawLine(
                new Vertex(zero, new Vector(), Color.Blue), 
                new Vertex(z, new Vector(), Color.Blue));
            graphics3D.DrawPoint(new Vertex(z, new Vector(), Color.Blue));
            Mesh.Draw(graphics3D);
            e.Graphics.DrawImage(graphics3D.ColorBuffer, 0, 0);
        }
    }
}
