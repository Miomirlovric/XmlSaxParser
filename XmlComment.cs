using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlSaxParser
{
    public class XmlComment : XmlNode
    {
        public XmlComment(string comment) : base(NodeType.Comment)
        {
            Comment = comment;
        }

        public override void AddChild(XmlNode child)
        {
            throw new Exception("Comments cannot be nested");
        }

        public string Comment { get; private set; }
    }
}
