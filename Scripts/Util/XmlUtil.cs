using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Bale007.Util
{
    public static class XmlUtil
    {
        public static XmlNode GetNodeById(this XmlDocument xmlDocument, string id)
        {
            foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
                if (node.Name == id)
                    return node;
            return null;
        }

        public static XmlNode GetNodeById(this XmlNode parentnode, string id)
        {
            foreach (XmlNode node in parentnode.ChildNodes)
                if (node.Name == id)
                    return node;
            return null;
        }

        public static string GetNodeTextById(this XmlDocument xmlDocument, string id)
        {
            var node = GetNodeById(xmlDocument, id);
            if (node != null) return node.InnerText;
            return null;
        }

        public static string GetNodeTextById(this XmlNode parentnode, string id)
        {
            var node = GetNodeById(parentnode, id);
            if (node != null) return node.InnerText;
            return null;
        }

        public static XmlAttribute GetAttributeById(this XmlNode node, string id)
        {
            var attrs = node.Attributes;

            foreach (XmlAttribute attr in attrs)
                if (attr.Name == id)
                    return attr;
            return null;
        }

        public static string GetAttributeTextById(this XmlNode node, string id)
        {
            var attr = GetAttributeById(node, id);
            if (attr != null) return attr.InnerText;
            return null;
        }

        public static void IterateNodes(XmlDocument xmlDocument, Action<XmlNode> callback)
        {
            foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes) callback(node);
        }

        /// <summary>
        ///     Given an xml string and target element name return an enumerable for fast lightweight
        ///     forward reading through the xml document.
        ///     NOTE: This function uses an XmlReader to provide forward access to the xml document.
        ///     It is meant for serial single-pass looping over the element collection. Calls to functions
        ///     like ToList() will defeat the purpose of this function.
        /// </summary>
        public static IEnumerable<XElement> StreamElement(string xmlString, string elementName)
        {
            using (var reader = XmlReader.Create(new StringReader(xmlString)))
            {
                while (reader.Name == elementName || reader.ReadToFollowing(elementName))
                    yield return (XElement) XNode.ReadFrom(reader);
            }
        }
    }
}