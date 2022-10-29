using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.UpdateBook;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public UpdateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Theory]
        [InlineData("Lean Startup")]
        [InlineData("   Lean Startup ")]
        [InlineData(" lean startup   ")]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn(string title)
        {
            // Arrange
            UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
            command.Id = 2;
            command.Model = new UpdateBookModel{ Title = title };
            // Act            
            // Assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The book is already exist");            
        }

        [Fact]
        public void WhenNotExistBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
            command.Id = 10;
             if(_context.Books.Any(x => x.Id == 10))
                _context.Books.Remove(_context.Books.Find(10)!);
            // Act            
            // Assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The book is not exist");            
        }

        [Theory]
        [InlineData(null, default, default, default)]
        [InlineData("", default, default, default)]
        [InlineData("   ", default, default, default)]
        public void WhenAllDataNullOrEmptyIsGiven_BookDataExceptTheIsActive_ShouldNotBeChanged(string title, int genreId, int authorId, int pageCount)
        {
            // Arrange
            UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
            command.Id = 2;
            command.Model = new UpdateBookModel
            {
                Title = title,
                GenreId = genreId,
                AuthorId = authorId,
                PageCount = pageCount          
            };
            
            // Act
            FluentActions.Invoking(() => command.Handle()).Should().NotThrow();
            // Assert
            var book = _context.Books.SingleOrDefault(x => x.Id == 2);
            book!.Title.Should().NotBe(title);
            book.AuthorId.Should().NotBe(authorId);
            book.GenreId.Should().NotBe(genreId);
            book.PageCount.Should().NotBe(pageCount);
            book.PublishDate.Should().NotBe(default);
            book.IsActive.Should().Be(true);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            // Arrange
            UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
            var book = new Book
            {
                Title = "Lort Of The Rings",
                GenreId = 1,
                AuthorId = 1,
                PageCount = 110,
                PublishDate = new DateTime(1920, 01, 01),
                IsActive = true
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            command.Id = book.Id;
            command.Model = new UpdateBookModel
            {
                Title = "Lord Of The Rings",
                GenreId = 2,
                AuthorId = 3,
                PageCount = 1100,
                PublishDate = new DateTime(1920, 08, 26),
                IsActive = false
            };
            // Act   
            FluentActions.Invoking(() => command.Handle()).Should().NotThrow();         
            // Assert                
            book = _context.Books.SingleOrDefault(x => x.Id == command.Id);
            book!.Title.Should().Be(command.Model.Title);
            book.GenreId.Should().Be(command.Model.GenreId);
            book.AuthorId.Should().Be(command.Model.AuthorId);
            book.PageCount.Should().Be(command.Model.PageCount);
            book.PublishDate.Should().Be(command.Model.PublishDate);
            book.IsActive.Should().Be(command.Model.IsActive);
        }
    }
}