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
            this.Find_tb = new System.Windows.Forms.TextBox();
            this.FindButton = new System.Windows.Forms.Button();
            this.FindNextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Find_tb
            // 
            this.Find_tb.Location = new System.Drawing.Point(12, 12);
            this.Find_tb.Name = "Find_tb";
            this.Find_tb.Size = new System.Drawing.Size(216, 23);
            this.Find_tb.TabIndex = 0;
            // 
            // FindButton
            // 
            this.FindButton.Location = new System.Drawing.Point(12, 41);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(75, 23);
            this.FindButton.TabIndex = 1;
            this.FindButton.Text = "Find";
            this.FindButton.UseVisualStyleBackColor = true;
            this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // FindNextButton
            // 
            this.FindNextButton.Location = new System.Drawing.Point(152, 41);
            this.FindNextButton.Name = "FindNextButton";
            this.FindNextButton.Size = new System.Drawing.Size(75, 23);
            this.FindNextButton.TabIndex = 2;
            this.FindNextButton.Text = "Find Next";
            this.FindNextButton.UseVisualStyleBackColor = true;
            this.FindNextButton.Click += new System.EventHandler(this.FindNextButton_Click);
            // 
            // FindWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 74);
            this.Controls.Add(this.FindNextButton);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.Find_tb);
            this.Name = "FindWord";
            this.Text = "FindWord";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox Find_tb;
        private Button FindButton;
        private Button FindNextButton;
    }
}