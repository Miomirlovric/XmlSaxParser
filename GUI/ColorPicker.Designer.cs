namespace GUI
{
    partial class ColorPicker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ApplyButton = new Button();
            comboBox1 = new ComboBox();
            ColorPanel = new Panel();
            ChangeColor = new Button();
            CancelB = new Button();
            SuspendLayout();
            // 
            // ApplyButton
            // 
            ApplyButton.Location = new Point(103, 76);
            ApplyButton.Name = "ApplyButton";
            ApplyButton.Size = new Size(75, 23);
            ApplyButton.TabIndex = 0;
            ApplyButton.Text = "Apply";
            ApplyButton.UseVisualStyleBackColor = true;
            ApplyButton.Click += ApplyButton_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(20, 20);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 1;
            // 
            // ColorPanel
            // 
            ColorPanel.Location = new Point(156, 22);
            ColorPanel.Name = "ColorPanel";
            ColorPanel.Size = new Size(22, 21);
            ColorPanel.TabIndex = 2;
            // 
            // ChangeColor
            // 
            ChangeColor.Location = new Point(184, 22);
            ChangeColor.Name = "ChangeColor";
            ChangeColor.Size = new Size(75, 23);
            ChangeColor.TabIndex = 3;
            ChangeColor.Text = "Select";
            ChangeColor.UseVisualStyleBackColor = true;
            ChangeColor.Click += ChangeColor_Click;
            // 
            // CancelB
            // 
            CancelB.Location = new Point(184, 76);
            CancelB.Name = "CancelB";
            CancelB.Size = new Size(75, 23);
            CancelB.TabIndex = 4;
            CancelB.Text = "Cancel";
            CancelB.UseVisualStyleBackColor = true;
            CancelB.Click += CancelB_Click;
            // 
            // ColorPicker
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(278, 136);
            Controls.Add(CancelB);
            Controls.Add(ChangeColor);
            Controls.Add(ColorPanel);
            Controls.Add(comboBox1);
            Controls.Add(ApplyButton);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ColorPicker";
            Text = "Select color";
            ResumeLayout(false);
        }

        #endregion

        private Button ApplyButton;
        private ComboBox comboBox1;
        private Panel ColorPanel;
        private Button ChangeColor;
        private Button CancelB;
    }
}
