using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class PositionPI
    {
        public PositionPI(string name, string value, Position position)
        {
            Name = name;
            Value = value;
            Position = position;
        }

        public string Name { get; set; }    
        public string Value { get; set; }

        public Position Position { get; set; }
        
        public List<PositionPI> Positions { get; set; } = new List<PositionPI>();   
    }
}
