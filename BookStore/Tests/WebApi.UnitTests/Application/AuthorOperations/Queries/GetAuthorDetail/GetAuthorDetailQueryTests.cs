using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAuthorIdIsNotExist_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, null);
            query.Id = 10;
            if(_context.Authors.Any(x => x.Id == 10))
                _context.Authors.Remove(_context.Authors.Find(10)!);
            // When
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The Author is not found");
        }

        [Fact]
        public void WhenAuthorIdIsExist_Author_ShouldBeTaken()
        {
            // Given
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
            var author = new Author
            {
                FirstName = "WhenAuthorIdIsExist",
                MiddleName = "Author",
                LastName = "ShouldBeTaken",
                DateOfBirth = new DateTime(2000, 08, 26)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            query.Id = author.Id;
            // When
            AuthorViewModel vm = query.Handle();
            // Then
            vm.Should().NotBeNull();
            vm.Name.Should().Be(author.FirstName + (author.MiddleName is null ? " " : " " + author.MiddleName + " ") + author.LastName);
            vm.DateOfBirth.Should().Be(author.DateOfBirth.ToShortDateString());
            author.IsActive.Should().BeTrue();
        }

        [Fact]
        public void WhenAuthorIsActiveIsFalse_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, null);
            var Author = new Author
            {
                FirstName = "WhenAuthorIsActiveIsFalse",
                MiddleName = "InvalidOperationException",
                LastName = "ShouldBeReturn",
                DateOfBirth = new DateTime(2000, 08, 26),
                IsActive = false
            };
            _context.Authors.Add(Author);
            _context.SaveChanges();
            query.Id = Author.Id;
            // When
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The Author is not found");
        }
    }
}