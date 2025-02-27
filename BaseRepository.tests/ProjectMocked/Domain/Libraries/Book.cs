using BaseRepository.Entities.Base;
using BaseUtils.FlowControl.ErrorType;
using BaseUtils.FlowControl.ResultType;

namespace BaseRepository.tests.Domain.Libraries;
public class Book : Entity<int>
{
    public string Name { get; private set; }
    public string Author { get; private set; }
    public int YearOfPublication { get; private set; }

    public bool Disponible { get; private set; } = true;

    public virtual List<BookLink> BookLinks {get; private set; } = []; 

    protected Book() : base()
    {

    }

    private Book(string name, string author, int yearOfPublication, 
    EntityStatus entityStatus, int id = default): base(entityStatus, id)
    {
        Name = name;
        Author = author;
        YearOfPublication = yearOfPublication;
    }

    public static Book Create(string name, string author, int yearOfPublication, int id)
    => new (name, author, yearOfPublication, EntityStatus.Activated, id);

    public override Result Deactivate()
    {
        return ErrorResponse.InvalidOperationError();
    }
}
