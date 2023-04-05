using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlAtributeEventArgs : EventArgs
    {
        public XmlAtributeEventArgs(string name, string value, Position position)
        {
            Name = name;
            Value = value;
            Position = position;
        }

        public readonly string Name;
        public readonly string Value;
        public readonly Position Position;

    }
}
