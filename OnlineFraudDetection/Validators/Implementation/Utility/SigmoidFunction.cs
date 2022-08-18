namespace OnlineFraudDetection.Validators.Implementation.Utility;

public static class SigmoidFunction
{
    public static int Calculate(double input, double normalizationFactor, double avg)
    {
        var power = (-6 * (input - avg)) / (normalizationFactor * avg);
        return (int)((200 / (1 + Math.Pow(Math.E, power))) - 100);
    }
}