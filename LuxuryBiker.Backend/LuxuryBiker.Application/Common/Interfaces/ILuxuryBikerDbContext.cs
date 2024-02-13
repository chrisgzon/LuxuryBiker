namespace LuxuryBiker.Application.Common.Interfaces
{
    public interface ILuxuryBikerDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
