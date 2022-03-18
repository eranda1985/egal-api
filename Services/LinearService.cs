using System.Diagnostics;
using System.Text.Json;

namespace LinearServices;
public class LinearService : ILinearService
{
    private string _pythonDir => Path.Combine(Directory.GetCurrentDirectory(), "linear_regression");
    private ILogger<LinearService> _logger;

    public LinearService(ILogger<LinearService> logger)
    {
        _logger = logger;
    }

    public async Task<LinearRegressionResult?> GetLinearRegression(string url)
    {
        // Check if the incoming url is http or https 
        _logger.LogWarning("Checking if the input URL is valid..");
        var checkUrlResult = Utilities.CheckIsAUrl(url);
        if (!checkUrlResult)
        {
            throw new Exception("Input URL is not a valid http or https url. ");
        }

        _logger.LogWarning("Checking if the URL has a file associated and the csv is valid..");
        // Parse the csv file and check if it's in right format.
        var checkFileResult = await Utilities.CheckFileExistsInUrl(url);
        if (!checkFileResult)
        {
            throw new Exception("The CSV file is either not available in the URL or the file is not a valid CSV");
        }

        using (var python = new Process())
        {
            // python must be enabled in the environment variable for the following. 
            python.StartInfo.RedirectStandardOutput = true;
            python.StartInfo.UseShellExecute = false;
            python.StartInfo.Arguments = $"{_pythonDir}\\lr.py {url}";
            python.StartInfo.FileName = "python";
            python.Start();

            var jsonStr = python.StandardOutput.ReadToEnd();
            var jsonModel = JsonSerializer.Deserialize<LinearRegressionResult>(jsonStr);
            return jsonModel;
        }
    }

    public async Task<string> GetGraph(string url)
    {
        // Check if the incoming url is http or https 
        _logger.LogWarning("Checking if the input URL is valid..");
        var checkUrlResult = Utilities.CheckIsAUrl(url);
        if (!checkUrlResult)
        {
            throw new Exception("Input URL is not a valid http or https url. ");
        }

        _logger.LogWarning("Checking if the URL has a file associated and the csv is valid..");
        // Parse the csv file and check if it's in right format.
        var checkFileResult = await Utilities.CheckFileExistsInUrl(url);
        if (!checkFileResult)
        {
            throw new Exception("The CSV file is either not available in the URL or the file is not a valid CSV");
        }

        using (var python = new Process())
        {
            // python must be enabled in the environment variable for the following. 
            python.StartInfo.RedirectStandardOutput = true;
            python.StartInfo.UseShellExecute = false;
            python.StartInfo.Arguments = $"{_pythonDir}\\graph.py {url}";
            python.StartInfo.FileName = "python";
            python.Start();

            var base64Str = python.StandardOutput.ReadToEnd();
            return base64Str;
        }
    }
}