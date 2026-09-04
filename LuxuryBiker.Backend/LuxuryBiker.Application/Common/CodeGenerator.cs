using System.Text.RegularExpressions;

namespace LuxuryBiker.Application.Common
{
    /// <summary>
    /// Genera el siguiente código correlativo a partir del último ("CLB12" -> "CLB13"),
    /// tomando el primer grupo de dígitos del código anterior.
    /// </summary>
    public static class CodeGenerator
    {
        public static string Next(string prefix, string? lastCode)
        {
            int lastNumber = 0;

            if (!string.IsNullOrWhiteSpace(lastCode))
            {
                Match match = Regex.Match(lastCode, @"\d+");
                if (match.Success)
                {
                    int.TryParse(match.Value, out lastNumber);
                }
            }

            return $"{prefix}{lastNumber + 1}";
        }
    }
}
