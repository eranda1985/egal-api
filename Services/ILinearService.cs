namespace LinearServices
{
    public interface ILinearService
    {
        Task<LinearRegressionResult?> GetLinearRegression(string url);
    }
}