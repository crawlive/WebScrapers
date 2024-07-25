using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;


namespace WikiTurtlesScraper
{
    public class Program
    {
	public static void Main()
        {
            //connect and load to page
            var web = new HtmlWeb();

            // loading the target web page 
            var document = web.Load("https://en.wikipedia.org/wiki/List_of_Testudines_families");

            var turtles = new List<Turtle>();
            
            var tableHTMLElements = document.DocumentNode.QuerySelectorAll("tr");

            foreach (row in tableHTMLElements) 
            {
                var family = 
            }

            // initializing the CSV output file 
            using (var writer = new StreamWriter("WikiTurtlesPull.csv")) 
            // initializing the CSV writer 
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) 
            { 
                // populating the CSV file 
                csv.WriteRecords(turtles); 
            }

        }
    }
}
