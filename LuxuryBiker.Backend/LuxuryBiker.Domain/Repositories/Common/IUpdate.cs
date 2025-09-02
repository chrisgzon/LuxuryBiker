namespace LuxuryBiker.Domain.Repositories.Common
{
    public interface IUpdate<TEntity>
    {
        Task UpdateAsync(TEntity entity);
    }
}
