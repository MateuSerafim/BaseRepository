using BaseRepository.src.Entities.Base.Actions;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.Entities.Base;

public interface IEntity<ID>: IEntity where ID: struct
{
    ID Id {get;}
}

public interface IEntity
{
    EntityStatus EntityStatus { get; }
    List<IAction> Actions { get; }

    Result Activate();
    Result Deactivate();
    Result Remove();

    void AddAction(IAction action);

    string GetEntityDescription();
}