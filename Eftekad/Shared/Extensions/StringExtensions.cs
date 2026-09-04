using System.Globalization;

namespace Eftekad.Shared.Extensions;

public static class StringExtensions
{
    public static DateOnly? ToDateOnly(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var formats = new[] { 
            "dd-MM-yyyy", 
            "d-M-yyyy",
            "d-MM-yyyy", 
            "dd-M-yyyy",
            "dd/MM/yyyy",
            "d/M/yyyy",
            "dd/M/yyyy",
            "d/MM/yyyy",
        };
        
        // Try exact formats
        if (DateTime.TryParseExact(value, formats, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var date))
        {
            return DateOnly.FromDateTime(date);
        }

        // Fallback to TryParse
        if (DateTime.TryParse(value, out var fallbackDate))
        {
            return DateOnly.FromDateTime(fallbackDate);
        }

        return null;
    }
}