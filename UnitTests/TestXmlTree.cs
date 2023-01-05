using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using XmlSaxParser;

namespace UnitTests
{
    [TestClass]
    public class TestXmlTree
    {
        SaxParser parser = new SaxParser();
        XmlTree tree = null;

        // This method is performed before each test so there is no need to rewrite these statements for each test method.
        [TestInitialize]
        public void Initialize()
        {
            tree = new XmlTree();
            parser.XmlElementStart += tree.ElementStartHandler;
            parser.XmlElementEnd += tree.ElementEndHandler;
            parser.XmlText += tree.TextHandler;
            parser.XmlAtribute += tree.AtributeHandler;
            parser.XmlComment += tree.CommentHandler;
            parser.XmlCDATA += tree.CDATAHandler;
        }

        [TestMethod]
        public void XmlTreeContainsNoElementForEmptyXml()
        {
            using (TextReader reader = new StringReader(""))
            {
                var result = parser.Parse(reader);
            }

            Assert.IsNull(tree.Root);
        }

        [TestMethod]
        public void XmlTreeContainsRootElementOnlyForAnXmlWithSingleElement()
        {
            using (TextReader reader = new StringReader("<document/>"))
            {
                var result = parser.Parse(reader);
            }

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("document", tree.Root.Name);
        }

        [TestMethod]
        public void XmlTreeContainsRootAndTwoSiblingChildren()
        {
            using (TextReader reader = new StringReader("<document><child1></child1><child2></child2></document>"))
            {
                var result = parser.Parse(reader);
            }

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("document", tree.Root.Name);

            Assert.AreEqual(2, tree.Root.Children.Count());
            var child1 = tree.Root.Children.ElementAt(0);
            Assert.AreEqual(NodeType.Element, child1.NodeType);
            Assert.AreEqual("child1", ((XmlElement)tree.Root.Children.ElementAt(0)).Name);

            var child2 = tree.Root.Children.ElementAt(1);
            Assert.AreEqual(NodeType.Element, child2.NodeType);
            Assert.AreEqual("child2", ((XmlElement)tree.Root.Children.ElementAt(1)).Name);
        }

        [TestMethod]
        public void XmlTreeContainsRootWithChildAndGrandChild()
        {
            using (TextReader reader = new StringReader("<document><child><grandchild/></child></document>"))
            {
                var result = parser.Parse(reader);
            }

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("document", tree.Root.Name);

            Assert.AreEqual(1, tree.Root.Children.Count());
            var child = tree.Root.Children.ElementAt(0);
            Assert.AreEqual(NodeType.Element, child.NodeType);
            Assert.AreEqual("child", ((XmlElement)child).Name);

            Assert.AreEqual(1, child.Children.Count());
            var grandchild = child.Children.ElementAt(0);
            Assert.AreEqual(NodeType.Element, grandchild.NodeType);
            Assert.AreEqual("grandchild", ((XmlElement)child.Children.ElementAt(0)).Name);
        }

        [TestMethod]
        public void XmlTreeContainsSingleTextNode()
        {
            using (TextReader reader = new StringReader("<document>text</document>"))
            {
                var result = parser.Parse(reader);
            }

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("document", tree.Root.Name);

            Assert.AreEqual(1, tree.Root.Children.Count());
            var child = tree.Root.Children.ElementAt(0);
            Assert.AreEqual(NodeType.Text, child.NodeType);
            Assert.AreEqual("text", ((XmlText)child).Text);
        }

        [TestMethod]
        public void XmlTreeRootContainsMultipleTextAndElementNodes()
        {
            using (TextReader reader = new StringReader("<document>text0<child>text1</child>text2</document>"))
            {
                var result = parser.Parse(reader);
            }

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("document", tree.Root.Name);

            Assert.AreEqual(3, tree.Root.Children.Count());

            var child0 = tree.Root.Children.ElementAt(0);
            Assert.AreEqual(NodeType.Text, child0.NodeType);
            Assert.AreEqual("text0", ((XmlText)child0).Text);

            var child1 = tree.Root.Children.ElementAt(1);
            Assert.AreEqual(NodeType.Element, child1.NodeType);
            Assert.AreEqual("child", ((XmlElement)child1).Name);

            Assert.AreEqual(1, child1.Children.Count());
            var child2 = child1.Children.ElementAt(0);
            Assert.AreEqual(NodeType.Text, child2.NodeType);
            Assert.AreEqual("text1", ((XmlText)child2).Text);

            var child3 = tree.Root.Children.ElementAt(2);
            Assert.AreEqual(NodeType.Text, child3.NodeType);
            Assert.AreEqual("text2", ((XmlText)child3).Text);
        }
        [TestMethod]
        public void NodeHasOneAtribute()
        {
            using (TextReader reader = new StringReader("<document gender=\"male\"></document>"))
            {
                var result = parser.Parse(reader);

            }

            var child0 = tree.Root;
            Assert.AreEqual(NodeType.Element, child0.NodeType);
            Assert.AreEqual(true, ((XmlElement)child0).Attributes.ContainsKey("gender"));

        }
        [TestMethod]
        public void NodeHasMultipleAtribute()
        {
            using (TextReader reader = new StringReader("<document gender=\"male\"><child gender=\"female\"></child></document>"))
            {
                var result = parser.Parse(reader);

            }

            var child0 = tree.Root;
            var child1 = tree.Root.Children.ElementAt(0);
            var child3 = tree.Root.Children.GetEnumerator();
            child3.MoveNext();
            Assert.AreEqual(NodeType.Element, child0.NodeType);
            Assert.AreEqual(true, ((XmlElement)child0).Attributes.ContainsKey("gender"));
            Assert.AreEqual("male", ((XmlElement)child0).Attributes["gender"]);
            Assert.AreEqual(true, ((XmlElement)child1).Attributes.ContainsKey("gender"));
            Assert.AreEqual("female", ((XmlElement)child1).Attributes["gender"]);
        }
        [TestMethod]
        public void TreeHasComment()
        {
            using (TextReader reader = new StringReader("<document><child>text1</child><!--Students grades are uploaded by months--></document>"))
            {
                var result = parser.Parse(reader);

            }
            var child1 = tree.Root.Children.ElementAt(1);
            Assert.AreEqual(((XmlComment)child1).Comment, "Students grades are uploaded by months");

        }
        [TestMethod]
        public void TreeHasMultipleComment()
        {
            using (TextReader reader = new StringReader("<document><child>text1</child><!--Students grades are uploaded by months--><!--gaga--></document>"))
            {
                var result = parser.Parse(reader);

            }
            var child1 = tree.Root.Children.ElementAt(1);
            var child2 = tree.Root.Children.ElementAt(2);
            Assert.AreEqual(((XmlComment)child1).Comment, "Students grades are uploaded by months");
            Assert.AreEqual(((XmlComment)child2).Comment, "gaga");

        }
        [TestMethod]
        public void TreeHasCDATA()
        {
            using (TextReader reader = new StringReader("<script>\r\n   <![CDATA[\r\n      <message> Welcome to TutorialsPoint </message>\r\n   ]] >\r\n</script >"))
            {
                var result = parser.Parse(reader);

            }
            var child1 = tree.Root.Children.ElementAt(0);
            Assert.AreEqual(((XmlCDATA)child1).CDATA, "<message> Welcome to TutorialsPoint </message>");

        }
    }
}
