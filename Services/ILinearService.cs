namespace LinearServices
{
    public interface ILinearService
    {
        Task<LinearRegressionResult?> GetLinearRegression(string url);
        Task<string> GetGraph(string url);
        Task<string> GetHist(string url);
        Task<string> GetRegressionGraph(string url);
        Task<StatsResult?> GetStats(string url);
    }
}