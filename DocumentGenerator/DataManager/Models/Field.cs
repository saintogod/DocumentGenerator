using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataManager.Models
{
    [XmlRoot("Field")]
    public abstract class Field : IComparable<Field>, IEquatable<Field>
    {
        [XmlElement("Title")]
        public string Title { get; set; }
        [XmlElement("Width")]
        public double Width { get; set; }
        [XmlElement("Height")]
        public double Height { get; set; }
        [XmlElement("Top")]
        public double Top { get; set; }
        [XmlElement("Left")]
        public double Left { get; set; }
        [XmlElement("BackgroundColor")]
        public string BackgroundColor { get; set; }
        [XmlElement("Layout")]
        public Utils.Common.LayoutType Layout { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("Defaulstring")]
        public string Defaulstring { get; set; }


        public Field()
        {
            this.Layout = Utils.Common.LayoutType.LeftRight;
            this.BackgroundColor = "#00000000";
        }

        public Field(string title, double width, double height, double left, double top, 
            string bgColor, Utils.Common.LayoutType layout, string description, string defaulstring)
        {
            this.Title = title;
            this.Width = width;
            this.Height = height;
            this.Left = left;
            this.Top = top;
            this.BackgroundColor = bgColor;
            this.Layout = layout;
            this.Description = description;
            this.Defaulstring = defaulstring;
        }

        public Field(Field field)
        {
                this.Title = field.Title;
                this.BackgroundColor = field.BackgroundColor;
                this.Layout = field.Layout;
                this.Width = field.Width;
                this.Height = field.Height;
                this.Left = field.Left;
                this.Top = field.Top;
                this.Description = field.Description;
                this.Defaulstring = field.Defaulstring;
        }

        int IComparable<Field>.CompareTo(Field other)
        {
            return this.Title.CompareTo(other.Title) != 0 ? this.Title.CompareTo(other.Title) :
                this.Description.CompareTo(other.Description);
        }

        bool IEquatable<Field>.Equals(Field other)
        {
            if (other == null)
                return false;
            return other.Title.Equals(this.Title) && other.Description.Equals(this.Description);
        }

        public virtual void SetValue(Field field) 
        {
            this.Title = field.Title;
            this.BackgroundColor = field.BackgroundColor;
            this.Layout = field.Layout;
            this.Width = field.Width;
            this.Height = field.Height;
            this.Left = field.Left;
            this.Top = field.Top;
            this.Description = field.Description;
            this.Defaulstring = field.Defaulstring;
        }
    }
}
