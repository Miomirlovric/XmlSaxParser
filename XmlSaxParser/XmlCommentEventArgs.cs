using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlCommentEventArgs : EventArgs
    {
        public XmlCommentEventArgs(string comment)
        {
            Comment = comment;
        }

        public readonly string Comment;
    }
}
