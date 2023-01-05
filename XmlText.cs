using System;

namespace XmlSaxParser
{
    public class XmlText : XmlNode
    {
        public XmlText(string text) : base(NodeType.Text)
        {
            Text = text;
        }

        public override void AddChild(XmlNode child)
        {
            throw new Exception("Text node cannot have children");
        }

        public string Text { get; private set; }
    }
}
