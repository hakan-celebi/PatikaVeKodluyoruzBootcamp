using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.DeleteGenre;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            if(_context.Genres.Any(x => x.Id == 10))
                _context.Genres.Remove(_context.Genres.SingleOrDefault(x => x.Id == 10)!);
            command.Id = 10;
            // When
            Action act = FluentActions.Invoking(() => command.Handle());
            // Then
            act.Should().Throw<InvalidOperationException>().And.Message.Should().Be("The genre is not found");
        }

        [Fact]
        public void WhenGenreNotUsingByAnyBook_Genre_ShouldBeDeleted()
        {
            // Given
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            var genre = new Genre
            {
                Name = "WhenGenreNotUsingByAnyBook_Genre_ShouldBeDeleted"
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            command.Id = genre.Id;
            // When
            Action act = FluentActions.Invoking(() => command.Handle());
            // Then
            act.Should().NotThrow();
            _context.Genres.Any(x => x.Id == command.Id).Should().BeFalse();
        }

        [Fact]
        public void WhenGenreIsUsingByAnyBook_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            var genre = new Genre
            {
                Name = "WhenGenreIsUsingByAnyBook_InvalidOperationException_ShouldBeReturn"
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            var book = new Book
            {
                Title = "WhenGenreIsUsingByAnyBook_InvalidOperationException_ShouldBeReturn_Book",
                GenreId = genre.Id,
                AuthorId = 2,
                PageCount = 145,
                PublishDate = new DateTime(1850, 05, 24)
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            command.Id = genre.Id;
            // When
            // Then
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The genre is using by any book");
        }
    }
}