using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.CreateAuthor;
using WebApi.DBOperations;
using Xunit;

namespace Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public CreateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Theory]
        [InlineData("Eric", "Ries")]
        [InlineData("eric", "ries")]
        [InlineData("ERİC", "RİES")]
        [InlineData(" Eric  ", "  Ries ")]
        public void WhenAlreadyExistAuthorIsGiven_InvalidOperationException_ShouldBeReturn(string firstName, string lastName)
        {
            // Given
            CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
            command.Model = new CreateAuthorModel
            {
                FirstName = firstName,
                MiddleName = default,
                LastName = lastName,
                DateOfBirth = new DateTime(2000, 08, 26)
            };
            // When            
            // Then
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The author is already exist");
        }

        [Fact]
        public void WhenNotExistAuthorIsGiven_Author_ShouldBeCreated()
        {
            // Given
            CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
            command.Model = new CreateAuthorModel
            {
                FirstName = "Hakan",
                MiddleName = default,
                LastName = "ÇELEBİ",
                DateOfBirth = new DateTime(2000, 08, 26)
            };
            // When            
            Action act = FluentActions.Invoking(() => command.Handle());
            // Then
            act.Should().NotThrow();
            var author = _context.Authors.SingleOrDefault(x => x.FirstName == command.Model.FirstName && x.MiddleName == command.Model.MiddleName && x.LastName == command.Model.LastName);
            author.Should().NotBeNull();
        }
    }
}