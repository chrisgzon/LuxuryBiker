namespace LuxuryBiker.Domain.Repositories.Common
{
    public interface IGet<TEntity, TEntityId>
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity?> GetAsync(TEntityId entityId);
    }
}
