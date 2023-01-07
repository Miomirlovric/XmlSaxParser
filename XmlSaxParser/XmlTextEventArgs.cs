using System;

namespace XmlSaxParser
{
    public class XmlTextEventArgs : EventArgs
    {
        public XmlTextEventArgs(string text)
        {
            Text = text;
        }

        public readonly string Text;
    }
}
