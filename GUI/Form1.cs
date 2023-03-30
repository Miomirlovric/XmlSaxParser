using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using XmlSaxParser;
using static GUI.Form1;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GUI
{
    public partial class Form1 : Form
    {
        public delegate void Del();
        Del del;
        public string document { get; set; } = "";
        public SaxParser parser { get; set; }
        public XmlTree tree { get; set; }
        public FillTreeControl fillTreeControl { get; set; }
        //public bool Wait { get; set; }
        public string documentPath { get; set; } = "";
        public Timer aTimer { get; set; }
        public Form1()
        {
            InitializeComponent();
            del = DelegateMethod;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 6000;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            //Wait = true;
            parser = new SaxParser();
            tree = new XmlTree();
            fillTreeControl = new FillTreeControl(treeView1, tree);
            parser.XmlElementStart += tree.ElementStartHandler;
            parser.XmlElementEnd += tree.ElementEndHandler;
            parser.XmlText += tree.TextHandler;
            parser.XmlAtribute += tree.AtributeHandler;
            parser.XmlComment += tree.CommentHandler;
            parser.XmlCDATA += tree.CDATAHandler;
            //using (TextReader reader = new StringReader(document)) //"<document><child1><child1></child1><child2></child2></child1><child2></child2></document>"
            //{
            //    var result = parser.Parse(reader);               
            //}
            //fillTreeControl.FillTree(treeView1, tree);        
        }

        public class FillTreeControl
        {
            public TreeView view { get; set; }
            public XmlTree tree { get; set; }

            public FillTreeControl(TreeView view, XmlTree tree)
            {
                this.view = view;
                this.tree = tree;
            }

            public void FillTree(TreeView view, XmlTree tree)
            {
                view.BeginUpdate();
                view.Nodes.Clear();
                view.Nodes.Add(((XmlElement)tree.Root).Name);
                IterateThroughTree((List<XmlNode>)tree.Root.Children, view.Nodes[0]);
                view.EndUpdate();
            }

            public void RefreshTree()
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
                        if (((XmlElement)x).Attributes.Count > 0)
                        {
                            string atribute = "";
                            foreach (var i in ((XmlElement)x).Attributes)
                            {
                                atribute += i.Key + i.Value;
                            }
                            return ((XmlElement)x).Name + atribute;
                        }
                        else
                        {
                            return ((XmlElement)x).Name;
                        }
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
        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //using (TextReader reader = new StringReader(document))
            //{
            //    var result = parser.Parse(reader);

            //}
            //fillTreeControl.FillTree(treeView1, tree);
            DelegateMethod();
        }
        public void DelegateMethod()
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new Del(DelegateMethod);
                richTextBox1.Invoke(d, null);   
            }
            else
            {
                try 
                {
                    using (TextReader reader = new StringReader(document))
                    {
                        var result = parser.Parse(reader);

                    }
                    fillTreeControl.FillTree(treeView1, tree);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message,"Validation eror",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            document = richTextBox1.Text.ToString();
            aTimer.Enabled=false;
            aTimer.Enabled = true;
            //if (Wait)
            //{
            //    Wait = false;
            //    await Task.Delay(10000);
            //    document = richTextBox1.Text;
            //    using (TextReader reader = new StringReader(document)) //"<document><child1><child1></child1><child2></child2></child1><child2></child2></document>"
            //    {
            //        var result = parser.Parse(reader);

            //    }
            //    fillTreeControl.FillTree(treeView1, tree);
            //    Wait = true;
            //}
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;            

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    documentPath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            document = fileContent;
            richTextBox1.Text = document;
            DelegateMethod();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (documentPath != string.Empty && document != string.Empty)
            {
                File.WriteAllText(documentPath, document);
                MessageBox.Show("File saved", "Notification", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                saveAsToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            richTextBox1.Text = String.Empty;
            document = String.Empty;
            documentPath = String.Empty;
            fillTreeControl.RefreshTree();
            MessageBox.Show("New File created", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());

                writer.Write(document);

                writer.Dispose();

                writer.Close();

                documentPath = saveFileDialog1.FileName;
                MessageBox.Show("File saved", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}