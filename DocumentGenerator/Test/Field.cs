using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
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
        public String Layout { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("DefaultValue")]
        public string DefaultValue { get; set; }


        public Field()
        {
            this.Layout = "LeftRight";
        }

        public Field(string title, double width, double height, double left, double top, 
            string bgColor, string layout, string description, string defaultValue)
        {
            this.Title = title;
            this.Width = width;
            this.Height = height;
            this.Left = left;
            this.Top = top;
            this.BackgroundColor = bgColor;
            this.Layout = layout;
            this.Description = description;
            this.DefaultValue = defaultValue;
        }

        public Field(Field field = null)
        {
            if (field != null)
            {
                this.Title = field.Title;
                this.BackgroundColor = field.BackgroundColor;
                this.Layout = field.Layout;
                this.Width = field.Width;
                this.Height = field.Height;
                this.Left = field.Left;
                this.Top = field.Top;
                this.Description = field.Description;
                this.DefaultValue = field.DefaultValue;
            }
            else
                this.Layout = "LeftRight";
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

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Title.GetHashCode() * 7 + this.Description.GetHashCode() * 13);
            }
        }
    }
}
