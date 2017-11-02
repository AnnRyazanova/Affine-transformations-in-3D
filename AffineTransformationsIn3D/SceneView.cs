
using AffineTransformationsIn3D.Primitives;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AffineTransformationsIn3D
{
    class SceneView : Control
    {
        private List<IPrimitive> scene = new List<IPrimitive>();
        private Transformation view = Transformation.Identity();
        private Transformation projection = Transformation.OrthogonalProjection();

        public List<IPrimitive> Scene
        {
            get { return scene; }
            set
            {
                scene = value;
                Invalidate();
            }
        }

        public Transformation View
        {
            get { return view; }
            set
            {
                view = value;
                Invalidate();
            }
        }

        public Transformation Projection
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
            if (null == scene) return;
            foreach (var primitive in scene)
                primitive.Draw(e.Graphics, view * projection, Width, Height);
        }
    }
}
