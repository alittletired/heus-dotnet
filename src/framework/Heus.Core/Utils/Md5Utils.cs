using System.Text;

namespace Heus.Core.Utils;

public static class Md5Utils
{
    public static string ToHash(string input)
    {
        // step 1, calculate MD5 hash from input


        using var md5 = System.Security.Cryptography.MD5.Create();

        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }

        return sb.ToString();
    }
}