using System.Text;

namespace Icof.Api.Services
{
    /// <summary>
    /// Shared slug generation, used anywhere a human-entered name/title needs to become a
    /// URL/DB-safe slug (DataSeeder, TeamMembersController, etc.) — one implementation so slugs
    /// stay consistent across every place they're generated.
    /// </summary>
    public static class SlugHelper
    {
        public static string Slugify(string value)
        {
            var normalized = value
                .Replace("č", "c").Replace("Č", "C")
                .Replace("ć", "c").Replace("Ć", "C")
                .Replace("š", "s").Replace("Š", "S")
                .Replace("ž", "z").Replace("Ž", "Z")
                .Replace("đ", "dj").Replace("Đ", "Dj");

            var builder = new StringBuilder();
            var lastWasHyphen = false;

            foreach (var c in normalized.ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(c);
                    lastWasHyphen = false;
                }
                else if (!lastWasHyphen && builder.Length > 0)
                {
                    builder.Append('-');
                    lastWasHyphen = true;
                }
            }

            return builder.ToString().TrimEnd('-');
        }
    }
}
