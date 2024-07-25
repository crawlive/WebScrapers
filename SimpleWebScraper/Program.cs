using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;


namespace SimpleWebScraper 
{ 
	class Program 
	{ 
		static void Main(string[] args) 
		{ 
			Console.WriteLine("Starting Scraper Bot PokeCountry3000..."); 
 
			// scraping logic... 
            var web = new HtmlWeb();

            // loading the target web page 
            var document = web.Load("https://www.scrapethissite.com/pages/simple/");

            var countries = new List<Country>();

            // selecting all HTML country elements from the current page 
            var countryHTMLElements = document.DocumentNode.QuerySelectorAll("div.col-md-4.country");

            // iterating over the list of country elements 
            foreach (var countryElement in countryHTMLElements) 
            { 
                // instancing a new Country object 
                var country = new Country();
                // scraping the interesting data from the current HTML element 

                country.Country_Name = HtmlEntity.DeEntitize(countryElement.QuerySelector("h3").InnerText).Trim(); 
                var countryInfoHtmlElements = document.DocumentNode.QuerySelectorAll("div.country-info");
                // sub container
                foreach (var countryInfoElement in countryInfoHtmlElements)
                {
                    country.Capital = HtmlEntity.DeEntitize(countryElement.QuerySelector("span.country-capital").InnerText).Trim(); 
                    country.Population = Convert.ToInt32(HtmlEntity.DeEntitize(countryElement.QuerySelector("span.country-population").InnerText).Trim()); 
                    country.Area = Convert.ToDouble(HtmlEntity.DeEntitize(countryElement.QuerySelector("span.country-area").InnerText).Trim()); 
                }
                // adding the object containing the scraped data to the list 
                countries.Add(country); 
            }

            // initializing the CSV output file 
            using (var writer = new StreamWriter("countries.csv")) 
            // initializing the CSV writer 
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) 
            { 
                // populating the CSV file 
                csv.WriteRecords(countries); 
            }
		} 
	} 

    public class Country 
    { 
        public string? Country_Name { get; set; } 
        public string? Capital { get; set; } 
        public int? Population { get; set; } 
        public double? Area { get; set; } 
    }

}
