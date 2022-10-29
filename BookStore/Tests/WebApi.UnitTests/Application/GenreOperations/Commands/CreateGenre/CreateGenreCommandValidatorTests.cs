using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.CreateGenre;
using WebApi.DBOperations;
using Xunit;

namespace Application.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandValidatorTests
    {
        public CreateGenreCommandValidatorTests() { }

        [Fact]
        public void WhenNullCreateGenreModelIsGiven_Validator_ShouldBeReturnErrors()
        {
            // Given
            CreateGenreCommand command = new CreateGenreCommand(null, null);
            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            // When
            var result = validator.Validate(command);        
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("fo")]
        [InlineData("f")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" fo ")]
        [InlineData(null)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string genreName)
        {
            // Given
            CreateGenreCommand command = new CreateGenreCommand(null, null);
            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            command.Model = new CreateGenreModel
            {
                Name = genreName
            };
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnErrors()
        {
            // Given
            CreateGenreCommand command = new CreateGenreCommand(null, null);
            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            command.Model = new CreateGenreModel
            {
                Name = "Story"
            };
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}