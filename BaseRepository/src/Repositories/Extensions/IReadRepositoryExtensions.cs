using BaseRepository.Entities.Base;

namespace BaseRepository.Repositories.Extensions;
public static class IReadRepositoryExtensions
{
    public static async Task<bool> NotExist<R, E, EId>(
        this R repository, EId entityId, CancellationToken token = default) 
    where R: class, IReadRepository<E, EId>
    where E: class, IEntity<EId>
    where EId: struct
    {
        return !await repository.ExistAsync(entityId, token);
    }

    public static async Task<bool> NotExist<R, E, EId>(
        this R repository, EId entityId, EntityStatus entityStatus, 
        CancellationToken token = default) 
    where R: class, IReadRepository<E, EId>
    where E: class, IEntity<EId>
    where EId: struct
    {
        return !await repository.ExistAsync(entityId, entityStatus, token);
    }
}
