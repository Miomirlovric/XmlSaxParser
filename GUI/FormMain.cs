using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using XmlSaxParser;
using static GUI.FormMain;
using System.Timers;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Runtime.InteropServices;

namespace GUI
{
    public partial class FormMain : Form
    {
        public delegate void Del();
        public SaxParser parser { get; set; }
        public XmlTree tree { get; set; }
        public bool FileIsChanged { get; set; } = false;
        public string documentPath { get; set; } = "";
        public Timer aTimer { get; set; }
        public FillTreeHandler treehandler { get; set; }
        FindWord findTextDlg;

        public FormMain()
        {
            InitializeComponent();

            findTextDlg = new FindWord(richTextBox);
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Interval = 6000;
            aTimer.Tick += OnTimedEvent;
            parser = new SaxParser();
            tree = new XmlTree();
            treehandler = new FillTreeHandler(treeView, richTextBox) { view = treeView, stack = new Stack<TreeNode>() };
            treeView.ImageList = imageList1;
            parser.XmlElementStart += treehandler.ElementStartHandler;
            parser.XmlElementEnd += treehandler.ElementEndHandler;
            parser.XmlText += treehandler.TextHandler;
            parser.XmlComment += treehandler.CommentHandler;
            parser.XmlCDATA += treehandler.CDATAHandler;
            parser.XmlAtribute += treehandler.AtributeHandler;
        }

        public class FillTreeHandler
        {
            public TreeView view { get; set; }
            public Stack<TreeNode> stack { get; set; }
            public Dictionary<string, Color> Colors { get; set; }
            public RichTextBox RichTextBox { get; set; }

            public FillTreeHandler(TreeView view, RichTextBox richTextBox)
            {
                this.RichTextBox = richTextBox;
                this.view = view;
                this.stack = new Stack<TreeNode>();
                Colors = new Dictionary<string, Color>()
            {
                { "Element", Color.Blue },{ "Comment", Color.Black },{ "Text", Color.Black },
                { "CDATA", Color.Yellow }, { "Atribute", Color.Blue },{"Key",Color.Red},{"Value",Color.Purple}
            };
            }

            private void ColorSelections(Position position, int length, string NodeType, int KeyLength = 0)
            {
                var ln = position.LineNumber - 1;
                var lp = position.LinePosition - 1;
                var i = RichTextBox.GetFirstCharIndexFromLine(ln);
                i = i + lp;
                if (NodeType != "Atribute")
                {
                    //RichTextBox.DeselectAll();
                    RichTextBox.Select(i, length);
                    RichTextBox.SelectionColor = Colors[NodeType];
                }
                else
                {
                    //RichTextBox.DeselectAll();
                    RichTextBox.Select(i, KeyLength);
                    RichTextBox.SelectionColor = Colors["Key"];
                    //RichTextBox.DeselectAll();
                    RichTextBox.Select(i + KeyLength, length - KeyLength);
                    RichTextBox.SelectionColor = Colors["Value"];
                }
                RichTextBox.DeselectAll();
            }

            public void ElementStartHandler(object sender, XmlElementStartEventArgs args)
            {
                // If element stack is empty, this element is the root.
                if (stack.Count == 0)
                {
                    var node = view.Nodes.Add(args.Name);
                    node.ForeColor = Colors["Element"];
                    node.Tag = new XmlElement(args.Name) { Position = args.Position };
                    node.ImageIndex = 0;
                    stack.Push(node);
                }
                // If new element is not the root element then add it as a child of current active element, which is the top element on the stack.
                else
                {
                    var node = stack.Peek().Nodes.Add(args.Name);
                    node.ForeColor = Colors["Element"];
                    node.Tag = new XmlElement(args.Name) { Position = args.Position };
                    node.ImageIndex = 1;
                    stack.Push(node);
                }
                ColorSelections(args.Position, args.Name.Count(), "Element");
            }

            public void ElementEndHandler(object sender, XmlElementEndEventArgs args)
            {
                if (stack.Count == 0)
                {
                    throw new Exception("Invalid XML document");
                }
                var elementRemoved = stack.Pop();
                if (elementRemoved.Text != args.Name)
                {
                    throw new Exception("Closing tag name is different from opening tag name");
                }
                ColorSelections(args.position, args.Name.Count(), "Element");
            }
            public void TextHandler(object sender, XmlTextEventArgs args)
            {
                if (stack.Count == 0)
                {
                    throw new Exception("XmlText node must have parent XmlElement node");
                }
                // Add XmlText node to parent XmlElement.
                var node = stack.Peek().Nodes.Add(args.Text);
                node.ForeColor = Colors["Text"];
                node.Tag = new XmlElement(args.Text) { Position = args.Position };
                node.ImageIndex = 2;
                ColorSelections(args.Position, args.Text.Count(), "Text");
            }
            public void AtributeHandler(object sender, XmlAtributeEventArgs args)
            {
                if (stack.Count == 0)
                {
                    throw new Exception("XmlAtribute must be inside opening tag");
                }
                // Add atribute name and value to the XmlElement.
                var node = stack.Peek().Nodes.Add(args.Name + "=\"" + args.Value + "\"");
                node.ForeColor = Colors["Atribute"];
                node.Tag = new XmlAttribute(args.Name, args.Value, args.Position);
                node.ImageIndex = 4;
                ColorSelections(args.Position, args.Name.Count() + args.Value.Count() + 3, "Atribute", args.Name.Count());
            }
            public void CommentHandler(object sender, XmlCommentEventArgs args)
            {
                if (stack.Count == 0)
                {
                    throw new Exception("Comment needs to be after first tag");
                }
                else
                {
                    var node = stack.Peek().Nodes.Add(args.Comment);
                    node.ForeColor = Colors["Comment"];
                    node.Tag = new XmlComment(args.Comment) { Position = args.Position };
                    node.ImageIndex = 2;
                    ColorSelections(args.Position, args.Comment.Count(), "Comment");
                }
                // Add atribute name and value to the XmlElement.
            }
            public void CDATAHandler(object sender, XmlCDATAEventArgs args)
            {
                if (stack.Count == 0)
                {
                    throw new Exception("CDATA needs to be after first tag");
                }
                var node = stack.Peek().Nodes.Add(args.CDATA);
                node.ForeColor = Colors["CDATA"];
                node.ImageIndex = 3;
                ColorSelections(args.Position, args.CDATA.Count(), "CDATA");
                // Add atribute name and value to the XmlElement.
            }
        }

