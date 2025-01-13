using System.Linq.Expressions;
using BaseRepository.EntityBase;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.RepositoryBase;
public interface IReadRepository<E, EId>: IGenericRepository where E : class, IEntity<EId> 
where EId: struct
{
    Task<bool> ExistAsync(EId entityId, CancellationToken cancellationToken = default);
    Task<bool> ExistAsync(EId entityId, EntityStatus entityStatus, 
                          CancellationToken cancellationToken = default);

    Task<Result<E>> GetByIdAsync(EId entityId, CancellationToken cancellationToken = default);
    Task<Result<E>> GetByIdAsync(EId entityId, EntityStatus entityStatus, 
                                 CancellationToken cancellationToken = default);

    IQueryable<E> Query(Expression<Func<E, bool>> filter);
    IQueryable<E> Query(Expression<Func<E, bool>> filter, EntityStatus entityStatus);

    IQueryable<E> GetAll();
    IQueryable<E> GetAll(EntityStatus entityStatus);
}
