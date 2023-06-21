using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlDeclaration : XmlNode
    {
        public XmlDeclaration(string declaration) : base(NodeType.Element)
        {
            Declaration = declaration;  
        }
        public override void AddChild(XmlNode child)
        {
            throw new Exception("XmlDeclaration cannot be nested");
        }

        public string Declaration { get; private set; }
    }
}
