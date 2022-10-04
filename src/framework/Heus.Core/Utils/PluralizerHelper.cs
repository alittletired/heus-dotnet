
using System.Globalization;

namespace Heus.Core.Utils;
public static class PluralizerHelper
{
    private static Inflector.Inflector inflector = new Inflector.Inflector(new CultureInfo("en"));
    public static string Pluralize(string name)
    {
        return inflector.Pluralize(name);
    }
}