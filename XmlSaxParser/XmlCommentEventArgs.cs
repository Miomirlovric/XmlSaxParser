using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlCommentEventArgs : EventArgs
    {
        public XmlCommentEventArgs(string comment, Position position)
        {
            Comment = comment;
            Position = position;
        }

        public readonly string Comment;
        public readonly Position Position;
    }
}
