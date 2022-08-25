namespace OnlineFraudDetection.Models;

public class SingleValueLog
{
    public int Id {get; set; }
    public string Type {get; set; }
    public float? Duration {get; set; }
    public int? Percentage {get; set; }
    public bool? IsRedisCacheEnabled {get; set; }
}