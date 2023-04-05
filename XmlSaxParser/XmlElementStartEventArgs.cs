using System;

namespace XmlSaxParser
{
    public class XmlElementStartEventArgs : EventArgs
    {
        public XmlElementStartEventArgs(string name, Position position)
        {
            Name = name;
            Position = position;
        }
        
        public readonly string Name;
        public readonly Position Position;
    }
}
