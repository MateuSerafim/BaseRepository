using System.Linq.Expressions;
using BaseRepository.Entities.Base;
using BaseRepository.Repositories.Extensions;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.Repositories.Base;

public class Repository<E, EId>(DbContext context) : 
RepositoryBase<E, EId>(context), IWriteRepository<E, EId>
where E: class, IEntity<EId>
where EId: struct 
{
    public async Task<Result<E>> AddAsync(E entity, CancellationToken token = default)
    {
        if (await ExistAsync(entity.Id, token)) 
            return ErrorResponse.InvalidOperationError(PrimaryKeyViolationErrorMessage, 
                    entity.GetEntityDescription());
        
        await _dbSet.AddAsync(entity, token);

        return entity;
    }
    
    public async Task<Result<E>> ActivateAsync(EId entityId, CancellationToken token = default)
    {
        var entityResult = await GetByIdAsync(entityId, EntityStatus.Removed, token);
        if (entityResult.IsFailure) 
            return entityResult;

        return ModifyEntityState(entityResult.GetValue(), EntityStatus.Activated);
    }

    public async Task<Result<E>> ActivateAsync(E entity, CancellationToken token = default)
    {
        if (await this.NotExist<Repository<E, EId>, E, EId>(entity.Id, EntityStatus.Removed, token))
            return ErrorResponse.NotFoundError(NotFoundErrorMessage, entity.Id);

        return ModifyEntityState(entity, EntityStatus.Activated);
    }

    public async Task<Result<E>> DeactivateAsync(EId entityId, CancellationToken token = default)
    {
        var entityResult = await GetByIdAsync(entityId, EntityStatus.Removed, token);
        if (entityResult.IsFailure) 
            return entityResult;

        return ModifyEntityState(entityResult.GetValue(), EntityStatus.Deactivated);
    }

    public async Task<Result<E>> DeactivateAsync(E entity, CancellationToken token = default)
    {
        if (await this.NotExist<Repository<E, EId>, E, EId>(entity.Id, EntityStatus.Removed, token))
            return ErrorResponse.NotFoundError(NotFoundErrorMessage, entity.Id);

        return ModifyEntityState(entity, EntityStatus.Deactivated);
    }

    public async Task<Result<E>> RemoveAsync(EId entityId, CancellationToken token = default)
    {
        var entityResult = await GetByIdAsync(entityId, EntityStatus.Removed, token);
        if (entityResult.IsFailure) 
            return entityResult;

        return ModifyEntityState(entityResult.GetValue(), EntityStatus.Removed);
    }

    public async Task<Result<E>> RemoveAsync(E entity, CancellationToken token = default)
    {
        if (await this.NotExist<Repository<E, EId>, E, EId>(entity.Id, EntityStatus.Removed, token))
            return ErrorResponse.NotFoundError(NotFoundErrorMessage, entity.Id);

        return ModifyEntityState(entity, EntityStatus.Removed);
    }

    protected Result<E> ModifyEntityState(E entity, EntityStatus newEntityStatus)
    {
        var changeStatusResult = newEntityStatus switch
        {
            EntityStatus.Activated => entity.Activate(),
            EntityStatus.Deactivated => entity.Deactivate(),
            EntityStatus.Removed => entity.Remove(),
            _ => ErrorResponse.InvalidOperationError(InvalidEntityState, newEntityStatus)
        };

        if (changeStatusResult.IsFailure)
            return changeStatusResult.Errors;

        _dbSet.Update(entity);

        return entity;
    }

    public async Task<Result<E>> UpdateAsync(E entity, CancellationToken token = default)
    {
        if (await this.NotExist<Repository<E, EId>, E, EId>(entity.Id, token))
            return ErrorResponse.NotFoundError(NotFoundErrorMessage, entity.Id);

        _dbSet.Update(entity);

        return entity;
    }

    public async Task<Result> DeleteEntityAsync(E entity, CancellationToken token = default)
    {
        if (await this.NotExist<Repository<E, EId>, E, EId>(entity.Id, EntityStatus.Removed, token))
            return ErrorResponse.NotFoundError(NotFoundErrorMessage, entity.Id);

        _dbSet.Remove(entity);

        return Result.Success();
    }

    public async Task<Result> DeleteEntityAsync(EId entityId, CancellationToken token = default)
    {
        var entityResult = await GetByIdAsync(entityId, EntityStatus.Removed, token);
        if (entityResult.IsFailure) 
            return entityResult.Errors;

        _dbSet.Remove(entityResult.GetValue());

        return Result.Success();
    }

    public async Task<bool> ExistAsync(EId entityId, CancellationToken cancellationToken = default)
    => await ExistAsync(entityId, EntityStatus.Activated, cancellationToken);
    public async Task<bool> ExistAsync(EId entityId, EntityStatus entityStatus, 
                                        CancellationToken cancellationToken = default)
    => await _dbSet.AnyAsync(e => Equals(e.Id, entityId) && e.EntityStatus <= entityStatus, 
                             cancellationToken);

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
    public IQueryable<E> Query(Expression<Func<E, bool>> filter, EntityStatus entityStatus)
    => GetAll(entityStatus).Where(filter);

    public IQueryable<E> GetAll() => GetAll(EntityStatus.Activated);
    public IQueryable<E> GetAll(EntityStatus entityStatus) 
    => _dbSet.Where(e => e.EntityStatus <= entityStatus);
}
