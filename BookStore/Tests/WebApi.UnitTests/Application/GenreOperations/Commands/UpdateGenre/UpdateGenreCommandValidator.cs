using FluentAssertions;
using WebApi.Application.GenreOperations.Commands.UpdateGenre;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidatorTests
    {
        public UpdateGenreCommandValidatorTests() {}

        [Theory]
        [InlineData(default)]
        [InlineData(-5)]
        public void WhenInvalidGenreIdIsGiven_Validator_ShouldBeReturnErrors(int genreId)
        {
            // Given
            UpdateGenreCommand command = new UpdateGenreCommand(null, null);
            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            command.Id = genreId;
            command.Model = new UpdateGenreModel
            {
                Name = "Story"
            };
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Select(x => x.PropertyName).Should().Contain("Id");
        }

        [Fact]
        public void WhenNullModelIsGiven_Validator_ShouldBeReturnErrors()
        {
            // Given
            UpdateGenreCommand command = new UpdateGenreCommand(null, null);
            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            command.Id = 2;
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Select(x => x.PropertyName).Should().Contain("Model");
        }

        [Theory]
        [InlineData("nul")]
        [InlineData("nu")]
        [InlineData("n")]
        [InlineData("nul   ")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string genreName)
        {
            // Given
            UpdateGenreCommand command = new UpdateGenreCommand(null, null);
            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            command.Id = 2;
            command.Model = new UpdateGenreModel
            {
                Name = genreName
            };
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Select(x => x.PropertyName).Should().Contain("Model.Name");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnErrors(string genreName)
        {
            // Given
            UpdateGenreCommand command = new UpdateGenreCommand(null, null);
            UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
            command.Id = 2;
            command.Model = new UpdateGenreModel
            {
                Name = genreName
            };
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}