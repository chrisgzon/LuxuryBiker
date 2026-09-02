namespace LuxuryBiker.Application.Dashboard.Queries.GetDashboard
{
    public class DashboardDto
    {
        public int Year { get; set; }
        public string CurrentMonthName { get; set; } = string.Empty;

        public decimal SalesToday { get; set; }
        public decimal PurchasesThisMonth { get; set; }
        public decimal SalesThisMonth { get; set; }

        /// <summary>12 posiciones (enero..diciembre) del año en curso.</summary>
        public IReadOnlyList<MonthlyTotalDto> PurchasesByMonth { get; set; } = Array.Empty<MonthlyTotalDto>();
        public IReadOnlyList<MonthlyTotalDto> SalesByMonth { get; set; } = Array.Empty<MonthlyTotalDto>();

        /// <summary>Últimos 15 días (incluye hoy), ordenados ascendentemente.</summary>
        public IReadOnlyList<DailyTotalDto> SalesLast15Days { get; set; } = Array.Empty<DailyTotalDto>();

        public IReadOnlyList<TopProductDto> TopSellingProducts { get; set; } = Array.Empty<TopProductDto>();
    }

    public class MonthlyTotalDto
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    public class DailyTotalDto
    {
        public DateOnly Date { get; set; }
        public decimal Total { get; set; }
    }

    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Stock { get; set; }
        public decimal QuantitySold { get; set; }
    }
}
