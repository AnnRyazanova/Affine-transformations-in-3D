using AffineTransformationsIn3D.Primitives;
using System;
using System.Collections.Generic;
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
    }
}
