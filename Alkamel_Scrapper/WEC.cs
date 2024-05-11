using System;
using System.Net;
using Microsoft.VisualBasic;
using PuppeteerSharp;

namespace Alkamel_Scrapper;
public static class WEC
{
    // use PuppeteerSharp to scrape the website
    public static async Task<List<string>> ScrapeData(int year, string eventNumber = "03")
    {
        var url = $"http://fiawec.alkamelsystems.com/?season={Alkamel.Season(year)}&evvent={Alkamel.Event(eventNumber)}";


        // download the browser
        await new BrowserFetcher().DownloadAsync();
        // define options
        var options = new LaunchOptions { Headless = true };
        // launch the browser
        using var browser = await Puppeteer.LaunchAsync(options);
        // create a new page
        using var page = await browser.NewPageAsync();
        // go to the url
        await page.GoToAsync(url);

        //find all the csv files
        var urls = await page.EvaluateFunctionAsync<List<string>>("() => { return Array.from(document.querySelectorAll('a')).map(a => a.href); }");

        // return the urls
        return urls
            .Where(x => x.Contains(".csv", StringComparison.InvariantCultureIgnoreCase))
            .Where(x => x.Contains("fia%20wec", StringComparison.InvariantCultureIgnoreCase))
            .Where(x => x.Contains("qualifying", StringComparison.InvariantCultureIgnoreCase)
                || x.Contains("hyperpole", StringComparison.InvariantCultureIgnoreCase)
                || x.Contains("race", StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    // download the csv files
    public static async Task DownloadFiles(List<string> urls)
    {
        // validate if the directory exists
        var basePath = Path.Combine(Environment.CurrentDirectory, "data");
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        foreach (var url in urls)
        {
            // parse the url
            // decode the url
            var decode = WebUtility.UrlDecode(url);
            var filename = Path.GetFileName(decode);
            var path = Path.Combine(Environment.CurrentDirectory, "data", filename);
            // validate if file exists
            if (!File.Exists(path))
            {
                using var client = new HttpClient();
                using var stream = await client.GetStreamAsync(url);
                using var fileStream = new FileStream(path, FileMode.Create);
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}