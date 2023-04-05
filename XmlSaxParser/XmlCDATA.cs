using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlSaxParser
{
    public class XmlCDATA : XmlNode
    {
        public XmlCDATA(string CDATA) : base(NodeType.CDATA)
        {
            this.CDATA = CDATA;
        }
        public override void AddChild(XmlNode child)
        {
            throw new Exception("CDATA cannot be nested");
        }
        public string CDATA { get; set; }
    }
}
