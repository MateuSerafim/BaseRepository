using BaseRepository.Entities.Base;
using BaseRepository.tests.Domain.Users;

namespace BaseRepository.tests.Domain.Libraries;

public class BookLink : Entity<LinkedUserBook>
{
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }

    public int BookId { get; private set; }
    public virtual Book Book { get; private set; }

    public DateTime ExpiredDate { get; private set; }

    public override LinkedUserBook Id 
    {
        get => new ()
        {
            UserId = UserId,
            BookId = BookId,
        };
    }

    public bool IsLate => ExpiredDate > DateTime.UtcNow;

    protected BookLink(EntityStatus entityStatus, LinkedUserBook Id):
    base(entityStatus, Id)
    {
        UserId = Id.UserId;
        BookId = Id.BookId;
    }

    public static BookLink Create(Guid userId, int bookId, DateTime? expiredDate)
    {
        return new(EntityStatus.Activated, new(){UserId = userId, BookId = bookId})
        {
            ExpiredDate = expiredDate ?? DateTime.UtcNow,
        };
    }
}

public struct LinkedUserBook : IEquatable<LinkedUserBook>
{
    public Guid UserId { get; set; }
    public int BookId { get;  set;}

    public LinkedUserBook()
    {

    }

    public override readonly bool Equals(object? obj)
    => obj is not null && obj is LinkedUserBook other && Equals(other);

    public readonly bool Equals(LinkedUserBook value) 
    => UserId == value.UserId && BookId == value.BookId;

    public override readonly int GetHashCode() 
    => HashCode.Combine(UserId, BookId);

    
    public static bool operator == (LinkedUserBook left, LinkedUserBook right)
    => left.Equals(right);

    public static bool operator != (LinkedUserBook left, LinkedUserBook right)
    => !(left == right);
}
