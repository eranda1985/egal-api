using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;

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
        _logger.LogWarning("Entering into GetLinearReggression method.");
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
            python.StartInfo.Arguments = $"{_pythonDir}/lr.py {url}";
            python.StartInfo.FileName = "python";
            python.Start();

            var jsonStr = python.StandardOutput.ReadToEnd();
            var jsonModel = JsonConvert.DeserializeObject<LinearRegressionResult>(jsonStr);
            return jsonModel;
        }
    }

    public async Task<string> GetGraph(string url)
    {
        _logger.LogWarning("Entering into GetGraph method");
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
            python.StartInfo.Arguments = $"{_pythonDir}/graph.py {url}";
            python.StartInfo.FileName = "python3.8";
            python.Start();

            var base64Str = python.StandardOutput.ReadToEnd();
            return base64Str;
        }
    }

    public async Task<string> GetHist(string url)
    {
        _logger.LogWarning("Entering into GetHist method");

        using (var python = new Process())
        {
            // python must be enabled in the environment variable for the following. 
            python.StartInfo.RedirectStandardOutput = true;
            python.StartInfo.UseShellExecute = false;
            python.StartInfo.Arguments = $"{_pythonDir}/hist.py {url}";
            python.StartInfo.FileName = "python3.8";
            python.Start();

            var base64Str = python.StandardOutput.ReadToEnd();
            return await Task.FromResult<string>(base64Str);
        }
    }

    public async Task<string> GetRegressionGraph(string url)
    {
        _logger.LogWarning("Entering into GetRegressionGraph method");

        using (var python = new Process())
        {
            // python must be enabled in the environment variable for the following. 
            python.StartInfo.RedirectStandardOutput = true;
            python.StartInfo.UseShellExecute = false;
            python.StartInfo.Arguments = $"{_pythonDir}/regression_plot.py {url}";
            python.StartInfo.FileName = "python3.8";
            python.Start();

            var base64Str = python.StandardOutput.ReadToEnd();
            return await Task.FromResult<string>(base64Str);
        }
    }

    public async Task<StatsResult?> GetStats(string url)
    {
        _logger.LogWarning("Entering into GetStats method");

        using (var python = new Process())
        {
            // Check if the incoming url is http or https 
            var checkUrlResult = Utilities.CheckIsAUrl(url);
            if (!checkUrlResult)
            {
                throw new Exception("Input URL is not a valid http or https url. ");
            }

            _logger.LogWarning("Checking URL for GETStats method..");
            // Parse the csv file and check if it's in right format.
            var checkFileResult = await Utilities.CheckFileExistsInUrl(url);
            if (!checkFileResult)
            {
                throw new Exception("The CSV file is either not available in the URL or the file is not a valid CSV");
            }

            // python must be enabled in the environment variable for the following. 
            python.StartInfo.RedirectStandardOutput = true;
            python.StartInfo.UseShellExecute = false;
            python.StartInfo.Arguments = $"{_pythonDir}/stats.py {url}";
            python.StartInfo.FileName = "python3.8";
            python.Start();

            var statsData = python.StandardOutput.ReadToEnd();
            var statsJsonModel = JsonConvert.DeserializeObject<StatsResult>(statsData);
            return statsJsonModel;
        }
    }
}
