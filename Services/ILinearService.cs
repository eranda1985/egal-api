namespace LinearServices
{
    public interface ILinearService
    {
        Task<LinearRegressionResult?> GetLinearRegression(string url);
        Task<string> GetGraph(string url);
    }
}