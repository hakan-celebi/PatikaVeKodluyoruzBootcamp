using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Commands.UpdateGenre;
using WebApi.DBOperations;
using Xunit;

namespace Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        public readonly BookStoreDbContext _context;
        public readonly IMapper _mapper;
        public UpdateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Theory]
        [InlineData("Personal Growth")]
        [InlineData("  Personal Growth   ")]
        [InlineData("personal growth")]
        [InlineData("  personal growth ")]
        public void WhenExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn(string genreName)
        {
            // Given
            UpdateGenreCommand command = new UpdateGenreCommand(_context, _mapper);
            command.Id = 2;
            command.Model = new UpdateGenreModel
            {
                Name = genreName
            };
            // When            
            // Then
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The genre is already exist");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void WhenNullOrEmptyGenreNameIsGiven_Genre_ShouldNotBeUpdatedAndIsActiveShouldBeTrue(string genreName)
        {
            // Given
            UpdateGenreCommand command = new UpdateGenreCommand(_context, _mapper);
            command.Id = 2;
            command.Model = new UpdateGenreModel
            {
                Name = genreName
            };
            // When      
            Action act = FluentActions.Invoking(() => command.Handle());      
            // Then
            act.Should().NotThrow();
            var genre = _context.Genres.SingleOrDefault(x => x.Id == command.Id);
            genre.Should().NotBeNull();
            genre!.Name.Should().NotBe(genreName);
            genre.IsActive.Should().BeTrue();
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeUpdated()
        {
             // Given
            UpdateGenreCommand command = new UpdateGenreCommand(_context, _mapper);
            command.Id = 2;
            command.Model = new UpdateGenreModel
            {
                Name = "Story",
                IsActive = false
            };
            // When      
            Action act = FluentActions.Invoking(() => command.Handle());      
            // Then
            act.Should().NotThrow();
            var genre = _context.Genres.SingleOrDefault(x => x.Id == command.Id);
            genre.Should().NotBeNull();
            genre!.Name.Should().Be(command.Model.Name);
            genre.IsActive.Should().Be(command.Model.IsActive);
        }
    }
}