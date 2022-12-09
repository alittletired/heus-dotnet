
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

    //"some_title for something".Pascalize() => "SomeTitleForSomething"
    public static string Pascalize(string name)
    {
       return name.Pascalize();
    }
    //"some_title for something".Camelize() => "someTitleForSomething"
    public static string Camelize(string name)
    {
        return name.Camelize();
    }
    //"SomeTitle".Underscore() => "some_title"
    public static string Underscore(string name)
    {
        return name.Underscore();
    }
    //"SomeText".Kebaberize() => "some-text"
    public static string Kebaberize(string name)
    {
        return name.Kebaberize();
    }
    
}