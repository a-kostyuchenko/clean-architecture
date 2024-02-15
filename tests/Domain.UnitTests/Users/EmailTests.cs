using Domain.Users;
using FluentAssertions;

namespace Domain.UnitTests.Users;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Email_Should_ReturnError_WhenValueIsInvalid(string? value)
    {
        // Act
        var emailResult = Email.Create(value);

        // Assert
        emailResult.IsSuccess.Should().BeFalse();
        emailResult.Error.Should().Be(EmailErrors.NullOrEmpty);
    }
    
    [Theory]
    [InlineData("test@test@test.com")]
    [InlineData("test.com")]
    [InlineData("test.com@gmail")]
    public void Email_Should_ReturnError_WhenValueIsNotMatchingPattern(string value)
    {
        // Act
        var emailResult = Email.Create(value);

        // Assert
        emailResult.IsSuccess.Should().BeFalse();
        emailResult.Error.Should().Be(EmailErrors.InvalidFormat);
    }
}