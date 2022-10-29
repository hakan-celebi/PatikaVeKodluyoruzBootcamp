using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.DeleteAuthor;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenAuthorIdIsNotExist_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.Id = 10;
            if(_context.Authors.Any(x => x.Id == 10))
                _context.Authors.Remove(_context.Authors.Find(10)!);
            // When
            // Then
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The Author is not found");
        }

        [Fact]
        public void WhenAuthorIdIsExist_Author_ShouldBeDeleted()
        {
            // Given
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            var author = new Author
            {
                FirstName = "WhenAuthorIdIsExist",
                MiddleName = "Author",
                LastName = "ShouldBeDeleted",
                DateOfBirth = new DateTime(2000, 08, 26)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            command.Id = author.Id;
            // When
            FluentActions.Invoking(() => command.Handle()).Should().NotThrow();
            // Then
            _context.Authors.Any(x => x.Id == command.Id).Should().BeFalse();
        }
    }
}