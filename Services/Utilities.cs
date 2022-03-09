using System.Net;
using System.Text.RegularExpressions;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace LinearServices;
public static class Utilities
{
    // Method to check if the url is http and not null
    public static bool CheckIsAUrl(string? url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            var match = Regex.Match(url, "https?://");
            return (match is not null && match.Success);
        }
        return false;
    }

    // Method to check if there is a CSV file available for the URL. 
    public static async Task<bool> CheckFileExistsInUrl(string? url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            // Check if the url has a file associated.
            using (var httpClient = new HttpClient())
            {
                using (var incomingStream = await httpClient.GetStreamAsync(url))
                {
                    var csvParserOptions = new CsvParserOptions(true, ',');
                    var csvMapper = new CsvModelMapping();
                    CsvParser<CsvModel> csvParser = new CsvParser<CsvModel>(csvParserOptions, csvMapper);
                    var csvResult = csvParser.ReadFromStream(incomingStream, System.Text.Encoding.ASCII).ToList();

                    // Check if the csv file has more than 9 records 
                    // Check the csv file itself is valid using the fist 10 records. 
                    // If this file is anything else (i.e. not csv), it will be invalid.  
                    return csvResult.Count > 9 && csvResult.Take(10).All(x => x.IsValid);
                }
            }
        }
        return false;
    }

    internal class CsvModelMapping : CsvMapping<CsvModel>
    {
        public CsvModelMapping() : base()
        {
            MapProperty(0, p => p.X);
            MapProperty(1, p => p.Y);
        }
    }
}