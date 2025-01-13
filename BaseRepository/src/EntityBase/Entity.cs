using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.EntityBase;
public abstract class Entity(EntityStatus entityStatus, Guid id) : IEntity<Guid> 
{
    public Guid Id { get; } = id;
    public EntityStatus EntityStatus { get; private set; } = entityStatus;

    public virtual void SetEntityStatus(EntityStatus entityStatus)
    {
        EntityStatus = entityStatus;
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

    public virtual string GetEntityDescription()
    => $"{nameof(Entity)} with Id: {Id}";

}
