using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Queries.GetAuthors;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Queries.GetAuthors
{
    public class GetAuthorsQueryTests: IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorsQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAuthorsTaken_AuthorsAreIsActiveFalse_ShouldNotBeContains()
        {
            // Given
            GetAuthorsQuery query = new GetAuthorsQuery(_context, _mapper);
            var author = new Author
            {
                FirstName = "WhenAuthorsTaken",
                MiddleName = "AuthorsAreIsActiveFalse",
                LastName = "ShouldNotBeContains",
                DateOfBirth = new DateTime(2000, 08, 26),
                IsActive = false
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            // When
            List<AuthorsViewModel> vm = query.Handle();
            // Then
            vm.Any(x => x.Name == author.FirstName + (author.MiddleName is null ? " " : " " + author.MiddleName + " ") + author.LastName)
                .Should().BeFalse();
            vm.Count.Should().Be(_context.Authors.Count() - _context.Authors.Where(x => !x.IsActive).Count());
        }

        [Fact]
        public void WhenAuthorsIsActivesAreNotFalse_AllAuthors_ShouldBeTaken()
        {
            GetAuthorsQuery query = new GetAuthorsQuery(_context, _mapper);
            _context.Authors.ToList().ForEach(x => x.IsActive = true);
            _context.SaveChanges();
            // When
            List<AuthorsViewModel> vm = query.Handle();
            // Then
            vm.Count.Should().Be(_context.Authors.Count());
        }
    }
}