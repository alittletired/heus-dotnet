namespace Heus.Core.Utils;

public static class RandomUtils
{
    private const string Charset = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static Random random = new ();

    public static string GenerateString(int len)
    {
        if (len <= 0)
        {
            throw new InvalidDataException($"num must be a positive integer.input: {len}");
        }

        return new string(Enumerable.Repeat(Charset, len)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
