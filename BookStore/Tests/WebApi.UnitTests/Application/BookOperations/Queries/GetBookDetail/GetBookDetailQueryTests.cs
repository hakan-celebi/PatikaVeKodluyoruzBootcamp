using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestSetup;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBookDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenBookIdIsNotExist_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetBookDetailQuery query = new GetBookDetailQuery(_context, null);
            query.Id = 10;
            if(_context.Books.Any(x => x.Id == 10))
                _context.Books.Remove(_context.Books.Find(10)!);
            // When
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The book is not found");
        }

        [Fact]
        public void WhenBookIdIsExist_Book_ShouldBeTaken()
        {
            // Given
            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            var book = new Book
            {
                Title = "WhenBookIdIsExist_Book_ShouldBeTaken",
                GenreId = 2,
                AuthorId = 3,
                PageCount = 1100,
                PublishDate = new DateTime(1920, 08, 26),
                IsActive = true
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            //book = _context.Books.Include(x => x.Genre).Include(x => x.Author).SingleOrDefault(x => x.Id == book.Id);
            query.Id = book.Id;
            // When
            BookViewModel vm = query.Handle();
            // Then
            vm.Title.Should().Be(book.Title);
            vm.GenreName.Should().Be(book.Genre.Name);
            vm.AuthorName.Should().Be(book.Author.FirstName + " " + 
                (book.Author.MiddleName is not null ? book.Author.MiddleName + " " : string.Empty + book.Author.LastName));
            vm.PageCount.Should().Be(book.PageCount);        
            vm.PublishDate.Should().Be(book.PublishDate.Date.ToShortDateString());
        }

        [Fact]
        public void WhenBookIsActiveIsFalse_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetBookDetailQuery query = new GetBookDetailQuery(_context, null);
            var book = new Book
            {
                Title = "WhenBookIsActiveIsFalse_Book_ShouldNotBeTaken",
                GenreId = 2,
                AuthorId = 3,
                PageCount = 1100,
                PublishDate = new DateTime(1920, 08, 26),
                IsActive = false
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            query.Id = book.Id;
            // When
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The book is not found");
        }

        [Fact]
        public void WhenIsActiveOfGenreOfBookIsGivenAsFalse_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetBookDetailQuery query = new GetBookDetailQuery(_context, null);
            _context.Genres.SingleOrDefault(x => x.Id == 2)!.IsActive = false;
            _context.SaveChanges();
            var book = new Book
            {
                Title = "WhenBookIsActiveIsFalse_Book_ShouldNotBeTaken",
                GenreId = 2,
                AuthorId = 3,
                PageCount = 1100,
                PublishDate = new DateTime(1920, 08, 26),
                IsActive = false
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            query.Id = book.Id;
            // When
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The book is not found");
        }

        [Fact]
        public void WhenIsActiveOfAuthorOfBookIsGivenAsFalse_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetBookDetailQuery query = new GetBookDetailQuery(_context, null);
            _context.Authors.SingleOrDefault(x => x.Id == 3)!.IsActive = false;
            _context.SaveChanges();
            var book = new Book
            {
                Title = "WhenBookIsActiveIsFalse_Book_ShouldNotBeTaken",
                GenreId = 2,
                AuthorId = 3,
                PageCount = 1100,
                PublishDate = new DateTime(1920, 08, 26),
                IsActive = false
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            query.Id = book.Id;
            // When
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The book is not found");
        }
    }
}