using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.CreateBook;
using WebApi.DBOperations;
using Xunit;

namespace Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public CreateBookCommandValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        // Invalid data for Title
        [InlineData("L", 1100, 1, 2)]
        [InlineData("", 1100, 1, 2)]
        [InlineData("  ", 1100, 1, 2)]
        [InlineData(null, 1100, 1, 2)]
        // Invalid data for PageCount
        [InlineData("Lord Of The Rings", -5, 1, 2)]
        [InlineData("Lord Of The Rings", default, 1, 2)]
        [InlineData("Lord Of The Rings", null, 1, 2)]
        // Invalid data for GenreId
        [InlineData("Lord Of The Rings", 1100, -5, 2)]
        [InlineData("Lord Of The Rings", 1100, 10, 2)]
        [InlineData("Lord Of The Rings", 1100, default, 2)]
        [InlineData("Lord Of The Rings", 1100, null, 2)]
        // Invalid data for AuthorId
        [InlineData("Lord Of The Rings", 1100, 1, 0)]
        [InlineData("Lord Of The Rings", 1100, 1, 10)]
        [InlineData("Lord Of The Rings", 1100, 1, default)]
        [InlineData("Lord Of The Rings", 1100, 1, null)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string title, int pageCount, int genreId, int authorId)
        {
            // ARRANGE
            CreateBookCommand command = new CreateBookCommand(null, null);
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            command.Model = new CreateBookModel
            {
                 Title = title,
                 PageCount = pageCount,
                 PublishDate = new DateTime(1920, 10, 15),
                 GenreId = genreId,
                 AuthorId = authorId
            };
            // ACT
            var result = validator.Validate(command);
            // ASSERT
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenPublishDateEqualNowIsGiven_Validator_ShouldBeReturnErrors()
        {
            // ARRANGE
            CreateBookCommand command = new CreateBookCommand(null, null);
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            command.Model = new CreateBookModel
            {
                 Title = "Lord Of The Rings",
                 PageCount = 1100,
                 PublishDate = DateTime.Now.Date,
                 GenreId = 2,
                 AuthorId = 3
            };
            // ACT
            var result = validator.Validate(command);
            // ASSERT
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenPublishDateIsGivenAsDefault_Validator_ShouldBeReturnErrors()
        {
            // ARRANGE
            CreateBookCommand command = new CreateBookCommand(null, null);
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            command.Model = new CreateBookModel
            {
                 Title = "Lord Of The Rings",
                 PageCount = 1100,
                 PublishDate = default,
                 GenreId = 2,
                 AuthorId = 3
            };
            // ACT
            var result = validator.Validate(command);
            // ASSERT
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnErrors()
        {
            // ARRANGE
            CreateBookCommand command = new CreateBookCommand(null, null);
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            command.Model = new CreateBookModel
            {
                 Title = "Lord Of The Rings",
                 PageCount = 1100,
                 PublishDate = new DateTime(1920, 10, 26),
                 GenreId = 2,
                 AuthorId = 1
            };
            // ACT
            var result = validator.Validate(command);
            // ASSERT
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void WhenModelNotGiven_Validator_ShouldBeReturnErrors()
        {
            // Given
            CreateBookCommand command = new CreateBookCommand(null, null);
            CreateBookCommandValidator validator = new CreateBookCommandValidator(_context);
            // When
            var result = validator.Validate(command);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}