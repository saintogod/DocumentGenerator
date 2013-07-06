using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
    [XmlRoot("TextInputField")]
    public class TextInputField : Field
    {
        public TextInputField():base()
        { }
        public TextInputField(TextInputField tif)
            : base(tif)
        { }
        public TextInputField(string title, double width, double height, double left, double top, 
            string bgColor = null, string layout = "LeftRight", 
            string description = "", string defaultValue="")
            : base(title,  width, height, left, top, bgColor, layout, description, defaultValue)
        { }
    }
}
