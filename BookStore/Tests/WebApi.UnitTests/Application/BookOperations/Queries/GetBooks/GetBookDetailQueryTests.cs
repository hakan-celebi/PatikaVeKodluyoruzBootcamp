using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestSetup;
using WebApi.Application.BookOperations.Queries.GetBooks;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Queries.GetBooks
{
    public class GetBooksQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBooksQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenBooksTaken_BooksAreIsActiveFalse_ShouldNotBeContains()
        {
            // Given
            GetBooksQuery query = new GetBooksQuery(_context, _mapper);
            var book = new Book
            {
                Title = "WhenBooksTaken_BooksAreIsActiveFalse_ShouldNotBeContains",
                GenreId = 2,
                AuthorId = 3,
                PageCount = 1100,
                PublishDate = new DateTime(1920, 08, 26),
                IsActive = false
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            // When
            List<BooksViewModel> vm = query.Handle();
            // Then
            vm.Any(x => x.Title == book.Title).Should().BeFalse();
            vm.Count.Should().Be(_context.Books.Count() - _context.Books.Where(x => !x.IsActive).Count());
        }

        [Fact]
        public void WhenBooksIsActivesAreNotFalse_AllBooks_ShouldBeTaken()
        {
            GetBooksQuery query = new GetBooksQuery(_context, _mapper);
            _context.Books.ToList().ForEach(x => x.IsActive = true);
            _context.SaveChanges();
            // When
            List<BooksViewModel> vm = query.Handle();
            // Then
            vm.Count.Should().Be(_context.Books.Count());
        }
    }
}