using AutoFixture;
using BaseRepository.Repositories.Base;
using BaseRepository.tests.Domain.Libraries;
using BaseRepository.tests.Domain.Users;
using BaseRepository.tests.Utils;
using BaseUtils.FlowControl.ErrorType;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.tests.Repositories;

public class WriteRepositoryTests
{
    [Fact(DisplayName = "WRT-1.01.01: ExistAsync, has entity - Guid")]
    public async Task ExistAsyncMethodGuidTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedUser);
            context.SaveChanges();
        }
        // When
        var existResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<User, Guid>()
                                .ExistAsync(mockedUser.Id);

        // Then
        Assert.True(existResult);
    }

    [Fact(DisplayName = "WRT-1.01.02: ExistAsync, hasn't entity - Guid")]
    public async Task ExistAsyncMethodGuidTest2()
    {
        // Given
        var fixture= new Fixture();
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedUser);
            context.SaveChanges();
        }
        // When
        var existResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<User, Guid>()
                                .ExistAsync(fixture.Create<Guid>());

        // Then
        Assert.False(existResult);
    }
    
    [Fact(DisplayName = "WRT-1.02.01: ExistAsync, has entity - Integer")]
    public async Task ExistAsyncMethodIntTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedBook);
            context.SaveChanges();
        }
        // When
        var existResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<Book, int>()
                                .ExistAsync(mockedBook.Id);

        // Then
        Assert.True(existResult);
    }

    [Fact(DisplayName = "WRT-1.02.02: ExistAsync, hasn't entity - Integer")]
    public async Task ExistAsyncMethodIntTest2()
    {
        // Given
        var fixture= new Fixture();
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());

        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedBook);
            context.SaveChanges();
        }
        // When
        var existResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<Book, int>()
                                .ExistAsync(fixture.Create<int>());

        // Then
        Assert.False(existResult);
    }

    [Fact(DisplayName = "WRT-1.03.01: ExistAsync, has entity - Struct")]
    public async Task ExistAsyncMethodStructTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var ids = await mockedProgram.SetBasicDataSet();

        var linkId = new LinkedUserBook()
        {
            UserId = ids.Item1,
            BookId = ids.Item2
        };

        // When
        var existResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<BookLink, LinkedUserBook>()
                                .ExistAsync(linkId);

        // Then
        Assert.True(existResult);
    }

    [Fact(DisplayName = "WRT-1.03.02: ExistAsync, hasn't entity - Struct")]
    public async Task ExistAsyncMethodStructTest2()
    {
        // Given
        var fixture= new Fixture();

        var mockedProgram = new MockedProgram();

        var linkId = new LinkedUserBook()
        {
            UserId = fixture.Create<Guid>(),
            BookId = fixture.Create<int>()
        };
        // When
        var existResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<BookLink, LinkedUserBook>()
                                .ExistAsync(linkId);

        // Then
        Assert.False(existResult);
    }

    [Fact(DisplayName = "WRT-2.01.01: GetByIdAsync - Guid")]
    public async Task GetByIdAsyncMethodGuidTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedUser);
            context.SaveChanges();
        }
        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<User, Guid>()
                                .GetByIdAsync(mockedUser.Id);

        // Then
        Assert.True(valueResult.IsSuccess);
        Assert.Equal(mockedUser.Id, valueResult.GetValue().Id);
    }

    [Fact(DisplayName = "WRT-2.01.02: GetByIdAsync, hasn't entity - Guid")]
    public async Task GetByIdAsyncMethodGuidTest2()
    {
        // Given
        var fixture= new Fixture();
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var mockedProgram = new MockedProgram();

        var id = fixture.Create<Guid>();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedUser);
            context.SaveChanges();
        }
        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<User, Guid>()
                                .GetByIdAsync(id);

        // Then
        Assert.True(valueResult.IsFailure);
        Assert.Single(valueResult.Errors);
        Assert.Equal(RepositoryBase<User, Guid>
                    .NotFoundErrorMessage
                    .Replace(ErrorResponse.ReferenceToVariable, id.ToString()),
            valueResult.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "WRT-2.02.01: GetByIdAsync - Integer")]
    public async Task GetByIdAsyncMethodIntTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedBook);
            context.SaveChanges();
        }
        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<Book, int>()
                                .GetByIdAsync(mockedBook.Id);

        // Then
        Assert.True(valueResult.IsSuccess);
        Assert.Equal(mockedBook.Id, valueResult.GetValue().Id);
    }

    [Fact(DisplayName = "WRT-2.02.02: GetByIdAsync, hasn't entity - Integer")]
    public async Task GetByIdAsyncMethodIntTest2()
    {
        // Given
        var fixture= new Fixture();
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());

        var mockedProgram = new MockedProgram();

        var id = fixture.Create<int>();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedBook);
            context.SaveChanges();
        }
        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<Book, int>()
                                .GetByIdAsync(id);

        // Then
        Assert.True(valueResult.IsFailure);
        Assert.Single(valueResult.Errors);
        Assert.Equal(RepositoryBase<Book, int>
                    .NotFoundErrorMessage
                    .Replace(ErrorResponse.ReferenceToVariable, id.ToString()),
            valueResult.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "WRT-2.03.01: GetByIdAsync - Struct")]
    public async Task GetByIdAsyncMethodStructTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var ids = await mockedProgram.SetBasicDataSet();

        var linkId = new LinkedUserBook()
        {
            UserId = ids.Item1,
            BookId = ids.Item2
        };

        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<BookLink, LinkedUserBook>()
                                .GetByIdAsync(linkId);

        // Then
        Assert.True(valueResult.IsSuccess);
        Assert.Equal(linkId, valueResult.GetValue().Id);
    }

    [Fact(DisplayName = "WRT-2.03.02: GetByIdAsync, hasn't entity - Struct")]
    public async Task GetByIdAsyncMethodStructTest2()
    {
        // Given
        var fixture= new Fixture();
        var mockedProgram = new MockedProgram();
        _ = await mockedProgram.SetBasicDataSet();

        var linkId = new LinkedUserBook()
        {
            UserId = fixture.Create<Guid>(),
            BookId = fixture.Create<int>()
        };

        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<BookLink, LinkedUserBook>()
                                .GetByIdAsync(linkId);

        // Then
        Assert.True(valueResult.IsFailure);
        Assert.Single(valueResult.Errors);
        Assert.Equal(RepositoryBase<BookLink, LinkedUserBook>
                    .NotFoundErrorMessage
                    .Replace(ErrorResponse.ReferenceToVariable, linkId.ToString()),
            valueResult.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "WRT-3.01.01: Query - Guid")]
    public async Task QueryMethodGuidTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedUser1 = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var mockedUser2 = User.Create(fixture.Create<string>(), 
                                      fixture.Create<string>()).GetValue();
        var mockedUser3 = User.Create(fixture.Create<string>(), 
                                      fixture.Create<string>()).GetValue();
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedUser1);
            context.Add(mockedUser2);
            context.Add(mockedUser3);
            context.SaveChanges();
        }
        List<Guid> listId = [mockedUser1.Id, mockedUser3.Id];

        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<User, Guid>()
                                .Query(u => listId.Contains(u.Id))
                                .ToListAsync();
        var queryIds = valueResult.Select(s => s.Id).ToList();    

        // Then
        Assert.Equal(listId.Count, valueResult.Count);
        Assert.Contains(mockedUser1.Id, queryIds);
        Assert.DoesNotContain(mockedUser2.Id, queryIds);
        Assert.Contains(mockedUser3.Id, queryIds);
    }

    [Fact(DisplayName = "WRT-3.02.01: Query - Integer")]
    public async Task QueryMethodIntTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedBook1 = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedBook2 = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedBook3 = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedBook1);
            context.Add(mockedBook2);
            context.Add(mockedBook3);
            context.SaveChanges();
        }
        List<int> listId = [mockedBook1.Id, mockedBook3.Id];
    
        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<Book, int>()
                                .Query(u => listId.Contains(u.Id))
                                .ToListAsync();
        var queryIds = valueResult.Select(s => s.Id).ToList(); 

        // Then
        Assert.Equal(listId.Count, valueResult.Count);
        Assert.Contains(mockedBook1.Id, queryIds);
        Assert.DoesNotContain(mockedBook2.Id, queryIds);
        Assert.Contains(mockedBook3.Id, queryIds);
    }

    [Fact(DisplayName = "WRT-3.03.01: Query - Struct")]
    public async Task QueryMethodStructTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var ids = await mockedProgram.SetBasicDataSet();

        var linkId = new LinkedUserBook()
        {
            UserId = ids.Item1,
            BookId = ids.Item2
        };

        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<BookLink, LinkedUserBook>()
                                .Query(l => l.Id == linkId)
                                .ToListAsync();

        // Then
        Assert.Single(valueResult);
    }

    [Fact(DisplayName = "WRT-4.01.01: GetAll - Guid")]
    public async Task GelAllMethodGuidTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedUser1 = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var mockedUser2 = User.Create(fixture.Create<string>(), 
                                      fixture.Create<string>()).GetValue();
        var mockedUser3 = User.Create(fixture.Create<string>(), 
                                      fixture.Create<string>()).GetValue();
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedUser1);
            context.Add(mockedUser2);
            context.Add(mockedUser3);
            context.SaveChanges();
        }

        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<User, Guid>()
                                .GetAll()
                                .ToListAsync();

        // Then
        Assert.Equal(valueResult.Count, valueResult.Count);
    }

    [Fact(DisplayName = "WRT-4.02.01: GetAll - Integer")]
    public async Task GetAllMethodIntTest1()
    {
        // Given
        var fixture= new Fixture();
        var mockedBook1 = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedBook2 = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedBook3 = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());
        var mockedProgram = new MockedProgram();

        using (var context = mockedProgram.GetContext())
        {
            context.Add(mockedBook1);
            context.Add(mockedBook2);
            context.Add(mockedBook3);
            context.SaveChanges();
        }
    
        // When
        var valueResult = await mockedProgram
                                .GetUnitOfWork()
                                .WriteRepository<Book, int>()
                                .GetAll()
                                .ToListAsync();

        // Then
        Assert.Equal(valueResult.Count, valueResult.Count);
    }

    [Fact(DisplayName = "WRT-4.03.01: GetAll - Struct")]
    public async Task GetAllMethodStructTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        _ = await mockedProgram.SetBasicDataSet();

        // When
        var valueResult = await mockedProgram.GetUnitOfWork()
                                       .WriteRepository<BookLink, LinkedUserBook>()
                                       .GetAll().ToListAsync();

        // Then
        Assert.Single(valueResult);
    }

    [Fact(DisplayName = "WRT-5.01.01: AddAsync - Guid")]
    public async Task AddAsyncMethodGuidTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();
        var userRepository = unitOfWork.WriteRepository<User, Guid>();

        var fixture= new Fixture();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();

        // When        
        var result = await userRepository.AddAsync(mockedUser);

        // Then
        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "WRT-5.01.02: AddAsync, primary key error - Guid")]
    public async Task AddAsyncMethodGuidTest2()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();
        var userRepository = unitOfWork.WriteRepository<User, Guid>();

        var fixture= new Fixture();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();

        // When        
        _ = await userRepository.AddAsync(mockedUser);
        await unitOfWork.CommitAsync();

        var result = await userRepository.AddAsync(mockedUser);

        // Then
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Equal(RepositoryBase<User, Guid>
            .PrimaryKeyViolationErrorMessage
            .Replace(ErrorResponse.ReferenceToVariable, 
                     mockedUser.GetEntityDescription()), 
            result.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "WRT-5.02.01: AddAsync - Integer")]
    public async Task AddAsyncMethodIntTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();
        var userRepository = unitOfWork.WriteRepository<Book, int>();

        var fixture= new Fixture();
        
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());

        // When        
        var result = await userRepository.AddAsync(mockedBook);

        // Then
        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "WRT-5.02.02: AddAsync, primary key error - Integer")]
    public async Task AddAsyncMethodIntTest2()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();
        var userRepository = unitOfWork.WriteRepository<Book, int>();

        var fixture= new Fixture();
        
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());

        // When        
        _ = await userRepository.AddAsync(mockedBook);
        await unitOfWork.CommitAsync();

        var result = await userRepository.AddAsync(mockedBook);

        // Then
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Equal(RepositoryBase<User, Guid>
            .PrimaryKeyViolationErrorMessage
            .Replace(ErrorResponse.ReferenceToVariable, 
                     mockedBook.GetEntityDescription()), 
            result.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "WRT-5.03.01: AddAsync - Struct")]
    public async Task AddAsyncMethodStructTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();

        var userRepository = unitOfWork.WriteRepository<User, Guid>();
        var bookRepository = unitOfWork.WriteRepository<Book, int>();
        var linkRepository = unitOfWork.WriteRepository<BookLink, LinkedUserBook>();

        var fixture= new Fixture();

        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();      
        await userRepository.AddAsync(mockedUser);
        
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());                              
        await bookRepository.AddAsync(mockedBook);

        // When
        var mockedLink = BookLink.Create(mockedUser.Id, mockedBook.Id, null);
        var result = await linkRepository.AddAsync(mockedLink);

        // Then
        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "WRT-5.03.02: AddAsync, primary key error - Struct")]
    public async Task AddAsyncMethodStructTest2()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();

        var userRepository = unitOfWork.WriteRepository<User, Guid>();
        var bookRepository = unitOfWork.WriteRepository<Book, int>();
        var linkRepository = unitOfWork.WriteRepository<BookLink, LinkedUserBook>();

        var fixture= new Fixture();

        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();      
        await userRepository.AddAsync(mockedUser);
        
        var mockedBook = Book.Create(fixture.Create<string>(), 
                                     fixture.Create<string>(),
                                     fixture.Create<int>(), 
                                     fixture.Create<int>());                              
        await bookRepository.AddAsync(mockedBook);

        var mockedLink = BookLink.Create(mockedUser.Id, mockedBook.Id, null);
        await linkRepository.AddAsync(mockedLink);

        await unitOfWork.CommitAsync();

        // When
        var result = await linkRepository.AddAsync(mockedLink);

        // Then
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Equal(RepositoryBase<User, Guid>
            .PrimaryKeyViolationErrorMessage
            .Replace(ErrorResponse.ReferenceToVariable, 
                     mockedLink.GetEntityDescription()), 
            result.Errors[0].ErrorMessage());
    }
}
