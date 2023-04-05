using System.Collections.Generic;
using System.Xml;

namespace XmlSaxParser
{
    public class XmlElement : XmlNode
    {
        public XmlElement(string name) : base(NodeType.Element)
        {
            Name = name;
        }

        public void AddAttribute(string name, string value,Position position)
        {
            AttributesList.Add(new XmlAttribute(name, value, position));
        }

        public override void AddChild(XmlNode child)
        {
            children.Add(child);
        }

        public string Name { get; private set; }

        public readonly List<XmlAttribute> AttributesList = new List<XmlAttribute>();
    }
}
