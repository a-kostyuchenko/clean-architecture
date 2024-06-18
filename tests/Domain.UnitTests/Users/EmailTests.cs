using Domain.Users;
using Domain.Users.Errors;
using Domain.Users.ValueObjects;
using FluentAssertions;
using SharedKernel;
using SharedKernel.Result;

namespace Domain.UnitTests.Users;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Email_Should_ReturnError_WhenValueIsInvalid(string? value)
    {
        // Act
        Result<Email> emailResult = Email.Create(value!);

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
        Result<Email> emailResult = Email.Create(value);

        // Assert
        emailResult.IsSuccess.Should().BeFalse();
        emailResult.Error.Should().Be(EmailErrors.InvalidFormat);
    }
}
