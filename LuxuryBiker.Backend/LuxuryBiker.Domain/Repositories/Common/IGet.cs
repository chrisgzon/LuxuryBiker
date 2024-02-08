namespace LuxuryBiker.Domain.Repositories.Common
{
    public interface IGet<TEntity, TEntityId>
    {
        Task<List<TEntity>> GetAsync();
        Task<List<TEntity>> GetAsync(TEntityId entityId);
    }
}
