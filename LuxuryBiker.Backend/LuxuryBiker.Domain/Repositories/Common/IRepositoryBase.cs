namespace LuxuryBiker.Domain.Repositories.Common
{
    public interface IRepositoryBase<TEntity, TEntityId>
                    : ICreate<TEntity>, IUpdate<TEntity>, IGet<TEntity, TEntityId>
    {
    }
}
