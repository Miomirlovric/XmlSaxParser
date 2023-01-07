using System.Collections.Generic;

namespace XmlSaxParser
{
    public enum NodeType
    {
        Element,
        Text,
        Comment,
        CDATA
    }

    public abstract class XmlNode
    {
        public readonly NodeType NodeType;

        public XmlNode(NodeType nodeType)
        {
            NodeType = nodeType;
        }

        public abstract void AddChild(XmlNode child);

        public IEnumerable<XmlNode> Children => children;

        protected List<XmlNode> children = new List<XmlNode>();
    }
}
