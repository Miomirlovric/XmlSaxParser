using System;

namespace XmlSaxParser
{
    public class XmlTextEventArgs : EventArgs
    {
        public XmlTextEventArgs(string text, Position position)
        {
            Text = text;
            Position = position;
        }

        public readonly string Text;
        public readonly Position Position;
    }
}
