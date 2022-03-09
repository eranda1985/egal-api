
public record LinearRegressionResult()
{
    public float y_interceptor { get; set; }
    public float coefficent { get; set; }
    public List<float>? x_samples { get; set; }
    public List<float>? y_samples { get; set; }
}