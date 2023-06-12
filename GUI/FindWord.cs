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
        int indexFirst { get; set; }
        string word { get; set; }
        List<int> wordPositions = new List<int>();
        int selectedFromList { get; set; }  
        bool positionChanged { get; set; }

        public FindWord(RichTextBox richTextBox)
        {
            InitializeComponent();
            RichTextBox = richTextBox;
            FindNextButton.Enabled = false; 
            selectedFromList = 0;
            positionChanged = false; 
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
                if (word == null)
                {
                    word = Find_tb.Text;
                }
                if(word != Find_tb.Text)
                {
                    FindNextButton.Enabled = false;
                    word = Find_tb.Text;
                    wordPositions.Clear();
                    indexFirst = 0;
                    selectedFromList = 0;
                }
                if (wordPositions.Count()>0 && positionChanged /*wordPositions[selectedFromList-1]!=indexFirst*/)
                {
                    if(wordPositions[selectedFromList - 1] + word.Length == indexFirst)
                    {
                        positionChanged = false;
                        RichTextBox.Select(wordPositions[selectedFromList-1], Find_tb.Text.Length);
                        RichTextBox.ScrollToCaret();
                    }
                    else
                    {
                        RichTextBox.Select(wordPositions[selectedFromList-1], Find_tb.Text.Length);
                        RichTextBox.ScrollToCaret();
                        selectedFromList++;
                    }                  
                }
                else
                {               
                int index = RichTextBox.Find(Find_tb.Text, indexFirst, RichTextBoxFinds.None);
                if (index != -1)
                {
                    RichTextBox.Select(index, Find_tb.Text.Length);
                    RichTextBox.ScrollToCaret();
                    wordPositions.Add(index);
                    selectedFromList++;
                    indexFirst = index + Find_tb.Text.Length;
                    FindNextButton.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No word was found");
                }
                }
            }
        }

        private void FindNextButton_Click(object sender, EventArgs e)
        {
            if(selectedFromList-1 == 0)
            {
                selectedFromList = 1;
            }
            else
            {
                int index = RichTextBox.Find(Find_tb.Text, wordPositions[selectedFromList-2], RichTextBoxFinds.None);
                if (index != -1)
                {
                    RichTextBox.Select(index, Find_tb.Text.Length);
                    RichTextBox.ScrollToCaret();
                    selectedFromList--;
                    positionChanged = true;
                }
                else
                {
                    MessageBox.Show("No word was found");
                }
            }
        }
    }
}
