using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public GetGenreDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            if(_context.Genres.Any(x => x.Id == 10))
                _context.Genres.Remove(_context.Genres.SingleOrDefault(x => x.Id == 10)!);
            query.Id = 10;
            // When            
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The genre is not found");
        }

        [Fact]
        public void WhenNotActiveGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            var genre = new Genre
            {
                Name = "WhenNotActiveGenreIdIsGiven_InvalidOperationException_ShouldBeReturn",
                IsActive = false
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            query.Id = genre.Id;
            // When            
            // Then
            FluentActions.Invoking(() => query.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The genre is not found");
        }

        [Fact]
        public void WhenExistAndActiveGenreIdIsGiven_Genre_ShouldBeReturn()
        {
            // Given
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            var genre = new Genre
            {
                Name = "WhenExistAndActiveGenreIdIsGiven_Genre_ShouldBeReturn",
                IsActive = true
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            query.Id = genre.Id;
            // When
            GenreViewModel vm = null;
            Action act = FluentActions.Invoking(() => { vm = query.Handle(); });
            // Then
            act.Should().NotThrow();
            vm.Should().NotBeNull();
            vm!.Name.Should().NotBeNull();
            vm.Name.Should().Be(genre.Name);
        }
    }
}