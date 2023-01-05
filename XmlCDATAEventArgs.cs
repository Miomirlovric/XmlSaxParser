﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSaxParser
{
    public class XmlCDATAEventArgs : EventArgs
    {
        public XmlCDATAEventArgs(string cDATA)
        {
            CDATA = cDATA;
        }

        public readonly string CDATA;
    }
}
