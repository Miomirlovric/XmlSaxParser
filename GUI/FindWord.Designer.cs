namespace GUI
{
    partial class FindWord
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Find_tb = new TextBox();
            FindButton = new Button();
            FindNextButton = new Button();
            SuspendLayout();
            // 
            // Find_tb
            // 
            Find_tb.Location = new Point(12, 12);
            Find_tb.Name = "Find_tb";
            Find_tb.Size = new Size(216, 23);
            Find_tb.TabIndex = 0;
            // 
            // FindButton
            // 
            FindButton.Location = new Point(12, 41);
            FindButton.Name = "FindButton";
            FindButton.Size = new Size(75, 23);
            FindButton.TabIndex = 1;
            FindButton.Text = "Find";
            FindButton.UseVisualStyleBackColor = true;
            FindButton.Click += FindButton_Click;
            // 
            // FindNextButton
            // 
            FindNextButton.Location = new Point(152, 41);
            FindNextButton.Name = "FindNextButton";
            FindNextButton.Size = new Size(75, 23);
            FindNextButton.TabIndex = 2;
            FindNextButton.Text = "Find Next";
            FindNextButton.UseVisualStyleBackColor = true;
            FindNextButton.Click += FindNextButton_Click;
            // 
            // FindWord
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(239, 74);
            Controls.Add(FindNextButton);
            Controls.Add(FindButton);
            Controls.Add(Find_tb);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FindWord";
            Text = "FindWord";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Find_tb;
        private Button FindButton;
        private Button FindNextButton;
    }
}