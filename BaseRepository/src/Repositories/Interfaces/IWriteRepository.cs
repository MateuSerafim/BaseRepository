using BaseRepository.Entities.Base;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.Repositories;

public interface IWriteRepository<E, EId> : IReadRepository<E, EId>
where E : class, IEntity<EId>
where EId: struct
{
    Task<Result<E>> AddAsync(E entity, CancellationToken token = default);

    Task<Result<E>> ActivateAsync(E entity, CancellationToken token = default);
    Task<Result<E>> ActivateAsync(EId entityId, CancellationToken token = default);

    Task<Result<E>> DeactivateAsync(E entity, CancellationToken token = default);
    Task<Result<E>> DeactivateAsync(EId entityId, CancellationToken token = default);
    
    Task<Result<E>> RemoveAsync(E entity, CancellationToken token = default);
    Task<Result<E>> RemoveAsync(EId entityId, CancellationToken token = default);

    Task<Result<E>> UpdateAsync(E entity, CancellationToken token = default);

    Task<Result> DeleteEntityAsync(E entity, CancellationToken token = default);
    Task<Result> DeleteEntityAsync(EId entityId, CancellationToken token = default);
}
