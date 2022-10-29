using FluentAssertions;
using WebApi.Application.AuthorOperations.Commands.DeleteAuthor;
using Xunit;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandValidatorTests
    {
        public DeleteAuthorCommandValidatorTests() { }

        [Fact]
        public void WhenAuthorIdIsNotGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            DeleteAuthorCommand command = new DeleteAuthorCommand(null);
            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            command.Id = default;
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidAuthorIdIsGiven_Validation_ShouldNotBeReturnErrors()
        {
            // Given
            DeleteAuthorCommand command = new DeleteAuthorCommand(null);
            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            command.Id = 5;
            // When
             var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}