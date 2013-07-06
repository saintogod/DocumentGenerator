using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
    [XmlRoot("TemplateXml")]
    public class TemplateXml//: IXmlSerializable
    {
        [XmlElement("Version")]
        public int Version { get; set; }
        [XmlElement("Wapper")]
        public string Wapper { get; set; }

        [XmlElement("TextInputField", typeof(TextInputField))]
        public List<Field> Fields { get; set; }

        public TemplateXml() {
            this.Version = 1;
            Fields = new List<Field>();
        }

        public static void Main1(string[] args)
        {
            TemplateXml tx = new TemplateXml(){
                Version =1,
                Wapper = "@",
                Fields = new List<Field>(){
                    new TextInputField(){Title = "saint1",Width=10.0,Height=11.0,Top=70.0,Left=20.0,Layout="LeftRight",BackgroundColor="red",DefaultValue="ting",Description="fuck"},
                    new TextInputField(){Title = "saint2",Width=10.0,Height=11.0,Top=70.0,Left=20.0,Layout="LeftRight",BackgroundColor="red",DefaultValue="ting",Description="fuck"},
                    new TextInputField(){Title = "saint3",Width=10.0,Height=11.0,Top=70.0,Left=20.0,Layout="LeftRight",BackgroundColor="red",DefaultValue="ting",Description="fuck"},
                    new TextInputField(){Title = "saint4",Width=10.0,Height=11.0,Top=70.0,Left=20.0,Layout="LeftRight",BackgroundColor="red",DefaultValue="ting",Description="fuck"}
                }};
            string path =@"E:\fields.xml";
            XmlSerializer xser = new XmlSerializer(typeof(TemplateXml));
            TextWriter tw = new StreamWriter(path);
            xser.Serialize(tw,tx);
            tw.Close();

            TextReader tr = new StreamReader(path);
            TemplateXml tx2= new TemplateXml();
            tx2 = (TemplateXml)xser.Deserialize(tr);

            Console.ReadKey();
            
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToElement();
            Version = reader.ReadElementContentAsInt();
            reader.MoveToElement();
            Wapper = reader.ReadElementContentAsString();
            reader.MoveToElement();

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
