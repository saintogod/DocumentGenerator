using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
    public class Orders
    {
        public Book[] Books;
    }

    public class Book
    {
        public string ISBN;
    }

    public class ExpandedBook : Book
    {
        public bool NewEdition;
    }

    public class Run
    {
        public static void SerializeObject(string filename)
        {
            // Each overridden field, property, or type requires 
            // an XmlAttributes instance.
            XmlAttributes attrs = new XmlAttributes();

            // Creates an XmlElementAttribute instance to override the 
            // field that returns Book objects. The overridden field
            // returns Expanded objects instead.
            XmlElementAttribute attr = new XmlElementAttribute();
            attr.ElementName = "Field";
            attr.Type = typeof(ExpandedBook);

            // Adds the element to the collection of elements.
            attrs.XmlElements.Add(attr);

            // Creates the XmlAttributeOverrides instance.
            XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();

            // Adds the type of the class that contains the overridden 
            // member, as well as the XmlAttributes instance to override it 
            // with, to the XmlAttributeOverrides.
            attrOverrides.Add(typeof(Orders), "Books", attrs);

            // Creates the XmlSerializer using the XmlAttributeOverrides.
            XmlSerializer s =
            new XmlSerializer(typeof(Orders), attrOverrides);

            // Writing the file requires a TextWriter instance.
            TextWriter writer = new StreamWriter(filename);

            // Creates the object to be serialized.
            Orders myOrders = new Orders();

            // Creates an object of the derived type.
            ExpandedBook b = new ExpandedBook();
            b.ISBN = "123456789";
            b.NewEdition = true;
            myOrders.Books = new ExpandedBook[] { b };

            // Serializes the object.
            s.Serialize(writer, myOrders);
            writer.Close();
        }

        public static void DeserializeObject(string filename)
        {
            XmlAttributeOverrides attrOverrides =
                new XmlAttributeOverrides();
            XmlAttributes attrs = new XmlAttributes();

            // Creates an XmlElementAttribute to override the 
            // field that returns Book objects. The overridden field
            // returns Expanded objects instead.
            XmlElementAttribute attr = new XmlElementAttribute();
            attr.ElementName = "NewBook";
            attr.Type = typeof(ExpandedBook);

            // Adds the XmlElementAttribute to the collection of objects.
            attrs.XmlElements.Add(attr);

            attrOverrides.Add(typeof(Orders), "Books", attrs);

            // Creates the XmlSerializer using the XmlAttributeOverrides.
            XmlSerializer s =
            new XmlSerializer(typeof(Orders), attrOverrides);

            FileStream fs = new FileStream(filename, FileMode.Open);
            Orders myOrders = (Orders)s.Deserialize(fs);
            Console.WriteLine("ExpandedBook:");

            // The difference between deserializing the overridden 
            // XML document and serializing it is this: To read the derived 
            // object values, you must declare an object of the derived type 
            // and cast the returned object to it.
            ExpandedBook expanded;
            foreach (Book b in myOrders.Books)
            {
                expanded = (ExpandedBook)b;
                Console.WriteLine(
                expanded.ISBN + "\n" +
                expanded.NewEdition);
            }
        }

        public static void Main123(string[] args)
        {
            string path = @"E:\saint.xml";
            SerializeObject(path);
            DeserializeObject(path);
        }
    }
}
