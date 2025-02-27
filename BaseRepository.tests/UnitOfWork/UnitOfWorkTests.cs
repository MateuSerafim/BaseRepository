using AutoFixture;
using BaseRepository.tests.Domain.Users;
using BaseRepository.tests.Utils;
using BaseRepository.UnitWorkBase;
using BaseUtils.FlowControl.ErrorType;

namespace BaseRepository.tests;
public class UnitOfWorkTests
{
    [Fact(DisplayName = "UWT-1.01: commit - successfull")]
    public async Task UnitOfWorkCommitTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();
        var userRepository = unitOfWork.WriteRepository<User, Guid>();

        var fixture= new Fixture();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();

        // When        
        await userRepository.AddAsync(mockedUser);

        var resultCommit = unitOfWork.Commit();
        var savingCheck = await userRepository.ExistAsync(mockedUser.Id);

        unitOfWork.Dispose();

        // Then
        Assert.True(resultCommit.IsSuccess);
        Assert.Equal(1, resultCommit.GetValue().Item1);
        Assert.Empty(resultCommit.GetValue().Item2);

        Assert.True(savingCheck);
    }

    [Fact(DisplayName = "UWT-1.02: commit - Invalidate state")]
    public void UnitOfWorkCommitTest2()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();

        // When
        unitOfWork.InvalidateState();

        var resultCommit = unitOfWork.Commit();

        // Then
        Assert.True(resultCommit.IsFailure);
        Assert.Single(resultCommit.Errors);
        Assert.Equal(ErrorTypeEnum.CriticalError, resultCommit.Errors[0].ErrorType);
        Assert.Equal(UnitOfWork.DefaultInvalidStateMessage, 
                    resultCommit.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "UWT-1.03: commit - Invalidate state with message")]
    public void UnitOfWorkCommitTest3()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();

        var fixture= new Fixture();

        var messageError = fixture.Create<string>();

        // When
        unitOfWork.InvalidateState(messageError);

        var resultCommit = unitOfWork.Commit();

        // Then
        Assert.True(resultCommit.IsFailure);
        Assert.Single(resultCommit.Errors);
        Assert.Equal(ErrorTypeEnum.CriticalError, resultCommit.Errors[0].ErrorType);
        Assert.Equal(messageError, 
                    resultCommit.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "UWT-1.04: commit - error critical")]
    public void UnitOfWorkCommitTest4()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var references = mockedProgram.GetContextAndUnitOfWork();

        var fixture= new Fixture();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        references.Item2.Add(mockedUser);
        references.Item2.SaveChanges();

        references.Item2.Add(mockedUser);

        // When
        var resultCommit = references.Item1.Commit();

        // Then
        Assert.True(resultCommit.IsFailure);
        Assert.Single(resultCommit.Errors);
        Assert.Equal(ErrorTypeEnum.CriticalError, resultCommit.Errors[0].ErrorType);
    }

    [Fact(DisplayName = "UWT-2.01: commit async - successfull")]
    public async Task UnitOfWorkCommitAsyncTest1()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();
        var userRepository = unitOfWork.WriteRepository<User, Guid>();

        var fixture= new Fixture();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();

        // When        
        await userRepository.AddAsync(mockedUser);

        var resultCommit = await unitOfWork.CommitAsync();
        var savingCheck = await userRepository.ExistAsync(mockedUser.Id);

        // Then
        Assert.True(resultCommit.IsSuccess);
        Assert.Equal(1, resultCommit.GetValue().Item1);
        Assert.Empty(resultCommit.GetValue().Item2);

        Assert.True(savingCheck);
    }

    [Fact(DisplayName = "UWT-2.02: commit async - Invalidate state")]
    public async Task UnitOfWorkCommitAsyncTest2()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var unitOfWork = mockedProgram.GetUnitOfWork();

        // When
        unitOfWork.InvalidateState();

        var resultCommit = await unitOfWork.CommitAsync();

        // Then
        Assert.True(resultCommit.IsFailure);
        Assert.Single(resultCommit.Errors);
        Assert.Equal(ErrorTypeEnum.CriticalError, resultCommit.Errors[0].ErrorType);
        Assert.Equal(UnitOfWork.DefaultInvalidStateMessage, 
                    resultCommit.Errors[0].ErrorMessage());
    }

    [Fact(DisplayName = "UWT-1.03: commit async - error critical")]
    public async Task UnitOfWorkCommitAsyncTest3()
    {
        // Given
        var mockedProgram = new MockedProgram();
        var references = mockedProgram.GetContextAndUnitOfWork();

        var fixture= new Fixture();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        references.Item2.Add(mockedUser);
        references.Item2.SaveChanges();

        references.Item2.Add(mockedUser);

        // When
        var resultCommit = await references.Item1.CommitAsync();

        // Then
        Assert.True(resultCommit.IsFailure);
        Assert.Single(resultCommit.Errors);
        Assert.Equal(ErrorTypeEnum.CriticalError, resultCommit.Errors[0].ErrorType);
    }
}
