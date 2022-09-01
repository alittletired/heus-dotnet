using Heus.Core;
using System.Globalization;
namespace Heus.Utils;
public static class CultureHelper
{
    public static IDisposable Use(string culture, string? uiCulture = null)
    {

        return Use(new CultureInfo(culture), uiCulture == null ? null : new CultureInfo(uiCulture));
    }

    public static IDisposable Use(CultureInfo culture, CultureInfo? uiCulture = null)
    {

        var currentCulture = CultureInfo.CurrentCulture;
        var currentUiCulture = CultureInfo.CurrentUICulture;

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = uiCulture ?? culture;

        return DisposeAction.Create(() =>
        {
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentUiCulture;
        });
    }

    public static bool IsRtl => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

    public static bool IsValidCultureCode(string? cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
        {
            return false;
        }

        try
        {
            var culture = CultureInfo.GetCultureInfo(cultureCode);
            return culture != null;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }

    public static string GetBaseCultureName(string cultureName)
    {
        return cultureName.Contains('-')
            ? cultureName[cultureName.IndexOf("-", StringComparison.Ordinal)..]
            : cultureName;
    }
}
