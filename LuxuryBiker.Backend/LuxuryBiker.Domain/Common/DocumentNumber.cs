using System.Text.RegularExpressions;

namespace LuxuryBiker.Domain.Common
{
    /// <summary>
    /// Numeración correlativa de los documentos comerciales ("CLB12" -> "CLB13").
    /// Es una regla de negocio, por eso vive en el dominio y no en la capa de aplicación.
    /// </summary>
    public static class DocumentNumber
    {
        public const string PurchasePrefix = "CLB";
        public const string SalePrefix = "VLB";

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