        private async void OnTimedEvent(Object myObject, EventArgs myEventArgs)
        {
            aTimer.Stop();
            var selectionStart = richTextBox.SelectionStart;
            var selectionLength = richTextBox.SelectionLength;
            LockWindowUpdate((long)richTextBox.Handle);
            await DelegateMethod();
            richTextBox.SelectionStart = selectionStart;
            richTextBox.SelectionLength = selectionLength;
            LockWindowUpdate(0);
            richTextBox.Invalidate();
        }

        private bool updating = false;

        public async Task DelegateMethod()
        {
            try
            {
                if (richTextBox.TextLength != 0)
                {
                    updating = true;
                    treeView.Nodes.Clear();
                    using (TextReader reader = new StringReader(richTextBox.Text))
                    {
                        await parser.Parse(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation eror", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                updating = false;
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                FileIsChanged = true;
                aTimer.Start();
            }
        }

        [DllImport("user32.dll")]
        private static extern long LockWindowUpdate(long Handle);

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Clear();

                //Get the path of specified file
                documentPath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                using StreamReader reader = new StreamReader(fileStream);
                var fileContent = reader.ReadToEnd();

                aTimer.Stop();
                updating = true;

                Cursor.Current = Cursors.WaitCursor;
                LockWindowUpdate((long)richTextBox.Handle);
                richTextBox.Text = fileContent;
                Reload();
                LockWindowUpdate(0);
                richTextBox.Select(0, 0);
                richTextBox.Invalidate();

                updating = false;
                FileIsChanged = false;
            }
        }

        private async void Reload()
        {
            await DelegateMethod();
        }

        private void Clear()
        {
            updating = true;
            richTextBox.Clear();
            treeView.Nodes.Clear();
            updating = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (documentPath != string.Empty && richTextBox.Text != string.Empty)
            {
                File.WriteAllText(documentPath, richTextBox.Text);
                MessageBox.Show("File saved", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FileIsChanged = false;
            }
            else
            {
                saveAsToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
            documentPath = String.Empty;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                writer.Write(richTextBox.Text);
                // Ako koristite using, ne treba Vam Dispose
                // writer.Dispose();
                // ...niti Close (to su iste operacije)
                //writer.Close();

                documentPath = saveFileDialog.FileName;
                MessageBox.Show("File saved", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FileIsChanged = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FileIsChanged)
            {
                var res = MessageBox.Show(this, "You didnt save some changes. Are u sure u wish to exit?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (res != DialogResult.Yes)
                {
                    e.Cancel = true;
                    saveToolStripMenuItem_Click(this, new EventArgs());
                    return;
                }
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var x = e.Node.Tag;
            var length = e.Node.Text.Count();
            var ln = ((XmlNode)x).Position.LineNumber - 1;
            var lp = ((XmlNode)x).Position.LinePosition - 1;
            var i = richTextBox.GetFirstCharIndexFromLine(ln);
            i = i + lp;
            richTextBox.DeselectAll();
            richTextBox.Select(i, length);
        }

        private void nodeColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorPicker dlg = new ColorPicker(treehandler.Colors))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var result = dlg.colors;
                    treehandler.Colors = result;
                    Reload();
                }
            }
        }

        private void editorTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog font = new FontDialog())
            {
                if (font.ShowDialog() == DialogResult.OK)
                {
                    LockWindowUpdate((long)richTextBox.Handle);
                    richTextBox.Font = font.Font;
                    Reload();
                    richTextBox.Select(0, 0);
                    LockWindowUpdate(0);
                    richTextBox.Invalidate();
                }
            }
        }

        private void nodeTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog font = new FontDialog())
            {

                if (font.ShowDialog() == DialogResult.OK)
                {
                    LockWindowUpdate((long)richTextBox.Handle);
                    Size size = new Size((int)font.Font.Size * 2, (int)font.Font.Size * 2);
                    treeView.ImageList.ImageSize = size;
                    treeView.Font = font.Font;
                    Reload();
                    LockWindowUpdate(0);
                    richTextBox.Invalidate();
                }
            }
        }

        private void searchWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            findTextDlg.Show(this);
        }
    }
}