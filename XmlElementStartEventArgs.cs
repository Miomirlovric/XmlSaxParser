using System;

namespace XmlSaxParser
{
    public class XmlElementStartEventArgs : EventArgs
    {
        public XmlElementStartEventArgs(string name)
        {
            Name = name;
        }

        public readonly string Name;
    }
}
