using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XmlSaxParser
{
    public class SaxParser
    {
        public delegate void XmlElementStartEventHandler(object sender, XmlElementStartEventArgs args);
        public delegate void XmlElementEndEventHandler(object sender, XmlElementEndEventArgs args);
        public delegate void XmlTextEventHandler(object sender, XmlTextEventArgs args);
        public delegate void XmlAtributeEventHandler(object sender, XmlAtributeEventArgs args);
        public delegate void XmlCommentEventHandler(object sender, XmlCommentEventArgs args);
        public delegate void XmlCDATAEventHandler(object sender, XmlCDATAEventArgs args);

        public event XmlElementStartEventHandler XmlElementStart;
        public event XmlElementEndEventHandler XmlElementEnd;
        public event XmlTextEventHandler XmlText;
        public event XmlAtributeEventHandler XmlAtribute;
        public event XmlCommentEventHandler XmlComment;
        public event XmlCDATAEventHandler XmlCDATA;


        public SaxParser()
        {
            settings.Async = true;
        }

        XmlReaderSettings settings = new XmlReaderSettings();

        public async Task Parse(TextReader textReader)
        {
            using (XmlReader reader = XmlReader.Create(textReader, settings))
            {
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            OnElementStart(reader.Name);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    OnAtribute(reader.Name, reader.Value);
                                }
                                // Move the reader back to the element node.
                                reader.MoveToElement();
                            }
                            break;
                        case XmlNodeType.EndElement:
                            OnElementEnd(reader.Name);
                            break;
                        case XmlNodeType.Text:
                            OnText(await reader.GetValueAsync());
                            break;
                        case XmlNodeType.Comment:
                            OnComment(await reader.GetValueAsync());

                            break;
                        case XmlNodeType.CDATA:
                            OnCDATA(reader.Value);

                            break;

                        default:

                            break;
                    }
                }
            }
        }

        protected virtual void OnElementStart(string name)
        {
            XmlElementStart?.Invoke(this, new XmlElementStartEventArgs(name));
        }
        protected virtual void OnElementEnd(string name)
        {
            XmlElementEnd?.Invoke(this, new XmlElementEndEventArgs(name));
        }
        protected virtual void OnText(string text)
        {
            XmlText?.Invoke(this, new XmlTextEventArgs(text));
        }
        protected virtual void OnAtribute(string atrText, string atrValue)
        {
            XmlAtribute?.Invoke(this, new XmlAtributeEventArgs(atrText, atrValue));
        }
        protected virtual void OnComment(string comment)
        {
            XmlComment?.Invoke(this, new XmlCommentEventArgs(comment));
        }
        protected virtual void OnCDATA(string comment)
        {
            XmlCDATA?.Invoke(this, new XmlCDATAEventArgs(comment));
        }
    }
}
