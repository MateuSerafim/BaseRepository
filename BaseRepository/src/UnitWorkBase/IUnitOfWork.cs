using BaseRepository.Entities.Base;
using BaseRepository.Repositories;
using BaseRepository.src.Entities.Base.Actions;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.UnitWorkBase;
public interface IUnitOfWork
{
    IReadRepository<E, EId> ReadOnlyRepository<E, EId>()
    where E: class, IEntity<EId>
    where EId: struct;
    IWriteRepository<E, EId> WriteRepository<E, EId>()
    where E: class, IEntity<EId>
    where EId: struct;
    
    void InvalidateState();

    Result<(int, List<IAction>)> Commit();
    Task<Result<(int, List<IAction>)>> CommitAsync(CancellationToken token = default);
}
