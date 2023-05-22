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
    public partial class FindWord : Form
    {
        RichTextBox RichTextBox { get; set; }
        int indexFirst { get; set; }

        public FindWord(RichTextBox richTextBox)
        {
            InitializeComponent();
            RichTextBox = richTextBox;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            if (RichTextBox.Text != String.Empty && Find_tb.Text != String.Empty)
            {
                indexFirst = 0;
                int index = RichTextBox.Find(Find_tb.Text, indexFirst, RichTextBoxFinds.None);
                if (index != -1)
                {
                    RichTextBox.Select(index, Find_tb.Text.Length);
                    RichTextBox.ScrollToCaret();
                    indexFirst = index + Find_tb.Text.Length;
                }
                else
                {
                    MessageBox.Show("No word was found");
                }
            }
        }

        private void FindNextButton_Click(object sender, EventArgs e)
        {
            if (RichTextBox.Text != String.Empty && Find_tb.Text != String.Empty)
            {
                int index = RichTextBox.Find(Find_tb.Text, indexFirst, RichTextBoxFinds.None);
                if (index != -1)
                {
                    RichTextBox.Select(index, Find_tb.Text.Length);
                    RichTextBox.ScrollToCaret();
                    indexFirst = index + Find_tb.Text.Length;
                }
                else
                {
                    MessageBox.Show("Last word found");
                    indexFirst = 0;
                }
            }
        }
    }
}
