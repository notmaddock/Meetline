using System.Threading;
using System.Threading.Tasks;
using Application.Common.PipelineBehaviors;
using Application.Errors;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using Mediator;
using Moq;
using Xunit;

namespace Application.Tests.Common.PipelineBehaviors;

[TestSubject(typeof(ValidationBehavior<,>))]
public class ValidationBehaviorTest
{
    [Fact(DisplayName = "Should return validation errors and NOT call next when validation fails")]
    public async Task Handle_WhenValidationFails_ShouldReturnErrors()
    {
        var validatorMock = new Mock<IValidator<TestCommand>>();
        var validationFailure = new ValidationFailure("PropertyName", "ErrorMessage")
        {
            ErrorCode = "Validation.Error"
        };

        validatorMock
            .Setup(v => v.Validate(It.IsAny<ValidationContext<TestCommand>>()))
            .Returns(new ValidationResult([validationFailure]));

        var behavior = new ValidationBehavior<TestCommand, Result>([validatorMock.Object]);
        var nextMock = new Mock<MessageHandlerDelegate<TestCommand, Result>>();

        var result = await behavior.Handle(new TestCommand(), nextMock.Object, TestContext.Current.CancellationToken);

        Assert.True(result.IsFailed);
        var error = Assert.Single(result.Errors);
        Assert.IsType<ApplicationError>(error);
        Assert.Equal("Validation.Error", ((ApplicationError)error).Code);

        nextMock.Verify(n => n(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Should call next and return success when validation passes")]
    public async Task Handle_WhenValidationSucceeds_ShouldCallNext()
    {
        var validatorMock = new Mock<IValidator<TestCommand>>();
        validatorMock
            .Setup(v => v.Validate(It.IsAny<ValidationContext<TestCommand>>()))
            .Returns(new ValidationResult());

        var behavior = new ValidationBehavior<TestCommand, Result>([validatorMock.Object]);
        var nextMock = new Mock<MessageHandlerDelegate<TestCommand, Result>>();
        nextMock.Setup(n => n(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok());

        var result = await behavior.Handle(new TestCommand(), nextMock.Object, TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);
        nextMock.Verify(n => n(It.IsAny<TestCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // ReSharper disable once MemberCanBePrivate.Global (this is required for Moq)
    public record TestCommand : ICommand<Result>;
}