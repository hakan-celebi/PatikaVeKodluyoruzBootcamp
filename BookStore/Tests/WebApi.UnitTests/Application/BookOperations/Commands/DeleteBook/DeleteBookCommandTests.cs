using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.DeleteBook;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenBookIdIsNotExist_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.Id = 10;
            if(_context.Books.Any(x => x.Id == 10))
                _context.Books.Remove(_context.Books.Find(10)!);
            // When
            // Then
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The book is not exist");
        }

        [Fact]
        public void WhenBookIdIsExist_Book_ShouldBeDeleted()
        {
            // Given
            DeleteBookCommand command = new DeleteBookCommand(_context);
            var book = new Book
            {
                Title = "WhenBookIdIsExist_Book_ShouldBeDeleted",
                GenreId = 2,
                AuthorId = 3,
                PageCount = 1100,
                PublishDate = new DateTime(1920, 08, 26)
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            command.Id = book.Id;
            // When
            FluentActions.Invoking(() => command.Handle()).Should().NotThrow();
            // Then
            _context.Books.Any(x => x.Id == command.Id).Should().BeFalse();
        }
    }
}