using System;

namespace XmlSaxParser
{
    public class XmlElementEndEventArgs : EventArgs
    {
        public XmlElementEndEventArgs(string name,Position position)
        {
            Name = name;
            this.position = position;
        }

        public readonly string Name;
        public readonly Position position;
    }
}
