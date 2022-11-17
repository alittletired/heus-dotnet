
using System.Globalization;
using Humanizer;
namespace Heus.Core.Utils;
public static class HumanizerUtils
{
    
    public static string Pluralize(string name)
    {
        return name.Pluralize(inputIsKnownToBeSingular: false);
    }
    public static string Singularize(string name)
        => name.Singularize(inputIsKnownToBePlural: false);
}