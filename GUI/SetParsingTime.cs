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
    public partial class SetParsingTime : Form
    {
        public int time { get; set; }
        public SetParsingTime(int time)
        {
            InitializeComponent();
            this.time = time;
            this.numericUpDown1.Value = time/1000;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            time = (int)numericUpDown1.Value * 1000;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
