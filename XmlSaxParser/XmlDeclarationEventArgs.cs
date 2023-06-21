using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlDeclarationEventArgs : EventArgs
    {
        public XmlDeclarationEventArgs(string xmldeclaration,Position position)
        {
            XmlDeclaration = xmldeclaration;
            Position = position;
        }
        public readonly string XmlDeclaration;
        public readonly Position Position;
    }
}
