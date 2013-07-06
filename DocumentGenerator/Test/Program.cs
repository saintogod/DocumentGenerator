using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
    class Program
    {
        static void Main0(string[] args)
        {
            List<AV> movlist = new List<AV>();
            movlist.Add(new AV() { Title = "Starship Troopers1", ReleaseDate = DateTime.Parse("11/1/1997"), Rating = 1, Character = "kong1" });
            movlist.Add(new AV() { Title = "Starship Troopers2", ReleaseDate = DateTime.Parse("11/2/1997"), Rating = 2, Character = "kong2" });
            movlist.Add(new AV() { Title = "Starship Troopers3", ReleaseDate = DateTime.Parse("11/3/1997"), Rating = 3, Character = "kong3" });
            movlist.Add(new AV() { Title = "Starship Troopers4", ReleaseDate = DateTime.Parse("11/4/1997"), Rating = 4, Character = "kong4" });
            movlist.Add(new AV() { Title = "Starship Troopers5", ReleaseDate = DateTime.Parse("11/5/1997"), Rating = 5, Character = "kong5" });
            movlist.Add(new AV() { Title = "Starship Troopers6", ReleaseDate = DateTime.Parse("11/6/1997"), Rating = 6, Character = "kong6" });


            SerializeToXML(movlist);

            foreach (Movie mov in DeserializeFromXML())
                Console.WriteLine(mov.ToString());

            Console.ReadKey();
        }

        static public void SerializeToXML(List<AV> movie)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<AV>));
            TextWriter textWriter = new StreamWriter(@"E:\av.xml");
            serializer.Serialize(textWriter, movie);
            textWriter.Close();
        }

        static List<AV> DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<AV>));
            TextReader textReader = new StreamReader(@"E:\av.xml");
            List<AV> movies;
            movies = (List<AV>)deserializer.Deserialize(textReader);
            textReader.Close();

            return movies;
        }

        static void Main(string[] args)
        {
            Movie mov = new Movie();
            mov.Title = "3D 肉蒲团";
            mov.Rating = 3;
            mov.ReleaseDate = DateTime.Now.AddMonths(4).AddDays(12);
            Logs<string, string> logs = new Logs<string, string>();
            logs.Add("saint", "saint");
            logs.Add("saintogod", "saintogod");
            mov.MyLog = logs;

            XmlSerializer serializer = new XmlSerializer(typeof(Movie));
            Stream stream = new FileStream(@"E:\moive.xml", FileMode.Create);
            serializer.Serialize(stream, mov);
            stream.Close();

            stream = new FileStream(@"E:\moive.xml", FileMode.Open);
            Movie movie = (Movie)serializer.Deserialize(stream);
            stream.Close();
            Console.WriteLine(movie);

            Console.ReadKey();
        }
    }
    [XmlRoot("AV")]
    public class AV : Movie
    {
        public string Character { get; set; }
        public AV() { }
        public override string ToString()
        {
            return string.Format("{0}, Character: {1}", base.ToString(), Character);
        }

    }
    [XmlRoot("Movie")]
    public class Movie
    {
        public string Title
        { get; set; }

        public int Rating
        { get; set; }

        public DateTime ReleaseDate
        { get; set; }
        [XmlElement("MyLogforTest")]
        public Logs<string, string> MyLog { get; set; }

        public Movie()
        { }

        public override string ToString()
        {
            return string.Format("Title: {0}, ReleaseDate: {1}, Ratting: {2}", Title, ReleaseDate, Rating);
        }
    }

}
