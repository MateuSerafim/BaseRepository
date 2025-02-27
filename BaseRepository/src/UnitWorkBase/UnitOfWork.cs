using BaseRepository.Entities.Base;
using BaseRepository.Repositories;
using BaseRepository.Repositories.Base;
using BaseRepository.src.Entities.Base.Actions;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.UnitWorkBase;
public class UnitOfWork(DbContext context) : IUnitOfWork, IDisposable
{
    private readonly DbContext _context = context;
    private bool InvalidState = false;
    private bool Disposed = false;

    public const string DefaultInvalidStateMessage = "Request Database State has invalid.";
    private string InvalidStateMessage = DefaultInvalidStateMessage;

    public IReadRepository<E, EId> ReadOnlyRepository<E, EId>()
    where E : class, IEntity<EId>
    where EId: struct
    => new ReadOnlyRepository<E, EId>(_context);

    public IWriteRepository<E, EId> WriteRepository<E, EId>()
    where E : class, IEntity<EId>
    where EId: struct
    => new Repository<E, EId>(_context);

    public Result<(int, List<IAction>)> Commit()
    {
        if (InvalidState) 
            return ErrorResponse.CriticalError(InvalidStateMessage);
        try
        {
            return (_context.SaveChanges(), GetEntitiesActions());
        }
        catch (Exception ex)
        {
            return ErrorResponse.CriticalError(ex.Message);
        }
    }

    public async Task<Result<(int, List<IAction>)>> CommitAsync(CancellationToken token = default)
    {
        if (InvalidState) 
            return ErrorResponse.CriticalError(InvalidStateMessage);
        try
        {
            var trackedActions = GetEntitiesActions();
            var result = await _context.SaveChangesAsync(token);
            return (result, trackedActions);
        }
        catch (Exception ex)
        {
            return ErrorResponse.CriticalError(ex.Message);
        }
    }

    private List<IAction> GetEntitiesActions()
    => [.. _context.ChangeTracker.Entries<IEntity>().SelectMany(s => s.Entity.Actions)];

    public void InvalidateState(string invalidStateMessage)
    {
        if (!string.IsNullOrEmpty(invalidStateMessage))
            InvalidStateMessage = invalidStateMessage;

        InvalidateState();
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
