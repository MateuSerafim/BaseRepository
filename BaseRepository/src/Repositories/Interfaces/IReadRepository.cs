using System.Linq.Expressions;
using BaseRepository.Entities.Base;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.Repositories;
public interface IReadRepository<E, EId> 
where E : class, IEntity<EId>
where EId: struct
{
    Task<bool> ExistAsync(EId entityId, CancellationToken cancellationToken = default);
    Task<bool> ExistAsync(EId entityId, EntityStatus entityStatusRestriction, 
                          CancellationToken cancellationToken = default);

    Task<Result<E>> GetByIdAsync(EId entityId, CancellationToken cancellationToken = default);
    Task<Result<E>> GetByIdAsync(EId entityId, EntityStatus entityStatusRestriction, 
                                 CancellationToken cancellationToken = default);

    IQueryable<E> Query(Expression<Func<E, bool>> filter);
    IQueryable<E> Query(Expression<Func<E, bool>> filter, EntityStatus entityStatusRestriction);

    IQueryable<E> GetAll();
    IQueryable<E> GetAll(EntityStatus entityStatusRestriction);
}
