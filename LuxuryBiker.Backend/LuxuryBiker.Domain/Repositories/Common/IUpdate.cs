namespace LuxuryBiker.Domain.Repositories.Common
{
    public interface IUpdate<TEntity>
    {
        void Update(TEntity entity);
    }
}
