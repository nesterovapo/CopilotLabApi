namespace CopilotLabApi.Utils;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;

public static class ValidationHelper
{
    public static IResult? Validate(object dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        if (Validator.TryValidateObject(dto, context, results, validateAllProperties: true)) return null;

        var errors = results
            .SelectMany(r => r.MemberNames.DefaultIfEmpty(string.Empty).Select(m => new { Member = m, Error = r.ErrorMessage ?? string.Empty }))
            .GroupBy(x => x.Member)
            .ToDictionary(g => string.IsNullOrWhiteSpace(g.Key) ? string.Empty : g.Key, g => g.Select(x => x.Error).ToArray());

        return Results.ValidationProblem(errors);
    }

    /// <summary>
    /// Lightweight email validation helper. Returns true for syntactically valid emails.
    /// Uses MailAddress parsing then a conservative regex fallback.
    /// </summary>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            var addr = new MailAddress(email);
            // MailAddress allows some odd inputs; do a simple regex sanity check as well
            var pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
        catch
        {
            return false;
        }
    }
}
