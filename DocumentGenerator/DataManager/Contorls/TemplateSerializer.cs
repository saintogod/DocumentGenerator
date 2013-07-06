using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

using DataManager.Models;
using System.Text.RegularExpressions;

namespace DataManager.Contorls
{
    public class TemplateSerializer
    {
        public static bool Serialize(TemplateXml template, string outputFilePath)
        {
            
            try
            {
                if (template == null || outputFilePath == null)
                    return false;
                if(!Regex.IsMatch(outputFilePath,@"^\w:(\\.*)+\.xml$"))
                    return false;
                if (!Directory.GetParent(outputFilePath).Exists)
                {
                    Directory.CreateDirectory(Directory.GetParent(outputFilePath).ToString());
                }
                XmlSerializer ser = new XmlSerializer(typeof(TemplateXml));
                using (TextWriter tw = new StreamWriter(outputFilePath))
                {
                    ser.Serialize(tw,template);
                    return true;
                }
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        public static bool Deserialize(out TemplateXml template,string filePath)
        {
            
            try
            {
                if (!File.Exists(filePath))
                {
                    template = null;
                    return false;
                }
                XmlSerializer ser = new XmlSerializer(typeof(TemplateXml));
                using (TextReader tr = new StreamReader(filePath))
                {
                    template = (TemplateXml)ser.Deserialize(tr);
                    return true;
                }
            }
            catch
            {
                template = null;
                return false;
            }
        }
    }
}
