using System.Text.Json.Serialization;

public record StatsResult(Dimension x, Dimension y);


public record Dimension(string count, string mean, string std, string min, string max);