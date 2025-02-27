using BaseRepository.Entities.Base;
using BaseRepository.Repositories.Base;
using BaseUtils.FlowControl.ResultType;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.ProjectMocked.Repositories;

public class RepositoryTestMocked<E, EId>(DbContext context): 
                       Repository<E, EId>(context)
where E: class, IEntity<EId>
where EId: struct 
{
    public Result<E> InvalidModifyEntityState(E entity)
    => ModifyEntityState(entity, (EntityStatus)4);
}
