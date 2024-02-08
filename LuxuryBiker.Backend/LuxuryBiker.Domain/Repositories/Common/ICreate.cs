namespace LuxuryBiker.Domain.Repositories.Common
{
    public interface ICreate<TEntity>
    {
        Task CreateAsync(TEntity entity);
    }
}
