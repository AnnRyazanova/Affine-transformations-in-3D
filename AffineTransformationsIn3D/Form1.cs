using System.Windows.Forms;
using System;
using AffineTransformationsIn3D.Geometry;

namespace AffineTransformationsIn3D
{
    public partial class Form1 : Form
    {
        private Mesh CurrentMesh
        {
            get
            {
                return sceneView1.Mesh;
            }
            set
            {
                sceneView1.Mesh = value;
                sceneView2.Mesh = value;
                sceneView3.Mesh = value;
                sceneView4.Mesh = value;
                RefreshScenes();
            }
        }

        private Camera camera;

        public Form1()
        {
            InitializeComponent();
            CurrentMesh = new Tetrahedron(0.5f);
            sceneView1.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0, Transformations.OrthogonalProjection());
            sceneView2.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0, Transformations.OrthogonalProjection()
                * Transformations.RotateY(Math.PI / 2));
            sceneView3.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0, Transformations.OrthogonalProjection()
                * Transformations.RotateX(-Math.PI / 2));

            camera = new Camera(new Vertex(0, 0, 0), Math.PI / 4, -Math.PI / 4, Transformations.OrthogonalProjection());

            sceneView4.ViewCamera = camera;
        }

        private static double DegToRad(double deg)
        {
            return deg / 180 * Math.PI;
        }

        private void RefreshScenes()
        {
            sceneView1.Refresh();
            sceneView2.Refresh();
            sceneView3.Refresh();
            sceneView4.Refresh();
        }

        private void Scale(object sender, EventArgs e)
        {
            double scalingX = (double)numericUpDown1.Value;
            double scalingY = (double)numericUpDown2.Value;
            double scalingZ = (double)numericUpDown3.Value;
            CurrentMesh.Apply(
                Transformations.Scale(scalingX, scalingY, scalingZ));
            RefreshScenes();
        }

        private void Rotate(object sender, EventArgs e)
        {
            double rotatingX = DegToRad((double)numericUpDown4.Value);
            double rotatingY = DegToRad((double)numericUpDown5.Value);
            double rotatingZ = DegToRad((double)numericUpDown6.Value);
            CurrentMesh.Apply(Transformations.RotateX(rotatingX)
                * Transformations.RotateY(rotatingY)
                * Transformations.RotateZ(rotatingZ));
            RefreshScenes();
        }

        private void Translate(object sender, EventArgs e)
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            CurrentMesh.Apply(
                Transformations.Translate(translatingX, translatingY, translatingZ));
            RefreshScenes();
        }

        private void Reflect(object sender, EventArgs e)
        {
            Matrix reflection;
            if (radioButton1.Checked)
                reflection = Transformations.ReflectX();
            else if (radioButton2.Checked)
                reflection = Transformations.ReflectY();
            else if (radioButton3.Checked)
                reflection = Transformations.ReflectZ();
            else throw new Exception("Unreachable statement");
            CurrentMesh.Apply(reflection);
            RefreshScenes();
        }

        private void RotateAroundCenter(object sender, EventArgs e)
        {
            double angleX = DegToRad((double)numericUpDown10.Value);
            double angleY = DegToRad((double)numericUpDown11.Value);
            double angleZ = DegToRad((double)numericUpDown12.Value);
            var p = CurrentMesh.Center;
            CurrentMesh.Apply(Transformations.RotateAroundPoint(p, angleX, angleY, angleZ));
            RefreshScenes();
        }

        private void RotateAroundLine(object sender, EventArgs e)
        {
            Vertex a = new Vertex(
                (double)numericUpDownPoint1X.Value, 
                (double)numericUpDownPoint1Y.Value, 
                (double)numericUpDownPoint1Z.Value);
            Vertex b = new Vertex(
                (double)numericUpDownPoint2X.Value, 
                (double)numericUpDownPoint2Y.Value, 
                (double)numericUpDownPoint2Z.Value);
            var angle = DegToRad((double)numericUpDownAngle.Value);
            CurrentMesh.Apply(Transformations.RotateAroundLine(a, b, angle));
            RefreshScenes();
        }

        private void ChangeModel(object sender, EventArgs e)
        {
            var dialog = new FormChangeModel();
            if (DialogResult.OK != dialog.ShowDialog()) return;
            if (null == dialog.SelectedModel) return;
            CurrentMesh = dialog.SelectedModel;
        }

        private void SaveFile(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CurrentMesh.Save(saveDialog.FileName);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadFile(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                CurrentMesh = new Mesh(openDialog.FileName);
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SceneKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    camera.AngleX += 0.5;
                    break;
                case Keys.Down:
                    camera.AngleX -= 0.5;
                    break;
                case Keys.Left:
                    camera.AngleY += 0.5;
                    break;
                case Keys.Right:
                    camera.AngleY -= 0.5;
                    break;
                case Keys.W:
                    camera.Position *= Transformations.Translate(0, 0, -0.3);
                    break;
                case Keys.S:
                    camera.Position *= Transformations.Translate(0, 0, 0.3);
                    break;
                case Keys.A:
                    camera.Position *= Transformations.Translate(-0.3, 0, 0);
                    break;
                case Keys.D:
                    camera.Position *= Transformations.Translate(0.3, 0, 0);
                    break;
            }
            RefreshScenes();
        }
    }
}
