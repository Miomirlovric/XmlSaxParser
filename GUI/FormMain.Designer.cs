namespace GUI
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            treeView = new TreeView();
            richTextBox = new RichTextBox();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            nodeColorsToolStripMenuItem = new ToolStripMenuItem();
            editorTextToolStripMenuItem = new ToolStripMenuItem();
            nodeTextToolStripMenuItem = new ToolStripMenuItem();
            searchWordToolStripMenuItem = new ToolStripMenuItem();
            timer1 = new System.Windows.Forms.Timer(components);
            fileSystemWatcher1 = new FileSystemWatcher();
            saveFileDialog = new SaveFileDialog();
            splitter1 = new Splitter();
            openFileDialog = new OpenFileDialog();
            imageList1 = new ImageList(components);
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // treeView
            // 
            treeView.BackColor = SystemColors.GradientActiveCaption;
            treeView.Dock = DockStyle.Left;
            treeView.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            treeView.Location = new Point(0, 24);
            treeView.Name = "treeView";
            treeView.Size = new Size(280, 426);
            treeView.TabIndex = 0;
            treeView.NodeMouseDoubleClick += treeView1_NodeMouseDoubleClick;
            // 
            // richTextBox
            // 
            richTextBox.BackColor = SystemColors.GradientInactiveCaption;
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox.HideSelection = false;
            richTextBox.Location = new Point(280, 24);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(520, 426);
            richTextBox.TabIndex = 1;
            richTextBox.Text = "";
            richTextBox.WordWrap = false;
            richTextBox.TextChanged += richTextBox1_TextChanged;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = SystemColors.InactiveBorder;
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, settingsToolStripMenuItem, searchWordToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(800, 24);
            menuStrip.TabIndex = 2;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, newToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(112, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(112, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(112, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(112, 22);
            saveAsToolStripMenuItem.Text = "Save as";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(112, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nodeColorsToolStripMenuItem, editorTextToolStripMenuItem, nodeTextToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // nodeColorsToolStripMenuItem
            // 
            nodeColorsToolStripMenuItem.Name = "nodeColorsToolStripMenuItem";
            nodeColorsToolStripMenuItem.Size = new Size(138, 22);
            nodeColorsToolStripMenuItem.Text = "Node colors";
            nodeColorsToolStripMenuItem.Click += nodeColorsToolStripMenuItem_Click;
            // 
            // editorTextToolStripMenuItem
            // 
            editorTextToolStripMenuItem.Name = "editorTextToolStripMenuItem";
            editorTextToolStripMenuItem.Size = new Size(138, 22);
            editorTextToolStripMenuItem.Text = "Editor text";
            editorTextToolStripMenuItem.Click += editorTextToolStripMenuItem_Click;
            // 
            // nodeTextToolStripMenuItem
            // 
            nodeTextToolStripMenuItem.Name = "nodeTextToolStripMenuItem";
            nodeTextToolStripMenuItem.Size = new Size(138, 22);
            nodeTextToolStripMenuItem.Text = "Node text";
            nodeTextToolStripMenuItem.Click += nodeTextToolStripMenuItem_Click;
            // 
            // searchWordToolStripMenuItem
            // 
            searchWordToolStripMenuItem.Name = "searchWordToolStripMenuItem";
            searchWordToolStripMenuItem.Size = new Size(86, 20);
            searchWordToolStripMenuItem.Text = "Search Word";
            searchWordToolStripMenuItem.Click += searchWordToolStripMenuItem_Click;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            // 
            // splitter1
            // 
            splitter1.Location = new Point(280, 24);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 426);
            splitter1.TabIndex = 3;
            splitter1.TabStop = false;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "icons8-tree-64.png");
            imageList1.Images.SetKeyName(1, "icons8-branch-60.png");
            imageList1.Images.SetKeyName(2, "icons8-align-text-left-50.png");
            imageList1.Images.SetKeyName(3, "icons8-comments-24.png");
            imageList1.Images.SetKeyName(4, "icons8-filled-circle-48.png");
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(800, 450);
            Controls.Add(splitter1);
            Controls.Add(richTextBox);
            Controls.Add(treeView);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "FormMain";
            Text = "XML Tree Editor";
            FormClosing += Form1_FormClosing;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView treeView;
        private RichTextBox richTextBox;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private FileSystemWatcher fileSystemWatcher1;
        private SaveFileDialog saveFileDialog;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private Splitter splitter1;
        private OpenFileDialog openFileDialog;
        private ImageList imageList1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem nodeColorsToolStripMenuItem;
        private ToolStripMenuItem editorTextToolStripMenuItem;
        private ToolStripMenuItem nodeTextToolStripMenuItem;
        private ToolStripMenuItem searchWordToolStripMenuItem;
    }
}