using BaseRepository.ContextBase;
using BaseRepository.EntityBase;
using BaseRepository.RepositoryBase;
using BaseRepository.src.RepositoryBase;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.UnitWorkBase;
public class UnitOfWork(BaseDbContext context) : IUnitOfWork, IDisposable
{

    private readonly BaseDbContext _context = context;
    private bool InvalidState = false;
    private bool Disposed = false;


    public virtual IGenericRepository Repository<E>(bool repositoryReadOnly = false)
    where E : class, IEntity<Guid>
    {
        return repositoryReadOnly ? new ReadOnlyRepository<E>(_context) : new Repository<E>(_context);
    }

    public Result<int> Commit()
    {
        if (InvalidState) 
            return ErrorResponse.CriticalError();
        try
        {
            return _context.SaveChanges();
        }
        catch (Exception ex)
        {
            return ErrorResponse.CriticalError(ex.Message);
        }
    }

    public async Task<Result<int>> CommitAsync(CancellationToken token = default)
    {
        if (InvalidState) 
            return ErrorResponse.CriticalError();
        try
        {
            return await _context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            return ErrorResponse.CriticalError(ex.Message);
        }
    }

    public void InvalidateState() => InvalidState = true;

    protected virtual void Dispose(bool dispose)
    {
        if (!Disposed && dispose)
        {
            _context.Dispose();
            Disposed = true;
        }  
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
