using System.Globalization;
using LuxuryBiker.Domain.Repositories.Reporting;

namespace LuxuryBiker.Application.Dashboard.Queries.GetDashboard
{
    [Authorize]
    public record GetDashboardQuery : IRequest<ErrorOr<DashboardDto>>;

    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, ErrorOr<DashboardDto>>
    {
        private const int DailyWindowDays = 15;
        private const int TopProductsCount = 10;
        private static readonly CultureInfo SpanishCulture = CultureInfo.GetCultureInfo("es-ES");

        private readonly IDashboardRepository _repository;

        public GetDashboardQueryHandler(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<ErrorOr<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset now = DateTimeOffset.Now;

            DashboardSnapshot snapshot = await _repository.GetSnapshotAsync(
                now, DailyWindowDays, TopProductsCount, cancellationToken);

            var dailyWindowStart = DateOnly.FromDateTime(now.Date).AddDays(-(DailyWindowDays - 1));

            return new DashboardDto
            {
                Year = now.Year,
                CurrentMonthName = SpanishCulture.DateTimeFormat.GetMonthName(now.Month),
                SalesToday = snapshot.SalesToday,
                PurchasesThisMonth = snapshot.PurchasesThisMonth,
                SalesThisMonth = snapshot.SalesThisMonth,
                PurchasesByMonth = BuildMonthlySeries(snapshot.PurchaseTotalsByMonth),
                SalesByMonth = BuildMonthlySeries(snapshot.SaleTotalsByMonth),
                SalesLast15Days = BuildDailySeries(dailyWindowStart, snapshot.SaleTotalsByDay),
                TopSellingProducts = snapshot.TopSellingProducts
                    .Select(x => new TopProductDto
                    {
                        ProductId = x.ProductId,
                        Code = x.Code,
                        Name = x.Name,
                        Stock = x.Stock,
                        QuantitySold = x.QuantitySold
                    })
                    .ToList()
            };
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
