using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;

//Scraping a site with pagination
namespace HockeyTeamsScraper
{
    public class Program
    {
        public static async Task Main() 
		{
            // list of scraped data
            var hockeyTeams = new List<Team>();
            //connect and load to page
            var web = new HtmlWeb();
            // Loop through pagination
            var baseURL = "https://www.scrapethissite.com/pages/forms/?page_num=";
            var hasNextPage = true;
            var currPageNum = 1;

            using (var httpClient = new HttpClient())
            {
                while(hasNextPage)
                {
                    // adjust url to current page
                    var currPage = $"{baseURL}{currPageNum}";
                    var response = await httpClient.GetAsync(currPage);

                    //check if page is valid
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Error accessing the page.");
                        hasNextPage = false;
                    }
                    // loading the base target web page 
                    var document = web.Load(currPage);

                    //scrape data
                    // select all elements matching team class
                    var teamHTMLElements = document.DocumentNode.QuerySelectorAll("tr.team");

                    // iterate through and grab team information
                    foreach (var teamElement in teamHTMLElements)
                    {
                        var team = new Team();
                        team.Name = HtmlEntity.DeEntitize(teamElement.QuerySelector("td.name").InnerText).Trim();
                        team.Year = Convert.ToInt32(HtmlEntity.DeEntitize(teamElement.QuerySelector("td.year").InnerText).Trim());
                        team.Wins = Convert.ToInt32(HtmlEntity.DeEntitize(teamElement.QuerySelector("td.wins").InnerText).Trim());
                        team.Losses = Convert.ToInt32(HtmlEntity.DeEntitize(teamElement.QuerySelector("td.losses").InnerText).Trim());
                        var overtimeLosses = HtmlEntity.DeEntitize(teamElement.QuerySelector("td.ot-losses").InnerText).Trim();
                        team.OTLosses = overtimeLosses.Equals("") ? 0 : Convert.ToInt32(overtimeLosses);
                        team.SuccessPercents = Convert.ToDouble(HtmlEntity.DeEntitize(teamElement.QuerySelector("td.pct").InnerText).Trim());
                        team.GoalsFor = Convert.ToInt32(HtmlEntity.DeEntitize(teamElement.QuerySelector("td.gf").InnerText).Trim());
                        team.GoalsAgainst = Convert.ToInt32(HtmlEntity.DeEntitize(teamElement.QuerySelector("td.ga").InnerText).Trim());
                        team.GoalsDifference = Convert.ToInt32(HtmlEntity.DeEntitize(teamElement.QuerySelector("td.diff").InnerText).Trim());

                        //add team to list
                        hockeyTeams.Add(team);
                    }

                    currPageNum++;
                    // Determine if there is a next page by looking at the pagination next button
                    // This will depend on the specific website's structure
                    var nextPageNode = document.DocumentNode.SelectSingleNode("//a[@aria-label='Next']");
                    hasNextPage = nextPageNode != null;

                    // Be polite and don't hammer the server; add a delay
                    await Task.Delay(1000);
                }
            }


            // initializing the CSV output file 
            using (var writer = new StreamWriter("hockeyStats.csv")) 
            // initializing the CSV writer 
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) 
            { 
                // populating the CSV file 
                csv.WriteRecords(hockeyTeams); 
            }
        }
    }
}