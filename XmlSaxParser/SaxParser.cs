using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using System.Runtime.CompilerServices;

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
        public delegate void XmlDeclarationEventHandler(object sender, XmlDeclarationEventArgs args);
        public delegate void XmlProcesingEventHandler(object sender, XmlDeclarationEventArgs args);

        public event XmlElementStartEventHandler XmlElementStart;
        public event XmlElementEndEventHandler XmlElementEnd;
        public event XmlTextEventHandler XmlText;
        public event XmlAtributeEventHandler XmlAtribute;
        public event XmlCommentEventHandler XmlComment;
        public event XmlCDATAEventHandler XmlCDATA;
        public event XmlDeclarationEventHandler XmlDeclaration;
        public event XmlProcesingEventHandler XmlProcesing;

        public SaxParser()
        {
            settings.Async = true;
        }

        readonly XmlReaderSettings settings = new XmlReaderSettings();

        public async Task Parse(TextReader textReader)
        {
            using (XmlReader reader = XmlReader.Create(textReader, settings))
            {
                IXmlLineInfo xli = (IXmlLineInfo)reader;               
                while (await reader.ReadAsync())
                {
                    var Position = new Position { LineNumber = xli.LineNumber, LinePosition = xli.LinePosition };
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            OnElementStart(reader.Name, Position);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    OnAtribute(reader.Name, reader.Value, new Position() { LineNumber = xli.LineNumber, LinePosition=xli.LinePosition });
                                }
                                // Move the reader back to the element node.
                                reader.MoveToElement();
                            }
                            if (reader.IsEmptyElement)
                            {
                                OnElementEnd(reader.Name,Position);
                            }
                            break;
                        case XmlNodeType.EndElement:
                            OnElementEnd(reader.Name,Position);
                            break;
                        case XmlNodeType.Text:
                            OnText(await reader.GetValueAsync(), Position);
                            break;
                        case XmlNodeType.Comment:
                            OnComment(await reader.GetValueAsync(), Position);

                            break;
                        case XmlNodeType.CDATA:
                            OnCDATA(reader.Value, Position);

                            break;
                        case XmlNodeType.XmlDeclaration:

                            PositionPI positionPI = new PositionPI(reader.Name+reader.Value, "null", Position);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    positionPI.Positions.Add(new PositionPI(reader.Name, reader.Value, new Position() { LineNumber = xli.LineNumber, LinePosition = xli.LinePosition }));                                    
                                }
                                OnDeclaration(positionPI);
                                reader.MoveToElement();
                            }
                            else
                            {
                                OnDeclaration(positionPI);
                            }
                                                   
                            break;
                        case XmlNodeType.ProcessingInstruction:

                            PositionPI pi = new PositionPI(reader.Name + reader.Value, "null", Position);
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    pi.Positions.Add(new PositionPI(reader.Name, reader.Value, new Position() { LineNumber = xli.LineNumber, LinePosition = xli.LinePosition }));
                                }
                                OnProcesing(pi);
                                reader.MoveToElement();
                            }
                            else
                            {
                                OnProcesing(pi);
                            }

                            break;


                        default:
                            
                            break;
                    }
                }
            }
        }

        protected virtual void OnElementStart(string name, Position position)
        {
            XmlElementStart?.Invoke(this, new XmlElementStartEventArgs(name, position));
        }
        protected virtual void OnElementEnd(string name, Position position)
        {
            XmlElementEnd?.Invoke(this, new XmlElementEndEventArgs(name, position));
        }
        protected virtual void OnText(string text, Position position)
        {
            XmlText?.Invoke(this, new XmlTextEventArgs(text, position));
        }
        protected virtual void OnAtribute(string atrText, string atrValue, Position position)
        {
            XmlAtribute?.Invoke(this, new XmlAtributeEventArgs(atrText, atrValue, position));
        }
        protected virtual void OnComment(string comment, Position position)
        {
            XmlComment?.Invoke(this, new XmlCommentEventArgs(comment, position));
        }
        protected virtual void OnCDATA(string comment, Position position)
        {
            XmlCDATA?.Invoke(this, new XmlCDATAEventArgs(comment, position));
        }
        protected virtual void OnDeclaration(PositionPI position)
        {
            XmlDeclaration?.Invoke(this, new XmlDeclarationEventArgs(position));
        }
        protected virtual void OnProcesing(PositionPI position)
        {
            XmlProcesing?.Invoke(this, new XmlDeclarationEventArgs(position));
        }
    }
}
