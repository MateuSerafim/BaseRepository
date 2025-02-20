using System.Linq.Expressions;
using BaseRepository.Entities.Base;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.Repositories.Base;
public sealed class ReadOnlyRepository<E, EId>(DbContext context) : 
RepositoryBase<E, EId>(context),
IReadRepository<E, EId>
where E: class, IEntity<EId> 
where EId: struct
{
    public async Task<bool> ExistAsync(EId entityId, CancellationToken cancellationToken = default)
    => await ExistAsync(entityId, EntityStatus.Activated, cancellationToken);

    public async Task<bool> ExistAsync(EId entityId, EntityStatus entityStatus, 
                                       CancellationToken cancellationToken = default)
    => await _dbSet.AnyAsync(e => Equals(e.Id, entityId) 
                               && e.EntityStatus <= entityStatus, cancellationToken);

    public async Task<Result<E>> GetByIdAsync(EId entityId, CancellationToken token = default) 
    => await GetByIdAsync(entityId, EntityStatus.Activated, token);
    public async Task<Result<E>> GetByIdAsync(EId entityId, EntityStatus entityStatus, 
    CancellationToken cancellationToken = default)
    {
        var valueMaybe = await Query(e => Equals(e.Id, entityId), entityStatus)
                              .FirstOrDefaultAsync(cancellationToken);

        return valueMaybe is null 
             ? ErrorResponse.NotFoundError(NotFoundErrorMessage, entityId) 
             : valueMaybe; 
    }

    public IQueryable<E> Query(Expression<Func<E, bool>> filter) 
    => Query(filter, EntityStatus.Activated);
    public IQueryable<E> Query(Expression<Func<E, bool>> filter, EntityStatus entityStatusRestriction)
    => GetAll(entityStatusRestriction).Where(filter);

    public IQueryable<E> GetAll() => GetAll(EntityStatus.Activated);
    public IQueryable<E> GetAll(EntityStatus entityStatusRestriction) 
    => _dbSet.Where(e => e.EntityStatus <= entityStatusRestriction)
             .AsNoTrackingWithIdentityResolution();
}
