using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.CreateBook;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var book = new Book
            {
                Title = "WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn",
                PageCount = 100,
                PublishDate = new DateTime(1990, 01, 10),
                GenreId = 1,
                AuthorId = 1                
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            
            CreateBookCommand command = new CreateBookCommand(_context ,_mapper);
            command.Model = new CreateBookModel
            {
                Title = " " + book.Title.ToLower() + " "
            };

            // Act
            // Assert
            FluentActions.Invoking(() => command.Handle()).Should()
                .Throw<InvalidOperationException>().And.Message.Should().Be("The book is already exist");
        }

        [Fact]
        public void WhenValidDataIsGiven_Book_ShouldBeCreated()
        {
            // Arrange
            var book = new Book
            {
                Title = "WhenValidDataIsGiven_Book_ShouldBeCreated_FirstBook",
                PageCount = 100,
                PublishDate = new DateTime(1990, 01, 10),
                GenreId = 1,
                AuthorId = 1                
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            
            CreateBookCommand command = new CreateBookCommand(_context ,_mapper);
            command.Model = new CreateBookModel
            {
                Title = "WhenValidDataIsGiven_Book_ShouldBeCreated_SecondBook"
            };

            // Act
            Action act = () => command.Handle();
            
            // Assert
            act.Should().NotThrow();
            book = _context.Books.SingleOrDefault(x => x.Title == command.Model.Title);
            book.Should().NotBeNull();
            book!.Title.Should().Be(command.Model.Title);
            book.GenreId.Should().Be(command.Model.GenreId);
            book.AuthorId.Should().Be(command.Model.AuthorId);
            book.PageCount.Should().Be(command.Model.PageCount);
            book.PublishDate.Should().Be(command.Model.PublishDate);
        }
    }
}