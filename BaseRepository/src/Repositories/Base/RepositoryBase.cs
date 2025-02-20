using BaseRepository.Entities.Base;
using BaseUtils.FlowControl.ErrorType;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.Repositories.Base;
public abstract class RepositoryBase<E, EId>(DbContext context)
where E : class, IEntity<EId>
where EId : struct
{
    protected readonly DbSet<E> _dbSet = context.Set<E>();

    public static string NotFoundErrorMessage 
    => $"Entity with Id {ErrorResponse.ReferenceToVariable} not found.";

    public static string PrimaryKeyViolationErrorMessage 
    => $"Operation Error: Entity {ErrorResponse.ReferenceToVariable} cannot be added because Id away exists!";

    public static string InvalidEntityState 
    => $"Entity state {ErrorResponse.ReferenceToVariable} is not valid!";
}
