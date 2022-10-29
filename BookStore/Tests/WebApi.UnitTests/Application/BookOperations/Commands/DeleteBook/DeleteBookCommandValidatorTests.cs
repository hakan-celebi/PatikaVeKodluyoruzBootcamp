using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.DeleteBook;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandValidatorTests
    {
        public DeleteBookCommandValidatorTests() { }

        [Fact]
        public void WhenBookIdIsNotGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            DeleteBookCommand command = new DeleteBookCommand(null);
            DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
            command.Id = default;
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidBookIdIsGiven_Validation_ShouldNotBeReturnErrors()
        {
            // Given
            DeleteBookCommand command = new DeleteBookCommand(null);
            DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
            command.Id = 5;
            // When
             var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}