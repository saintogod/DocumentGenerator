using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class InterfaceAndInheritance:Iface<AV>
    {
        public string Name
        {
            get { return "Name of the InterfaceAndInheritance"; }
            set { Name = value; }
        }
        public static void Main12(string[] args)
        {
            Type tDerived = typeof(Iface<>);
            InterfaceAndInheritance iai = new InterfaceAndInheritance();
            if (iai is Iface<AV>)
                Console.WriteLine(iai.Name);
            Console.ReadKey();
        }


        public AV Movie
        {
            get;
            set;
        }
    }
    public interface Iface<T>
    {
        string Name { get; set; }
        T Movie { get; set; }
    }
}
