using FluentAssertions;
using WebApi.Application.GenreOperations.Commands.DeleteGenre;
using Xunit;

namespace Application.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandValidatorTests
    {
        public DeleteGenreCommandValidatorTests() { }

        [Fact]
        public void WhenGenreIdIsNotGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            command.Id = default;
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidGenreIdIsGiven_Validation_ShouldNotBeReturnErrors()
        {
            // Given
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            command.Id = 5;
            // When
             var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}