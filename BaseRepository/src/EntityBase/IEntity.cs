using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.EntityBase;
public interface IEntity<EId> where EId : notnull
{
    EId Id { get; }
    EntityStatus EntityStatus { get; }

    Result Activate();
    Result Deactivate();
    Result Remove();

    string GetEntityDescription();
}