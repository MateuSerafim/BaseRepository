using BaseRepository.Entities.Base;
using BaseRepository.tests.Domain.Libraries;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.tests.Domain.Users;
public class User : Entity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }

    public List<BookLink> BookLinks{ get; private set; } = [];

    public List<Book> RentedBooks 
    => [.. BookLinks.Where(i => i.EntityStatus == EntityStatus.Activated).Select(s => s.Book)];

    protected User() : base()
    {

    }

    private User(string name, string email, EntityStatus entityStatus, Guid id) : 
    base(entityStatus, id)
    {
        Name = name;
        Email = email;
    }

    public static Result<User> Create(string name, string email)
    {
        return new User(name, email, EntityStatus.Activated, Guid.CreateVersion7());
    }

    public void SetName(string name) 
    {
        Name = name;
    }
}
