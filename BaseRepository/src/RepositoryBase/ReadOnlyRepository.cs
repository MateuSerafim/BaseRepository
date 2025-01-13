using BaseRepository.EntityBase;
using BaseRepository.ContextBase;
using Microsoft.EntityFrameworkCore;
using BaseUtils.FlowControl.ResultType;
using BaseUtils.FlowControl.ErrorType;
using System.Linq.Expressions;

namespace BaseRepository.RepositoryBase;

public class ReadOnlyRepository<E>(BaseDbContext context) : IReadRepository<E, Guid>
where E: class, IEntity<Guid> 
{
    private readonly DbSet<E> _dbSet = context.Set<E>();

    public const string NotFoundErrorMessage = 
    $"Entity with Id {ErrorResponse.ReferenceToVariable} not found.";

    public async Task<bool> ExistAsync(Guid entityId, CancellationToken cancellationToken = default)
        => await ExistAsync(entityId, EntityStatus.Activated, cancellationToken);

    public async Task<bool> ExistAsync(Guid entityId, EntityStatus entityStatus, 
                                       CancellationToken cancellationToken = default)
        => await _dbSet.AnyAsync(e => e.Id == entityId && e.EntityStatus <= entityStatus, 
                                 cancellationToken);

    public async Task<Result<E>> GetByIdAsync(Guid entityId, CancellationToken token = default) 
    => await GetByIdAsync(entityId, EntityStatus.Activated, token);
    public async Task<Result<E>> GetByIdAsync(Guid entityId, EntityStatus entityStatus, 
    CancellationToken cancellationToken = default)
    {
        var valueMaybe = await Query(e => e.Id == entityId, entityStatus)
                              .FirstOrDefaultAsync(cancellationToken);

        return valueMaybe is null 
                ? ErrorResponse.NotFoundError(NotFoundErrorMessage, entityId) 
                : valueMaybe; 
    }

    public IQueryable<E> Query(Expression<Func<E, bool>> filter) 
    => Query(filter, EntityStatus.Activated);
    public IQueryable<E> Query(Expression<Func<E, bool>> filter, EntityStatus entityStatus)
    => GetAll(entityStatus).Where(filter);

    public IQueryable<E> GetAll() => GetAll(EntityStatus.Activated);
    public IQueryable<E> GetAll(EntityStatus entityStatus) 
    => _dbSet.Where(e => e.EntityStatus <= entityStatus).AsNoTrackingWithIdentityResolution();
}
