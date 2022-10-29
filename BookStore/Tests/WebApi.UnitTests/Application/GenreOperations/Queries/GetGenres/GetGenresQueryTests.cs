using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Queries.GetGenres;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Queries.GetGenres
{
    public class GetGenresQueryTests: IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetGenresQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenGenresTaken_GenresAreIsActiveFalse_ShouldNotBeContains()
        {
            // Given
            GetGenresQuery query = new GetGenresQuery(_context, _mapper);
            var genre = new Genre
            {
                Name = "WhenGenresTaken_GenresAreIsActiveFalse_ShouldNotBeContains",                
                IsActive = false
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            // When
            List<GenresViewModel> vm = query.Handle();
            // Then
            vm.Any(x => x.Name == genre.Name).Should().BeFalse();
            vm.Count.Should().Be(_context.Genres.Count() - _context.Genres.Where(x => !x.IsActive).Count());
        }

        [Fact]
        public void WhenGenresIsActivesAreNotFalse_AllGenres_ShouldBeTaken()
        {
            GetGenresQuery query = new GetGenresQuery(_context, _mapper);
            _context.Genres.ToList().ForEach(x => x.IsActive = true);
            _context.SaveChanges();
            // When
            List<GenresViewModel> vm = query.Handle();
            // Then
            vm.Count.Should().Be(_context.Genres.Count());
        }
    }
}