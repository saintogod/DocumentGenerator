using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Test
{
    [Serializable]
    public class Logs<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable where TKey:class where TValue:class
    {
        public Logs()
        { }

        public Logs(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        public Logs(IEqualityComparer<TKey> comparer)
            : base(comparer)
        { }

        public Logs(int capacity)
            : base(capacity)
        { }

        public Logs(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        { }

        protected Logs(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #region IXmlSerializable Members
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// 从对象的 XML 表示形式生成该对象
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));
            bool wasEmpty = reader.IsEmptyElement;
            if (wasEmpty)
                return;

            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //reader.MoveToAttribute("name");
                //var key = reader.Value as TKey;
                var key = reader.GetAttribute("name") as TKey;
                reader.ReadStartElement("logitem");
                reader.ReadStartElement("path");
                var value = reader.ReadInnerXml() as TValue;

                Add(key, value); 
                reader.ReadEndElement();
                
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// 将对象转换为其 XML 表示形式
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (TKey key in Keys)
            {
                writer.WriteStartElement("logitem");
                writer.WriteAttributeString("name", key as string);
               
                TValue value = this[key];
                writer.WriteElementString("path", value as string);
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}