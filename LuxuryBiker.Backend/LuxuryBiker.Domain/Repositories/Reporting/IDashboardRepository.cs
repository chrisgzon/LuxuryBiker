namespace LuxuryBiker.Domain.Repositories.Reporting
{
    public record TopSellingProduct(
        int ProductId,
        string? Code,
        string Name,
        decimal Stock,
        decimal QuantitySold);

    /// <summary>
    /// Datos crudos del panel. Las series vienen indexadas y sin rellenar: es la capa
    /// de aplicación la que decide el formato de presentación (12 meses, 15 días, etc.).
    /// </summary>
    public record DashboardSnapshot(
        decimal SalesToday,
        decimal PurchasesThisMonth,
        decimal SalesThisMonth,
        IReadOnlyDictionary<int, decimal> PurchaseTotalsByMonth,
        IReadOnlyDictionary<int, decimal> SaleTotalsByMonth,
        IReadOnlyDictionary<DateOnly, decimal> SaleTotalsByDay,
        IReadOnlyList<TopSellingProduct> TopSellingProducts);

    /// <summary>
    /// Puerto de lectura para los indicadores del panel. Solo debe considerar compras y
    /// ventas validadas (<c>Status = true</c>).
    /// </summary>
    public interface IDashboardRepository
    {
        Task<DashboardSnapshot> GetSnapshotAsync(
            DateTimeOffset now, int dailyWindowDays, int topProductsCount, CancellationToken cancellationToken);
    }
}
