using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using XmlSaxParser;
using static GUI.Form1;
using System.Timers;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GUI
{
    public partial class Form1 : Form
    {
        public delegate void Del();
        public SaxParser parser { get; set; }
        public XmlTree tree { get; set; }
        public bool FileIsChanged { get; set; } = false;
        public string documentPath { get; set; } = "";
        public Timer aTimer { get; set; }
        public FillTreeHandler treehandler { get; set;}
        FindWord dlg;

        public Form1()
        {
            InitializeComponent();

            dlg = new FindWord(richTextBox1);
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Interval = 6000;
            aTimer.Tick += OnTimedEvent;
            //Wait = true;
            parser = new SaxParser();
            tree = new XmlTree();
            treehandler = new FillTreeHandler(treeView1,richTextBox1) { view = treeView1,stack = new Stack<TreeNode>()};  
            treeView1.ImageList = imageList1;
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
            
            public FillTreeHandler(TreeView view,RichTextBox richTextBox)
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

            private void ColorSelections(Position position,int length,string NodeType,int KeyLength = 0)
            {
                var ln = position.LineNumber - 1;
                var lp = position.LinePosition - 1;
                var i = RichTextBox.GetFirstCharIndexFromLine(ln);
                i = i + lp;
                if (NodeType != "Atribute")
                {
                    RichTextBox.DeselectAll();
                    RichTextBox.Select(i, length);
                    RichTextBox.SelectionColor = Colors[NodeType];
                }
                else
                {

                    RichTextBox.DeselectAll();
                    RichTextBox.Select(i, KeyLength);
                    RichTextBox.SelectionColor = Colors["Key"];
                    RichTextBox.DeselectAll();
                    RichTextBox.Select(i+KeyLength, length-KeyLength);
                    RichTextBox.SelectionColor = Colors["Value"];
                    RichTextBox.DeselectAll();
                }
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
                ColorSelections(args.Position, args.Name.Count(),"Element");
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
                var node = stack.Peek().Nodes.Add(args.Name+"=\""+args.Value+"\"");
                node.ForeColor = Colors["Atribute"];
                node.Tag = new XmlAttribute(args.Name,args.Value,args.Position);
                node.ImageIndex = 4;
                ColorSelections(args.Position, args.Name.Count()+args.Value.Count()+3, "Atribute",args.Name.Count());
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
                    node.Tag = new XmlComment(args.Comment) { Position = args.Position};
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
    
         await DelegateMethod(); 
         aTimer.Stop();

        }
        public async Task DelegateMethod()
        {
            try
            {
                if (richTextBox1.Text != null)
                {
                    treeView1.Nodes.Clear();
                    using (TextReader reader = new StringReader(richTextBox1.Text))
                    {
                        await parser.Parse(reader);
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation eror", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            FileIsChanged = true;
            aTimer.Stop();
            aTimer.Start();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;

            using (openFileDialog1)
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileIsChanged = false;
                    //Get the path of specified file
                    documentPath = openFileDialog1.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog1.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    richTextBox1.Text = fileContent;
                    DelegateMethod();
                    aTimer.Stop();
                    FileIsChanged = false;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (documentPath != string.Empty && richTextBox1.Text != string.Empty)
            {
                File.WriteAllText(documentPath, richTextBox1.Text);
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
            richTextBox1.Text = String.Empty;
            richTextBox1.Text = String.Empty;
            documentPath = String.Empty;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());

                writer.Write(richTextBox1.Text);

                writer.Dispose();

                writer.Close();

                documentPath = saveFileDialog1.FileName;
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
            var i = richTextBox1.GetFirstCharIndexFromLine(ln);
            i = i + lp;
            richTextBox1.DeselectAll();
            richTextBox1.Select(i, length);
        }

        private void nodeColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorPicker dlg = new ColorPicker(treehandler.Colors))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var result = dlg.colors;
                    treehandler.Colors = result;
                    DelegateMethod();
                }
            }
        }

        private void editorTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog font = new FontDialog())
            {
                if (font.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.Font = font.Font;
                    DelegateMethod();
                }
            }
        }

        private void nodeTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog font = new FontDialog())
            {

                if (font.ShowDialog() == DialogResult.OK)
                {
                    Size size = new Size((int)font.Font.Size * 2, (int)font.Font.Size * 2);
                    treeView1.ImageList.ImageSize = size;
                    treeView1.Font = font.Font;
                    DelegateMethod();
                }
            }
        }

        private void searchWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
                dlg.Show(this);              
        }
    }
}