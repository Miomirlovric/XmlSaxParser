using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FindWord : Form
    {
        RichTextBox RichTextBox { get; set; }

        public FindWord(RichTextBox richTextBox)
        {
            InitializeComponent();
            RichTextBox = richTextBox;
            FindNextButton.Enabled = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void resetFindWord()
        {
            FindNextButton.Enabled = false;
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            int index = RichTextBox.Find(Find_tb.Text, RichTextBox.SelectionStart+1, RichTextBoxFinds.None);
            if (index != -1)
            {
                RichTextBox.Select(index, Find_tb.Text.Length);
                RichTextBox.ScrollToCaret();
                FindNextButton.Enabled = true;
            }
            else
            {
                MessageBox.Show("No word was found");
            }
        }

        private void FindNextButton_Click(object sender, EventArgs e)
        {
            int index = RichTextBox.Find(Find_tb.Text, 0, RichTextBox.SelectionStart, RichTextBoxFinds.Reverse);
            if (index != -1)
            {
                RichTextBox.Select(index, Find_tb.Text.Length);
                RichTextBox.ScrollToCaret();
            }
            else
            {
                MessageBox.Show("No word was found");
            }
        }
    }
}
