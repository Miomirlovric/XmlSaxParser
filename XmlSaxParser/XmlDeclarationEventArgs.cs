using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlDeclarationEventArgs : EventArgs
    {
        public XmlDeclarationEventArgs(PositionPI positionpi )
        {
            PositionPI = positionpi;
        }

        public readonly PositionPI PositionPI;
    }
}
