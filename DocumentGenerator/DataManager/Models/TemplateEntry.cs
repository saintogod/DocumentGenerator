using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DataManager.Models
{
    [Serializable]
    public class TemplateEntry : Dictionary<string, string>, IXmlSerializable
    {
        public TemplateEntry()
        { }

        public TemplateEntry(IDictionary<string, string> dictionary)
            : base(dictionary)
        { }

        public TemplateEntry(IEqualityComparer<string> comparer)
            : base(comparer)
        { }

        public TemplateEntry(int capacity)
            : base(capacity)
        { }

        public TemplateEntry(int capacity, IEqualityComparer<string> comparer)
            : base(capacity, comparer)
        { }

        protected TemplateEntry(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
        public void Add(Dictionary<string, string> data)
        {
            foreach (var item in data)
            {
                Add(item.Key, item.Value);
            }
        }
        #region IXmlSerializable Members
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;
            if (wasEmpty)
                return;

            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var key = reader.GetAttribute("filename"); 
                reader.Read();
                reader.ReadStartElement("content");
                var value = reader.Value;
                reader.Read();
                reader.ReadEndElement();
                Add(key, value);
                reader.Read();
                reader.MoveToContent();
            }
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (string key in Keys)
            {
                writer.WriteStartElement("template");
                writer.WriteAttributeString("filename", key);
                writer.WriteStartElement("content");
                string value = this[key];
                writer.WriteValue(value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}