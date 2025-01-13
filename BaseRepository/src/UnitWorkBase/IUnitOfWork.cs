using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.UnitWorkBase;

public interface IUnitOfWork
{
    void InvalidateState();
    Result<int> Commit();
}
