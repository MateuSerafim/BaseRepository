using System.ComponentModel.DataAnnotations.Schema;
using BaseRepository.src.Entities.Base.Actions;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.Entities.Base;
public abstract class Entity<ID> : IEntity<ID>
where ID : struct
{
    public virtual ID Id { get; private set; }
    public EntityStatus EntityStatus { get; private set; }

    [NotMapped]
    public List<IAction> Actions { get; private set; } = [];

    protected Entity()
    {

    }

    protected Entity(EntityStatus entityStatus, ID id)
    {
        EntityStatus = entityStatus;
        Id = id;
    }

    public virtual Result Activate()
    {
        EntityStatus = EntityStatus.Activated;
        return Result.Success();
    }

    public virtual Result Deactivate()
    {
        EntityStatus = EntityStatus.Deactivated;
        return Result.Success();
    }

    public virtual Result Remove()
    {
        EntityStatus = EntityStatus.Removed;
        return Result.Success();
    }

    public void AddAction(IAction action)
    {
        Actions.Add(action);
    }

    public virtual string GetEntityDescription()
        => $"{nameof(Entity<ID>)} with Id: {Id}";

}
