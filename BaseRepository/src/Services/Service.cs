using BaseRepository.Entities.Base;
using BaseRepository.Repositories;
using BaseRepository.UnitWorkBase;

namespace BaseRepository.Services;

public class Service<E, EId>(IUnitOfWork unitOfWork) : IService
where E : class, IEntity<EId>
where EId : struct
{
    private readonly IReadRepository<E, EId> _readRepository = 
    unitOfWork.ReadOnlyRepository<E, EId>();
    private readonly IWriteRepository<E, EId> _writeRepository = 
    unitOfWork.WriteRepository<E, EId>();
}
