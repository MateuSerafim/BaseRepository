using System;
using AutoFixture;
using BaseRepository.tests.Domain.Users;

namespace BaseRepository.tests.Entity;
public class EntityTests
{
    [Fact(DisplayName = "ET-1.01: Add Action")]
    public void AddActionTests()
    {
        // Given
        var fixture= new Fixture();
        
        var mockedUser = User.Create(fixture.Create<string>(), 
                                     fixture.Create<string>()).GetValue();
        var mockedAction = new UserAction();

        // When
        mockedUser.AddAction(mockedAction);

        Assert.Single(mockedUser.Actions);
    }
}
