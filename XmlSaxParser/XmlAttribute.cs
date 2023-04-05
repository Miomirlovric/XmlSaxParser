using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlAttribute : XmlNode
    {
        public override void AddChild(XmlNode child)
        {
            throw new Exception("Atributes cannot be nested");
        }

        public XmlAttribute(string key, string value, Position position) : base(NodeType.Atribute)
        {
            Position = position;
            Key = key;
            Value = value;  
        }
        public readonly string Key;
        public readonly string Value;
    }
}
