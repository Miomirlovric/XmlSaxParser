using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class ColorPicker : Form
    {
        public Dictionary<string, Color> colors;
        public ColorPicker(Dictionary<string, Color> Dic)
        {
            InitializeComponent();

            colors = Dic;
            comboBox1.DataSource = colors.ToList();
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += colorComboBox_SelectedIndexChanged;
            ColorPanel.BackColor = colors["Element"];

        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void colorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Update the color panels to reflect the selected color
            var selectedIndex = comboBox1.SelectedValue;
            ColorPanel.BackColor = (Color)selectedIndex;
        }

        private void ChangeColor_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            if (MyDialog.ShowDialog() == DialogResult.OK) 
            {
                colors[comboBox1.Text] = MyDialog.Color;
                comboBox1.DataSource = colors.ToList();
                colorComboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void CancelB_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
