using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class PositionEventArgs
    {
        public PositionEventArgs(int LineNumber, int LinePosition)
        {
            this.LineNumber = LineNumber;
            this.LinePosition = LinePosition;
        }

        public int LineNumber;
        public int LinePosition;
    }
}
