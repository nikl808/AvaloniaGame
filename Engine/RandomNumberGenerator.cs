using System.Security.Cryptography;

namespace Engine;

public static class RandomNumberGenerator
{
    private static readonly RNGCryptoServiceProvider _generator = new();
    private static readonly Random _simpleGenerator = new();
    public static int NumberBetween(int minValue, int maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be greater than minValue");
        }
        var randomNumber = new byte[1];
        _generator.GetBytes(randomNumber);
        var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
        
        var multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

        var range = maxValue - minValue + 1;

        var randomValueRange = Math.Floor(multiplier * range);

        // Ensure the result is within the specified range
        return (int)(minValue + randomValueRange);
    }

    public static int SimpleNumberBetween(int minimumValue, int maximumValue)
    {
        return _simpleGenerator.Next(minimumValue, maximumValue + 1);
    }
}
