using System;

namespace XmlSaxParser
{
    public class XmlElementEndEventArgs : EventArgs
    {
        public XmlElementEndEventArgs(string name)
        {
            Name = name;
        }

        public readonly string Name;
    }
}
