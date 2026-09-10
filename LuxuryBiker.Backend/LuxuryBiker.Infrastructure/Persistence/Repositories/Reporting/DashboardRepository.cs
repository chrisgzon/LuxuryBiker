using LuxuryBiker.Domain.Repositories.Reporting;
using Microsoft.EntityFrameworkCore;

namespace LuxuryBiker.Infrastructure.Persistence.Repositories.Reporting
{
    internal class DashboardRepository : IDashboardRepository
    {
        private readonly LuxuryBikerDbContext _context;

        public DashboardRepository(LuxuryBikerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DashboardSnapshot> GetSnapshotAsync(
            DateTimeOffset now, int dailyWindowDays, int topProductsCount, CancellationToken cancellationToken)
        {
            int year = now.Year;
            var todayStart = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);
            var tomorrowStart = todayStart.AddDays(1);
            var monthStart = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset);
            var dailyWindowStart = todayStart.AddDays(-(dailyWindowDays - 1));

            decimal salesToday = await _context.Sales
                .Where(s => s.Status == true && s.Date >= todayStart && s.Date < tomorrowStart)
                .SumAsync(s => (decimal?)s.Total, cancellationToken) ?? 0m;

            decimal purchasesThisMonth = await _context.Purchases
                .Where(p => p.Status == true && p.DatePurchase >= monthStart)
                .SumAsync(p => (decimal?)p.Total, cancellationToken) ?? 0m;

            decimal salesThisMonth = await _context.Sales
                .Where(s => s.Status == true && s.Date >= monthStart)
                .SumAsync(s => (decimal?)s.Total, cancellationToken) ?? 0m;

            var purchasesByMonth = await _context.Purchases
                .Where(p => p.Status == true && p.DatePurchase.Year == year)
                .GroupBy(p => p.DatePurchase.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Total) })
                .ToListAsync(cancellationToken);

            var salesByMonth = await _context.Sales
                .Where(s => s.Status == true && s.Date.Year == year)
                .GroupBy(s => s.Date.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Total) })
                .ToListAsync(cancellationToken);

            var salesByDay = await _context.Sales
                .Where(s => s.Status == true && s.Date >= dailyWindowStart && s.Date < tomorrowStart)
                .GroupBy(s => new { s.Date.Year, s.Date.Month, s.Date.Day })
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Day, Total = g.Sum(x => x.Total) })
                .ToListAsync(cancellationToken);

            var topProducts = await (
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
                .Take(topProductsCount)
                .ToListAsync(cancellationToken);

            return new DashboardSnapshot(
                SalesToday: salesToday,
                PurchasesThisMonth: purchasesThisMonth,
                SalesThisMonth: salesThisMonth,
                PurchaseTotalsByMonth: purchasesByMonth.ToDictionary(x => x.Month, x => x.Total),
                SaleTotalsByMonth: salesByMonth.ToDictionary(x => x.Month, x => x.Total),
                SaleTotalsByDay: salesByDay.ToDictionary(x => new DateOnly(x.Year, x.Month, x.Day), x => x.Total),
                TopSellingProducts: topProducts
                    .Select(x => new TopSellingProduct(x.ProductId, x.Code, x.Name, x.Stock ?? 0m, x.QuantitySold))
                    .ToList());
        }
    }
}
