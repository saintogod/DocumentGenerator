using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataManager.Models
{
    [XmlInclude(typeof(Field))]
    [XmlInclude(typeof(TextInputField))]
    [XmlRoot("TemplateXml")]
    public class TemplateXml
    {
        [XmlElement("Version")]
        public int Version { get; set; }
        [XmlElement("Wapper")]
        public string Wapper { get; set; }
        [XmlElement("Tempaltes")]
        public TemplateEntry TemplateEntries { get; set; }

        [XmlArray("Fields")]
        public List<Field> Fields { get; set; }

        public TemplateXml() {
            this.Version = 0;
            Fields = new List<Field>();
            TemplateEntries = new TemplateEntry();
        }

    }
}
