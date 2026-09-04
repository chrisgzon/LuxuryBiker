using System.Globalization;
using LuxuryBiker.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuxuryBiker.Application.Dashboard.Queries.GetDashboard
{
    [Authorize]
    public record GetDashboardQuery : IRequest<ErrorOr<DashboardDto>>;

    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, ErrorOr<DashboardDto>>
    {
        private const int DailyWindowDays = 15;
        private static readonly CultureInfo SpanishCulture = CultureInfo.GetCultureInfo("es-ES");

        private readonly ILuxuryBikerDbContext _context;

        public GetDashboardQueryHandler(ILuxuryBikerDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            int year = now.Year;
            var todayStart = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);
            var tomorrowStart = todayStart.AddDays(1);
            var monthStart = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset);
            var dailyWindowStart = todayStart.AddDays(-(DailyWindowDays - 1));

            decimal salesToday = await _context.Sales
                .Where(s => s.Status == true && s.Date >= todayStart && s.Date < tomorrowStart)
                .SumAsync(s => (decimal?)s.Total, cancellationToken) ?? 0m;

            decimal purchasesThisMonth = await _context.Purchases
                .Where(p => p.Status == true && p.DatePurchase >= monthStart)
                .SumAsync(p => (decimal?)p.Total, cancellationToken) ?? 0m;

            decimal salesThisMonth = await _context.Sales
                .Where(s => s.Status == true && s.Date >= monthStart)
                .SumAsync(s => (decimal?)s.Total, cancellationToken) ?? 0m;

            var purchasesByMonthRaw = await _context.Purchases
                .Where(p => p.Status == true && p.DatePurchase.Year == year)
                .GroupBy(p => p.DatePurchase.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Total) })
                .ToListAsync(cancellationToken);

            var salesByMonthRaw = await _context.Sales
                .Where(s => s.Status == true && s.Date.Year == year)
                .GroupBy(s => s.Date.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Total) })
                .ToListAsync(cancellationToken);

            var salesByDayRaw = await _context.Sales
                .Where(s => s.Status == true && s.Date >= dailyWindowStart && s.Date < tomorrowStart)
                .GroupBy(s => new { s.Date.Year, s.Date.Month, s.Date.Day })
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Day, Total = g.Sum(x => x.Total) })
                .ToListAsync(cancellationToken);

            var topProductsRaw = await (
                    from saleDetail in _context.SaleDetails
                    join sale in _context.Sales on saleDetail.SaleId equals sale.Id
                    join product in _context.Products on saleDetail.ProductId equals product.Id
                    where sale.Status == true && sale.Date.Year == year
                    group saleDetail by new { saleDetail.ProductId, product.Code, product.Name, product.Stock } into g
                    select new
                    {
                        g.Key.ProductId,
                        g.Key.Code,
                        g.Key.Name,
                        g.Key.Stock,
                        QuantitySold = g.Sum(x => x.Quantity)
                    })
                .OrderByDescending(x => x.QuantitySold)
                .Take(10)
                .ToListAsync(cancellationToken);

            var dto = new DashboardDto
            {
                Year = year,
                CurrentMonthName = SpanishCulture.DateTimeFormat.GetMonthName(now.Month),
                SalesToday = salesToday,
                PurchasesThisMonth = purchasesThisMonth,
                SalesThisMonth = salesThisMonth,
                PurchasesByMonth = BuildMonthlySeries(purchasesByMonthRaw.ToDictionary(x => x.Month, x => x.Total)),
                SalesByMonth = BuildMonthlySeries(salesByMonthRaw.ToDictionary(x => x.Month, x => x.Total)),
                SalesLast15Days = BuildDailySeries(
                    DateOnly.FromDateTime(dailyWindowStart.DateTime),
                    salesByDayRaw.ToDictionary(x => new DateOnly(x.Year, x.Month, x.Day), x => x.Total)),
                TopSellingProducts = topProductsRaw
                    .Select(x => new TopProductDto
                    {
                        ProductId = x.ProductId,
                        Code = x.Code,
                        Name = x.Name,
                        Stock = x.Stock ?? 0m,
                        QuantitySold = x.QuantitySold
                    })
                    .ToList()
            };

            return dto;
        }

        private static IReadOnlyList<MonthlyTotalDto> BuildMonthlySeries(IReadOnlyDictionary<int, decimal> totalsByMonth)
        {
            return Enumerable.Range(1, 12)
                .Select(month => new MonthlyTotalDto
                {
                    Month = month,
                    MonthName = SpanishCulture.DateTimeFormat.GetMonthName(month),
                    Total = totalsByMonth.TryGetValue(month, out var total) ? total : 0m
                })
                .ToList();
        }

        private static IReadOnlyList<DailyTotalDto> BuildDailySeries(DateOnly start, IReadOnlyDictionary<DateOnly, decimal> totalsByDay)
        {
            return Enumerable.Range(0, DailyWindowDays)
                .Select(offset =>
                {
                    var date = start.AddDays(offset);
                    return new DailyTotalDto
                    {
                        Date = date,
                        Total = totalsByDay.TryGetValue(date, out var total) ? total : 0m
                    };
                })
                .ToList();
        }
    }
}
