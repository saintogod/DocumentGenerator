using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataManager.Models
{
    [XmlInclude(typeof(TextInputField))]
    [XmlRoot("TextInputField")]
    public class TextInputField : Field
    {
        public TextInputField():base()
        { }
        public TextInputField(TextInputField tif)
            : base(tif)
        { }
        public TextInputField(string title, double width, double height, double left, double top, 
            string bgColor = null, Utils.Common.LayoutType layout = Utils.Common.LayoutType.LeftRight, 
            string description = "", string defaulstring="")
            : base(title,  width, height, left, top, bgColor, layout, description, defaulstring)
        { }

    }
}
