using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlAtributeEventArgs
    {
        public XmlAtributeEventArgs(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name;
        public string Value;

    }
}
