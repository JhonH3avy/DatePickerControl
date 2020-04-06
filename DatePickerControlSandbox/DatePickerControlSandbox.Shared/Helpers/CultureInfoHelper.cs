using System.Globalization;
using Windows.Globalization.DateTimeFormatting;

public static class CultureInfoHelper
{
    public static CultureInfo GetCurrentCulture()
    {
        var cultureName = new DateTimeFormatter("longdate", new[] { "US" }).ResolvedLanguage;
        return new CultureInfo(cultureName);
    }
}