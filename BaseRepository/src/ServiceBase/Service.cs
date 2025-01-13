using BaseRepository.EntityBase;
using BaseRepository.RepositoryBase;
using BaseRepository.UnitWorkBase;

namespace BaseRepository.src.ServiceBase;

public class Service<T>(UnitOfWork unitOfWork) where T : class, IEntity<Guid>
{
    private readonly IGenericRepository _readRepository = unitOfWork.Repository<T>(true);
    private readonly IGenericRepository _repository = unitOfWork.Repository<T>();
}
