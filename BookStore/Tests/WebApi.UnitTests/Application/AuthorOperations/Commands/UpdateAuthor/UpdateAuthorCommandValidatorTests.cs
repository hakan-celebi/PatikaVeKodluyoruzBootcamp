using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;
using Xunit;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidatorTests
    {
        public UpdateAuthorCommandValidatorTests() { }

        [Fact]
        public void WhenAuthorIdIsNotGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            command.Id = default;
            command.Model = new UpdateAuthorModel();
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
        }

        [Fact]
        public void WhenValidAuthorIdIsGiven_Validation_ShouldNotBeReturnErrors()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            command.Id = 5;
            command.Model = new UpdateAuthorModel();
            // When
             var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void WhenNullModelIsGiven_Validator_ShouldBeReturnError()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            command.Id = 1;
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Any(x => x.PropertyName == "Model").Should().BeTrue();
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            command.Model = new UpdateAuthorModel
            {
                FirstName = "s",
                MiddleName = "a",
                LastName = "t",
                DateOfBirth = DateTime.Now.AddDays(5)
            };
            // When
            var result = validator.Validate(command);
            // Then
            var errorPropertyNameList = result.Errors.Select(x => x.PropertyName);
            result.Errors.Count.Should().BeGreaterThan(0);            
            errorPropertyNameList.Should().Contain("Model.MiddleName");
            errorPropertyNameList.Should().Contain("Model.DateOfBirth");
            errorPropertyNameList.Should().Contain("Model.FirstName");
            errorPropertyNameList.Should().Contain("Model.LastName");
        }

        [Fact]
        public void WhenDateOfBirthIsGivenAsEqualOrBiggerThanToday_Validator_ShouldBeReturn()
        {
            // Given
             UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            command.Model = new UpdateAuthorModel
            {
                FirstName = "Hakan",
                MiddleName = default,
                LastName = "ÇELEBİ",
                DateOfBirth = DateTime.Now
            };
            // When
            var result = validator.Validate(command);
            // Then
            var errorPropertyNameList = result.Errors.Select(x => x.PropertyName);
            result.Errors.Count.Should().BeGreaterThan(0);  
            errorPropertyNameList.Should().Contain("Model.DateOfBirth");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnErrors()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(null, null);
            UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
            command.Id = 1;
            command.Model = new UpdateAuthorModel
            {
                FirstName = "Hakan",
                MiddleName = default,
                LastName = "ÇELEBİ",
                DateOfBirth = new DateTime(1920, 08, 26)
            };
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}