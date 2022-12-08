using System.Security.Cryptography;

namespace Heus.Core.Utils;

public static class RandomUtils
{
    public static string GenerateNumberString(int len)
    {
        switch (len)
        {
            case <= 0:
                throw new InvalidDataException($"num must be a positive integer.input: {len}");
            case >= 10:
                throw new InvalidDataException($"num must less than 10.input: {len}");
        }

        var  lowerBound = (int)Math.Pow(10, len-1);
        var upperBound = (int)Math.Pow(10, len);
        return RandomNumberGenerator.GetInt32(lowerBound, upperBound).ToString();

    }
}
