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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TreeView = System.Windows.Forms.TreeView;
using System.Drawing;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using XmlElement = XmlSaxParser.XmlElement;
using XmlAttribute = XmlSaxParser.XmlAttribute;
using XmlNode = XmlSaxParser.XmlNode;
using XmlComment = XmlSaxParser.XmlComment;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using XmlDeclaration = XmlSaxParser.XmlDeclaration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

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
        private bool updating = false;
        private string AppName { get; set; }
        private string SelectedFileName { get; set; }
        [DllImport("user32.dll")]
        private static extern long LockWindowUpdate(long Handle);

        public FormMain()
        {
            InitializeComponent();
            findTextDlg = new FindWord(richTextBox);
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Tick += OnTimedEvent;
            parser = new SaxParser();
            tree = new XmlTree();
            treehandler = new FillTreeHandler(treeView, richTextBox, aTimer) { view = treeView, stack = new Stack<TreeNode>() };
            treeView.ImageList = imageList1;
            KeyPreview = true;
            parser.XmlElementStart += treehandler.ElementStartHandler;
            parser.XmlElementEnd += treehandler.ElementEndHandler;
            parser.XmlText += treehandler.TextHandler;
            parser.XmlComment += treehandler.CommentHandler;
            parser.XmlCDATA += treehandler.CDATAHandler;
            parser.XmlAtribute += treehandler.AtributeHandler;
            parser.XmlDeclaration += treehandler.DeclarationHandler;
            parser.XmlProcesing += treehandler.DeclarationHandler;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            AppName = "XML Tree Viewer ";
            SelectedFileName = "NewDocument";
            this.Text = AppName;
        }

        public class FillTreeHandler
        {
            public TreeView view { get; set; }
            public Stack<TreeNode> stack { get; set; }
            public Dictionary<string, Color> Colors { get; set; }
            public RichTextBox RichTextBox { get; set; }
            public bool ConfigurationFileMissing { get; set; } = false;
            public Timer Timer { get; set; }

            public FillTreeHandler(TreeView view, RichTextBox richTextBox, Timer timer)
            {
                this.RichTextBox = richTextBox;
                this.view = view;
                this.stack = new Stack<TreeNode>();
                Colors = new Dictionary<string, Color>();
                Timer = timer;
                ResetColors();

                // Read Font settings.

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (File.Exists(config.FilePath))
                {
                    var x = config.AppSettings.Settings.AllKeys.ToList();
                    List<string> missing = new List<string>() { "ParsedTime", "EditorFont", "NodeFont", "Element", "Comment", "Text", "CDATA", "Atribute", "Key", "Value" };

                    if (x.All(item => missing.Contains(item)))
                    {
                        try
                        {
                            Timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["ParsedTime"]);
                            string serializedFontSettingsEditor = ConfigurationManager.AppSettings["EditorFont"];
                            string serializedFontSettingsNode = ConfigurationManager.AppSettings["NodeFont"];

                            XmlSerializer deserializer = new XmlSerializer(typeof(FontSettings));
                            StringReader stringReader = new StringReader(serializedFontSettingsEditor);
                            FontSettings fontSettings = (FontSettings)deserializer.Deserialize(stringReader);
                            RichTextBox.Font = fontSettings.ToFont();

                            stringReader = new StringReader(serializedFontSettingsNode);
                            fontSettings = (FontSettings)deserializer.Deserialize(stringReader);
                            view.Font = fontSettings.ToFont();
                            stringReader.Dispose();

                            foreach (var Node in Colors)
                            {
                                string colorvalue = ConfigurationManager.AppSettings[Node.Key];
                                Colors[Node.Key] = Color.FromArgb(Int32.Parse(colorvalue));
                            }
                            config.Save(ConfigurationSaveMode.Modified);
                        }
                        catch (Exception ex) when (ex is ArgumentException || ex is FormatException)
                        {
                            ConfigurationFileMissing = true;
                            MessageBox.Show("Configuration file setting is corupted, running at default settings", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Configuration file is corupted, running at default settings", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ConfigurationFileMissing = true;
                    }
                }
                else
                {
                    MessageBox.Show("Configuration file not found, running at default settings", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ConfigurationFileMissing = true;
                }
                if (ConfigurationFileMissing)
                {
                    config.Save();
                    ResetFontTimerColor();
                }
            }

            public void ResetFontTimerColor()
            {
                ResetColors();
                RichTextBox.Font = new Font("Arial", 12, FontStyle.Regular);
                view.Font = new Font("Arial", 12, FontStyle.Regular);
                Timer.Interval = 6000;

                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sb.AppendLine("<configuration>");
                sb.AppendLine("</configuration>");


                string loc = Assembly.GetEntryAssembly().Location;
                System.IO.File.WriteAllText(String.Concat(loc, ".config"), sb.ToString());

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                foreach (var color in Colors)
                {
                    config.AppSettings.Settings.Add(color.Key, color.Value.ToArgb().ToString());
                }

                XmlSerializer serializer = new XmlSerializer(typeof(FontSettings));
                StringWriter stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, new FontSettings(RichTextBox.Font.Name, RichTextBox.Font.Size, RichTextBox.Font.Style));
                string serializedFont = stringWriter.ToString();
                config.AppSettings.Settings.Add("NodeFont", serializedFont);
                config.AppSettings.Settings.Add("EditorFont", serializedFont);
                config.AppSettings.Settings.Add("ParsedTime", Timer.Interval.ToString());
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            public void ResetColors()
            {
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
                stack.Pop();
                ColorSelections(args.position, args.Name.Count(), "Element");
            }
            public void TextHandler(object sender, XmlTextEventArgs args)
            {
                var node = stack.Peek().Nodes.Add(args.Text);
                node.ForeColor = Colors["Text"];
                node.Tag = new XmlElement(args.Text) { Position = args.Position };
                node.ImageIndex = 2;
                ColorSelections(args.Position, args.Text.Count(), "Text");
            }
            public void AtributeHandler(object sender, XmlAtributeEventArgs args)
            {
                var node = stack.Peek().Nodes.Add(args.Name + "=\"" + args.Value + "\"");
                node.ForeColor = Colors["Atribute"];
                node.Tag = new XmlAttribute(args.Name, args.Value, args.Position);
                node.ImageIndex = 4;
                ColorSelections(args.Position, args.Name.Count() + args.Value.Count() + 3, "Atribute", args.Name.Count());
            }
            public void CommentHandler(object sender, XmlCommentEventArgs args)
            {
                var node = stack.Peek().Nodes.Add(args.Comment);
                node.ForeColor = Colors["Comment"];
                node.Tag = new XmlComment(args.Comment) { Position = args.Position };
                node.ImageIndex = 2;
                ColorSelections(args.Position, args.Comment.Count(), "Comment");
            }
            public void CDATAHandler(object sender, XmlCDATAEventArgs args)
            {
                var node = stack.Peek().Nodes.Add(args.CDATA);
                node.ForeColor = Colors["CDATA"];
                node.ImageIndex = 3;
                node.Tag = new XmlCDATA(args.CDATA) { Position = args.Position };
                ColorSelections(args.Position, args.CDATA.Count(), "CDATA");
            }
            public void DeclarationHandler(object sender, XmlDeclarationEventArgs args)
            {
                TreeNode node = new TreeNode();
                if (stack.Count == 0)
                {
                    node = view.Nodes.Add(args.PositionPI.Name);
                }
                else
                {
                    node = stack.Peek().Nodes.Add(args.PositionPI.Name);
                }
                node.ImageIndex = 2;
                node.ForeColor = Colors["Key"];
                node.Tag = new XmlDeclaration(args.PositionPI.Name) { Position = args.PositionPI.Position };
                var ln = args.PositionPI.Position.LineNumber - 1;
                var lp = args.PositionPI.Position.LinePosition - 1;
                var i = RichTextBox.GetFirstCharIndexFromLine(ln);
                i = i + lp;
                ColorSelections(args.PositionPI.Position, args.PositionPI.Name.Count(), "Element");

                RichTextBox.Select(i - 2, 2);
                RichTextBox.SelectionColor = Colors["Key"];
                RichTextBox.Select(i + args.PositionPI.Name.Length + 1, 2);
                RichTextBox.SelectionColor = Colors["Key"];
                RichTextBox.DeselectAll();

                foreach (var attribute in args.PositionPI.Positions)
                {
                    ColorSelections(attribute.Position, attribute.Name.Count() + attribute.Value.Count() + 3, "Atribute", attribute.Name.Count());
                }

            }

        }
        private async void OnTimedEvent(Object myObject, EventArgs myEventArgs)
        {
            aTimer.Stop();
            var selectionStart = this.richTextBox.SelectionStart;
            var selectionLength = richTextBox.SelectionLength;
            LockWindowUpdate((long)richTextBox.Handle);
            await DelegateMethod();
            richTextBox.SelectionStart = selectionStart;
            richTextBox.SelectionLength = selectionLength;
            LockWindowUpdate(0);
            richTextBox.Invalidate();
        }
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
            catch (XmlException ex)
            {
                statusStrip.Items[0].Text = ex.Message;

            }
            finally
            {
                updating = false;
                treehandler.stack.Clear();
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                if (richTextBox.Text != String.Empty)
                {
                    saveToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                }
                else
                {
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                }
                FileIsChanged = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                aTimer.Stop();
                aTimer.Start();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileIsChanged)
            {
                var res = MessageBox.Show(this, $"Do you want to save changes to ''{SelectedFileName}?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(this, new EventArgs());
                }
            }
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
                findTextDlg.resetFindWord();

                SelectedFileName = Path.GetFileName(openFileDialog.FileName);
                this.Text = AppName + "- " + SelectedFileName;

                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
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
            if (FileIsChanged)
            {
                var res = MessageBox.Show(this, $"Do you want to save changes to ''{SelectedFileName}?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(this, new EventArgs());
                }
            }

            Clear();
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            documentPath = String.Empty;
            findTextDlg.resetFindWord();
            SelectedFileName = "NewDocument";
            this.Text = AppName + "- " + SelectedFileName;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = SelectedFileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                writer.Write(richTextBox.Text);

                documentPath = saveFileDialog.FileName;
                MessageBox.Show("File saved", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FileIsChanged = false;
                SelectedFileName = Path.GetFileName(saveFileDialog.FileName);
                this.Text = AppName + "- " + SelectedFileName;
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
                var res = MessageBox.Show(this, $"Do you want to save changes to ''{SelectedFileName}?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Yes)
                {
                    e.Cancel = true;
                    saveToolStripMenuItem_Click(this, new EventArgs());
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
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    foreach (var color in result)
                    {
                        config.AppSettings.Settings[color.Key].Value = color.Value.ToArgb().ToString();
                    }
                    config.Save(ConfigurationSaveMode.Modified, true);
                    Reload();
                }
            }


        }
        private void Clear()
        {
            updating = true;
            richTextBox.Clear();
            treeView.Nodes.Clear();
            updating = false;
        }
        private async void Reload()
        {
            await DelegateMethod();
        }
        private void editorTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (FontDialog font = new FontDialog())
            {
                font.ShowEffects = false;
                font.ShowColor = false;
                font.AllowScriptChange = false;
                if (font.ShowDialog() == DialogResult.OK)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    XmlSerializer serializer = new XmlSerializer(typeof(FontSettings));
                    StringWriter stringWriter = new StringWriter();
                    serializer.Serialize(stringWriter, new FontSettings(font.Font.Name, font.Font.Size, font.Font.Style));
                    string serializedFont = stringWriter.ToString();
                    config.AppSettings.Settings["EditorFont"].Value = serializedFont;
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
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
                font.ShowEffects = false;
                font.ShowColor = false;
                font.AllowScriptChange = false;
                if (font.ShowDialog() == DialogResult.OK)
                {
                    LockWindowUpdate((long)richTextBox.Handle);
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    XmlSerializer serializer = new XmlSerializer(typeof(FontSettings));
                    StringWriter stringWriter = new StringWriter();
                    serializer.Serialize(stringWriter, new FontSettings(font.Font.Name, font.Font.Size, font.Font.Style));
                    string serializedFont = stringWriter.ToString();
                    config.AppSettings.Settings["NodeFont"].Value = serializedFont;
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    treeView.Font = font.Font;
                    Reload();
                    LockWindowUpdate(0);
                    richTextBox.Invalidate();
                }
            }

        }

        private void searchWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            findTextDlg.Hide();
            findTextDlg.Show(this);
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                aTimer.Stop();
                Reload();
            }
        }
        [Serializable]
        public class FontSettings
        {
            public string FontFamily { get; set; }
            public float Size { get; set; }
            public FontStyle Style { get; set; }

            public FontSettings()
            {
            }

            public FontSettings(string fontFamily, float size, FontStyle style)
            {
                FontFamily = fontFamily;
                Size = size;
                Style = style;
            }

            public Font ToFont()
            {
                return new Font(FontFamily, Size, Style);
            }
        }

        private void processingTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (SetParsingTime dialog = new SetParsingTime(aTimer.Interval))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var time = dialog.time;
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["ParseTime"].Value = time.ToString();
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    aTimer.Interval = time;
                }
            }

        }
    }
}