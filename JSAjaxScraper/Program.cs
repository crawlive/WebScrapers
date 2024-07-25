//using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;
using System.Text.Json;

namespace JSAjaxScraper
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var OscarMovies = new List<Movie>();
            using (var client = new HttpClient())
            {
                // Set any headers you observed are necessary for the AJAX request
                //client.DefaultRequestHeaders.Add("User-Agent", "C# App"); // Example header
                var baseURL = "https://www.scrapethissite.com/pages/ajax-javascript/?ajax=true&year=";
                var yearList = new List<string> { "2010", "2011", "2012", "2013", "2014", "2015" };
                foreach (var year in yearList)
                {
                    var ajaxUrl = $"{baseURL}{year}";
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(ajaxUrl);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();

                        // If the response is JSON, you can parse it using Json.NET or System.Text.Json
                        using var jsonDoc = JsonDocument.Parse(responseBody);
                        // If the response is HTML, you can parse it using an HTML parser like HtmlAgilityPack
                        // HtmlDocument htmlDoc = new HtmlDocument();
                        // htmlDoc.LoadHtml(responseBody);

                        // Parse json doc into list and add to overall list
                        OscarMovies.AddRange(JsonSerializer.Deserialize<List<Movie>>(jsonDoc, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
                        
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("\nException Caught!");
                        Console.WriteLine("Message :{0} ", e.Message);
                    }
                }

                // initializing the CSV output file 
                using (var writer = new StreamWriter("OscarMovies.csv")) 
                // initializing the CSV writer 
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) 
                { 
                    // populating the CSV file 
                    csv.WriteRecords(OscarMovies); 
                }
                
            }
        }
    }
}
