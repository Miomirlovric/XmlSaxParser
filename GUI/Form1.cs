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
        public FillTreeControl fillTreeControl { get; set; }
        public bool FileIsChanged { get; set; } = false;
        public string documentPath { get; set; } = "";
        public Timer aTimer { get; set; }
        public FillTreeHandler treehandler { get; set;}

        public Form1()
        {
            InitializeComponent();
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Interval = 6000;
            aTimer.Tick += OnTimedEvent;
            //Wait = true;
            parser = new SaxParser();
            tree = new XmlTree();
            treehandler = new FillTreeHandler() { view = treeView1,stack = new Stack<TreeNode>()};  
            fillTreeControl = new FillTreeControl(treeView1, tree);
            treeView1.ImageList = imageList1;
            //parser.XmlElementStart += tree.ElementStartHandler;
            //parser.XmlElementEnd += tree.ElementEndHandler;
            //parser.XmlText += tree.TextHandler;
            //parser.XmlAtribute += tree.AtributeHandler;
            //parser.XmlComment += tree.CommentHandler;
            //parser.XmlCDATA += tree.CDATAHandler;
            parser.XmlElementStart += treehandler.ElementStartHandler;
            parser.XmlElementEnd += treehandler.ElementEndHandler;
            parser.XmlText += treehandler.TextHandler;
        }

        public class FillTreeHandler
        {
            public TreeView view { get; set; }
            public Stack<TreeNode> stack { get; set; }

            public void ElementStartHandler(object sender, XmlElementStartEventArgs args)
            {
                var newElement = new XmlElement(args.Name) { Position = args.Position };
                // If element stack is empty, this element is the root.
                if (stack.Count == 0)
                {
                    var node = view.Nodes.Add(args.Name);
                    node.ForeColor = Color.Aqua;
                    stack.Push(node);
                }
                // If new element is not the root element then add it as a child of current active element, which is the top element on the stack.
                else
                {
                    var node = stack.Peek().Nodes.Add(args.Name);
                    node.ForeColor = Color.Aqua;
                    stack.Push(node);
                }
            }

            public void ElementEndHandler(object sender, XmlElementEndEventArgs args)
            {
                if (stack.Count == 0)
                {
                    throw new Exception("Invalid XML document");
                }
                var elementRemoved = stack.Pop();
                if (elementRemoved.Name != args.Name)
                {
                    throw new Exception("Closing tag name is different from opening tag name");
                }
            }
            public void TextHandler(object sender, XmlTextEventArgs args)
            {
                if (stack.Count == 0)
                {
                    throw new Exception("XmlText node must have parent XmlElement node");
                }
                // Add XmlText node to parent XmlElement.
                stack.Peek().Nodes.Add(args.Text);
            }
        }

        public class FillTreeControl
        {
            public TreeView view { get; set; }
            public XmlTree tree { get; set; }
            public Dictionary<string, Color> Colors;

            public FillTreeControl(TreeView view, XmlTree tree)
            {
                this.view = view;
                this.tree = tree;
                Colors = new Dictionary<string, Color>()
            {
                { "Element", Color.Green },{ "Comment", Color.LightGreen },{ "Text", Color.GreenYellow },
                { "CDATA", Color.LightBlue }, { "Atribute", Color.Blue }
            };
            }

            public void FillTree(TreeView view, XmlTree tree)
            {
                view.BeginUpdate();
                view.Nodes.Clear();
                view.Nodes.Add(((XmlElement)tree.Root).Name);
                foreach (var atribute in ((XmlElement)tree.Root).Attributes)
                {
                    var x = view.Nodes[0].Nodes.Add(atribute.Key + "=\"" + atribute.Value.Value + "\"");
                    x.Tag = atribute.Value.Position;
                }
                view.Nodes[0].ImageIndex = 0;
                view.Nodes[0].ForeColor = Colors["Element"];
                view.Nodes[0].Tag = tree.Root.Position;
                IterateThroughTree((List<XmlNode>)tree.Root.Children, view.Nodes[0]);
                view.EndUpdate();
            }

            public void ClearTree()
            {
                view.BeginUpdate();
                view.Nodes.Clear();
                view.EndUpdate();
            }

            private void IterateThroughTree(List<XmlNode> nodes, TreeNode prevNode)
            {
                int counter = 0;
                if (nodes.Count > 0)
                {
                    foreach (var node in nodes)
                    {
                        prevNode.Nodes.Add(GetValueOfNode(node));
                        prevNode.Nodes[counter].Tag = node.Position;
                        if (node.NodeType == NodeType.Element)
                        {
                            if (((XmlElement)node).Attributes.Count > 0)
                            {
                                int atCounter = 0;
                                foreach (var atribute in ((XmlElement)node).Attributes)
                                {
                                    prevNode.Nodes[counter].Nodes.Add(atribute.Key + "=\"" + atribute.Value.Value + "\"");
                                    prevNode.Nodes[counter].Nodes[atCounter].Tag = atribute.Value.Position;
                                    ColorNodes(atribute.Value, prevNode.Nodes[counter].Nodes[atCounter]);
                                    atCounter++;
                                }
                            }
                        }
                        ColorNodes(node, prevNode.Nodes[counter]);
                        IterateThroughTree((List<XmlNode>)node.Children, prevNode.Nodes[counter]);
                        counter++;
                    }
                }
            }

            private string GetValueOfNode(XmlNode x)
            {
                switch (x.NodeType)
                {
                    case NodeType.Element:
                        return ((XmlElement)x).Name;
                    case NodeType.Comment:
                        return ((XmlComment)x).Comment;
                    case NodeType.Text:
                        return ((XmlText)x).Text;
                    case NodeType.CDATA:
                        return ((XmlCDATA)x).CDATA;
                    default:
                        throw new Exception("unknown node type");
                }
            }
            public void ColorNodes(XmlNode x, TreeNode prevNode)
            {
                switch (x.NodeType)
                {
                    case NodeType.Element:
                        prevNode.ForeColor = Colors["Element"];
                        prevNode.ImageIndex = 1;
                        break;
                    case NodeType.Comment:
                        prevNode.ForeColor = Colors["Comment"];
                        prevNode.ImageIndex = 2;
                        break;
                    case NodeType.Text:
                        prevNode.ForeColor = Colors["Text"];
                        prevNode.ImageIndex = 2;
                        break;
                    case NodeType.CDATA:
                        prevNode.ForeColor = Colors["CDATA"];
                        prevNode.ImageIndex = 3;
                        break;
                    case NodeType.Atribute:
                        prevNode.ForeColor = Colors["Atribute"];
                        prevNode.ImageIndex = 4;
                        break;
                    default:
                        throw new Exception("unknown node type");
                }
            }
        }
        private void OnTimedEvent(Object myObject, EventArgs myEventArgs)
        {
            DelegateMethod();
            aTimer.Stop();
        }
        public void DelegateMethod()
        {
            try
            {
                if (richTextBox1.Text != null)
                {
                    using (TextReader reader = new StringReader(richTextBox1.Text))
                    {
                        var result = parser.Parse(reader);

                    }
                    //if (tree.Root != null)
                    //{
                    //    fillTreeControl.FillTree(treeView1, tree);
                    //    treeView1.ExpandAll();
                    //}
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
            fillTreeControl.ClearTree();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();

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
            var ln = ((Position)x).LineNumber - 1;
            var lp = ((Position)x).LinePosition - 1;
            var i = richTextBox1.GetFirstCharIndexFromLine(ln);
            i = i + lp;
            richTextBox1.DeselectAll();
            richTextBox1.Select(i, length);
        }

        private void nodeColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorPicker dlg = new ColorPicker(fillTreeControl.Colors))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var result = dlg.colors;
                    fillTreeControl.Colors = result;
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
    }
}