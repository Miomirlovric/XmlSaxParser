using System;
using System.Collections.Generic;

namespace XmlSaxParser
{
    public class XmlTree
    {
        public XmlNode Root => root;

        private XmlNode root = null;

        // Stack holds current hierarchy of elements. Last element corresponds to the active element and this stack is used to identify parent for a new element.
        private Stack<XmlElement> elementStack = new Stack<XmlElement>();

        public void ElementStartHandler(object sender, XmlElementStartEventArgs args)
        {
            var newElement = new XmlElement(args.Name) { Position = args.Position};
            // If element stack is empty, this element is the root.
            if (elementStack.Count == 0)
            {
                root = newElement;
            }
            // If new element is not the root element then add it as a child of current active element, which is the top element on the stack.
            else
            {
                elementStack.Peek().AddChild(newElement);
            }
            // Push element to the element stack.
            elementStack.Push(newElement);
        }

        public void ElementEndHandler(object sender, XmlElementEndEventArgs args)
        {
            if (elementStack.Count == 0)
            {
                throw new Exception("Invalid XML document");
            }
            var elementRemoved = elementStack.Pop();
            if (elementRemoved.Name != args.Name)
            {
                throw new Exception("Closing tag name is different from opening tag name");
            }
        }

        public void TextHandler(object sender, XmlTextEventArgs args)
        {
            if (elementStack.Count == 0)
            {
                throw new Exception("XmlText node must have parent XmlElement node");
            }
            // Add XmlText node to parent XmlElement.
            elementStack.Peek().AddChild(new XmlText(args.Text) { Position = args.Position});
        }

        public void AtributeHandler(object sender, XmlAtributeEventArgs args)
        {
            if (elementStack.Count == 0)
            {
                throw new Exception("XmlAtribute must be inside opening tag");
            }
            // Add atribute name and value to the XmlElement.
            elementStack.Peek().AddAttribute(args.Name, args.Value,args.Position);
        }
        public void CommentHandler(object sender, XmlCommentEventArgs args)
        {
            if (elementStack.Count == 0)
            {
                throw new Exception("Comment needs to be after first tag");
            }
            else { elementStack.Peek().AddChild(new XmlComment(args.Comment) { Position=args.Position}); }
            // Add atribute name and value to the XmlElement.
        }
        public void CDATAHandler(object sender, XmlCDATAEventArgs args)
        {
            if (elementStack.Count == 0)
            {
                throw new Exception("CDATA needs to be after first tag");
            }
            elementStack.Peek().AddChild(new XmlCDATA(args.CDATA) {Position = args.Position });
            // Add atribute name and value to the XmlElement.
        }
    }
}
