using AffineTransformationsIn3D.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AffineTransformationsIn3D
{
    public partial class FormChangeModel : Form
    {
        private IPrimitive selectedModel;

        public IPrimitive SelectedModel { get { return selectedModel; } }

        public FormChangeModel()
        {
            InitializeComponent();
            label4.Text = "";
        }

        private void AddPoint(object sender, EventArgs e)
        {
            var x = (double)numericUpDownX.Value;
            var y = (double)numericUpDownY.Value;
            var z = (double)numericUpDownZ.Value;
            numericUpDownX.Value = 0;
            numericUpDownY.Value = 0;
            numericUpDownZ.Value = 0;
            listBoxPoints.Items.Add(new Point3D(x, y, z));
        }

        private void SelectedPointChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = null != listBoxPoints.SelectedItem;
        }

        private void RemovePoint(object sender, EventArgs e)
        {
            listBoxPoints.Items.RemoveAt(listBoxPoints.SelectedIndex);
        }

        private void Ok(object sender, EventArgs e)
        {
            var tab = tabControl1.SelectedTab;
            if (tabPagePolyhedron == tab)
            {
                if (radioButtonTetrahedron.Checked)
                    selectedModel = Polyhedron.MakeTetrahedron(0.5);
                else
                    selectedModel = Polyhedron.MakeIcosahedron(0.5);
            }
            else if (tabPageRotationFigure == tab)
            {
                var initial = new List<Point3D>();
                foreach (var p in listBoxPoints.Items)
                    initial.Add((Point3D)p);
                int axis;
                if (radioButtonX.Checked) axis = 0;
                else if (radioButtonY.Checked) axis = 1;
                else /* if (radioButtonZ.Checked) */ axis = 2;
                var density = (int)numericUpDownDensity.Value;
                selectedModel = Polyhedron.MakeRotationFigure(initial, axis, density);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var points = new List<Point3D>();
                    var pointsSequence = new List<Triple>();

                    var str = File.ReadAllText(openDialog.FileName).Replace("\r\n", "!");
                    var info = str.Split('!');
                    int index = 0;

                    while (!info[index][0].Equals('v'))
                        index++;

                    while (info[index][0].Equals('v'))
                    {
                        var infoPoint = info[index].Split(' ');

                        float x, y, z;
                        float.TryParse(infoPoint[1], out x);
                        float.TryParse(infoPoint[2], out y);
                        float.TryParse(infoPoint[3], out z);

                        points.Add(new Point3D(x, y, z));
                        index++;
                    }

                    while (!info[index][0].Equals('f'))
                        index++;

                    while (info[index][0].Equals('f'))
                    {
                        var infoPointSeq = info[index].Split(' ');

                        int fst, snd, trd;
                        int.TryParse(infoPointSeq[1], out fst);
                        int.TryParse(infoPointSeq[2], out snd);
                        int.TryParse(infoPointSeq[3], out trd);

                        pointsSequence.Add(new Triple(fst, snd, trd));
                        index++;
                    }

                    selectedModel = new Polyhedron(points, pointsSequence);
                    label4.Text = "Многогранник загружен";
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
