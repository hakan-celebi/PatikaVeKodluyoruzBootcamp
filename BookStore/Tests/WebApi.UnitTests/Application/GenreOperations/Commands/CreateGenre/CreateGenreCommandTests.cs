using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.CreateGenre;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public CreateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Theory]
        [InlineData("Personal Growth")]
        [InlineData("  Personal Growth   ")]
        [InlineData("personal growth")]
        [InlineData("   personal growth   ")]
        public void WhenExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn(string genreName)
        {
            // Given
            CreateGenreCommand command = new CreateGenreCommand(_context, _mapper);
            command.Model = new CreateGenreModel
            {
                Name = genreName,
            };
            // When
            Action act = FluentActions.Invoking(() => command.Handle());
            // Then
            act.Should().Throw<InvalidOperationException>().And.Message.Should().Be("The genre is already exist");
        }

        [Fact]
        public void WhenUniqueGenreNameIsGiven_Genre_ShouldBeCreatedAsActive()
        {
            // Given
            CreateGenreCommand command = new CreateGenreCommand(_context, _mapper);
            command.Model = new CreateGenreModel
            {
                Name = "Story"
            };
            // When
            Action act = FluentActions.Invoking(() => command.Handle());
            // Then
            act.Should().NotThrow();
            var genre = _context.Genres.SingleOrDefault(x => x.Name == command.Model.Name);
            genre.Should().NotBeNull();
            genre!.IsActive.Should().BeTrue();
        }
    }
}