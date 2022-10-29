using FluentAssertions;
using WebApi.Application.AuthorOperations.Commands.CreateAuthor;
using Xunit;

namespace Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandValidatorTests
    {
        public CreateAuthorCommandValidatorTests() { }

        [Fact]
        public void WhenNullModelIsGiven_Validator_ShouldBeReturnError()
        {
            // Given
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Select(x => x.PropertyName).Should().Contain("Model");
        }

        [Fact]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors()
        {
            // Given
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            command.Model = new CreateAuthorModel
            {
                FirstName = null,
                MiddleName = default,
                LastName = string.Empty,
                DateOfBirth = new DateTime(1920, 08, 26)
            };
            // When
            var result = validator.Validate(command);
            // Then
            var errorPropertyNameList = result.Errors.Select(x => x.PropertyName);
            result.Errors.Count.Should().BeGreaterThan(0);            
            errorPropertyNameList.Should().NotContain("Model");
            errorPropertyNameList.Should().NotContain("Model.MiddleName");
            errorPropertyNameList.Should().NotContain("Model.DateOfBirth");
            errorPropertyNameList.Should().Contain("Model.FirstName");
            errorPropertyNameList.Should().Contain("Model.LastName");
        }

        [Fact]
        public void WhenDateOfBirthIsNotGiven_Validator_ShouldBeReturn()
        {
            // Given
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            command.Model = new CreateAuthorModel
            {
                FirstName = "Hakan",
                MiddleName = default,
                LastName = "ÇELEBİ"
            };
            // When
            var result = validator.Validate(command);
            // Then
            var errorPropertyNameList = result.Errors.Select(x => x.PropertyName);
            result.Errors.Count.Should().BeGreaterThan(0);            
            errorPropertyNameList.Should().NotContain("Model");
            errorPropertyNameList.Should().NotContain("Model.FirstName");
            errorPropertyNameList.Should().NotContain("Model.LastName");
            errorPropertyNameList.Should().NotContain("Model.MiddleName");
            errorPropertyNameList.Should().Contain("Model.DateOfBirth");
        }

        [Fact]
        public void WhenDateOfBirthIsGivenAsEqualOrBiggerThanToday_Validator_ShouldBeReturn()
        {
            // Given
             CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            command.Model = new CreateAuthorModel
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
            errorPropertyNameList.Should().NotContain("Model");
            errorPropertyNameList.Should().NotContain("Model.FirstName");
            errorPropertyNameList.Should().NotContain("Model.LastName");
            errorPropertyNameList.Should().NotContain("Model.MiddleName");
            errorPropertyNameList.Should().Contain("Model.DateOfBirth");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldBeReturnErrors()
        {
            // Given
            CreateAuthorCommand command = new CreateAuthorCommand(null, null);
            CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
            command.Model = new CreateAuthorModel
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