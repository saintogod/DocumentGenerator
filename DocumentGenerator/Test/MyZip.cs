using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Test
{
    public class MyZip//: IXmlSerializable
    {
        public string InputDoc { get; set; }
        public string Content { get; set; }

        public static void Main0(string[] args)
        {
            string outputDoc = @"E:\DocumentGenerator\ContentTemplate1.docx";
            string inputDoc = @"E:\DocumentGenerator\ContentTemplate.docx";
            string tempPath = @"E:\DocumentGenerator\temp\extract";

            //ZipFile.CreateFromDirectory(inputDoc, tempPath);
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
            ZipFile.ExtractToDirectory(inputDoc, tempPath);
            ZipFile.CreateFromDirectory(tempPath, outputDoc);
        }

        public static void Main1(string[] args)
        {
            MyZip mz = new MyZip();
            XmlSerializer zs = new XmlSerializer(typeof(MyZip));
            FileStream fs = new FileStream(@"E:\DocumentGenerator\temp\Template.xml", FileMode.OpenOrCreate, FileAccess.Write);
            zs.Serialize(fs, mz);
            fs.Close();
            fs = new FileStream(@"E:\DocumentGenerator\temp\Template.xml", FileMode.Open, FileAccess.Read);

            zs.Deserialize(fs);
            fs.Close();
        }
        public static void Main2(string[] args)
        {
            MyZip mz = new MyZip();
            mz.InputDoc = @"E:\DocumentGenerator\ContentTemplate.docx";
            using (FileStream doc = new FileStream(mz.InputDoc, FileMode.Open, FileAccess.Read))
            {
                Byte[] binaryData = new Byte[doc.Length];
                long bytesRead = doc.Read(binaryData, 0, (int)doc.Length);
                mz.Content = Convert.ToBase64String(binaryData);
            }

            XmlSerializer zs = new XmlSerializer(typeof(MyZip));
            FileStream fs = new FileStream(@"E:\DocumentGenerator\temp\Template.xml", FileMode.OpenOrCreate, FileAccess.Write);
            zs.Serialize(fs, mz);
            fs.Close();
            fs = new FileStream(@"E:\DocumentGenerator\temp\Template.xml", FileMode.Open, FileAccess.Read);
            MyZip ret = new MyZip();
            ret = (MyZip)zs.Deserialize(fs);
            fs.Close();

            using (FileStream doc = new FileStream(@"E:\DocumentGenerator\ContentTemplate2.docx", FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] binaryData = Convert.FromBase64String(mz.Content);
                doc.Write(binaryData, 0, binaryData.Length);
            }
        }
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();
            Console.WriteLine("path = " + reader.GetAttribute("Path"));
            reader.ReadStartElement();
            string content = reader.ReadElementString("Content");
            reader.ReadEndElement();

            Byte[] binaryData = Convert.FromBase64String(content);
            FileStream fs = new FileStream(@"E:\DocumentGenerator\Template3.docx", FileMode.Create, FileAccess.Write);
            fs.Write(binaryData, 0, binaryData.Length);

        }

        public void WriteXml(XmlWriter writer)
        {
            //writer.WriteStartDocument();
            writer.WriteStartElement("File");
            writer.WriteAttributeString("Path", InputDoc);
            FileStream fs = new FileStream(InputDoc, FileMode.Open, FileAccess.Read);
            Byte[] binaryData;
            binaryData = new Byte[fs.Length];
            long bytesRead = fs.Read(binaryData, 0, (int)fs.Length);
            fs.Close();
            string base64 = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

            writer.WriteElementString("Content", base64);
            writer.WriteEndElement();
            //writer.WriteEndDocument();
        }


        public static void Main3(string[] args)
        {
            string path = @"C:\abc.text";
            Console.WriteLine(path.Substring(0, path.LastIndexOf('.')));
            Console.ReadKey();
        }
    }
}