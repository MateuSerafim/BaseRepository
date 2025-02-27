using AutoFixture;
using BaseRepository.tests.Domain.Libraries;
using BaseRepository.tests.Domain.Users;
using BaseRepository.tests.ProjectMocked.Infrastructure;
using BaseRepository.UnitWorkBase;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.tests.Utils;

public class MockedProgram
{
    public MockedProgram()
    {
        Fixture fixture = new();
        DbContextOptions = new DbContextOptionsBuilder<ProjectContext>()
                        .UseInMemoryDatabase(databaseName: fixture.Create<string>()).Options;
    }
    private DbContextOptions<ProjectContext> DbContextOptions { get; set;}

    public UnitOfWork GetUnitOfWork()
    => new(GetContext());

    public ProjectContext GetContext()
    => new(DbContextOptions);

    public async Task<(Guid, int)> SetBasicDataSet()
    {
        var fixture= new Fixture();

        var unitOfWork = GetUnitOfWork();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var userRepository = unitOfWork.WriteRepository<User, Guid>();
        await userRepository.AddAsync(mockedUser);
    
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(),
                                     fixture.Create<int>());
        var bookRepository = unitOfWork.WriteRepository<Book, int>();
        await bookRepository.AddAsync(mockedBook);

        var mockedLink = BookLink.Create(mockedUser.Id, mockedBook.Id, null);
        var linkRepository = unitOfWork.WriteRepository<BookLink, LinkedUserBook>();
        await linkRepository.AddAsync(mockedLink);

        await unitOfWork.CommitAsync();
        
        return (mockedUser.Id, mockedBook.Id);
    }

    public (UnitOfWork, ProjectContext) GetContextAndUnitOfWork()
    {
        var context = GetContext();

        return (new UnitOfWork(context), context);
    }
}
