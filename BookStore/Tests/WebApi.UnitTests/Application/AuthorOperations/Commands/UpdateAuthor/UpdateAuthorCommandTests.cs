using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public UpdateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenNotExistAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, null);
            command.Id = 0;
            // When            
            // Then
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The Author is not found");
        }

        [Fact]
        public void WhenExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            command.Id = 2;
            command.Model = new UpdateAuthorModel
            {
                FirstName = "Eric",
                MiddleName = string.Empty,
                LastName = "Ries",
                DateOfBirth = new DateTime(1978, 09, 22)
            };
            // When            
            // Then
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The Author is already exist");
            _context.Entry(_context.Authors.SingleOrDefault(x => x.Id == command.Id)!).Reload();
        }

        [Fact]
        public void WhenAuthorNameIsGivenWithoutChanges_BookName_ShouldNotBeUpdated()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            command.Id = 1;
            command.Model = new UpdateAuthorModel
            {
                FirstName = "Eric",
                MiddleName = string.Empty,
                LastName = "Ries",
                DateOfBirth = new DateTime(1978, 09, 22)
            };
            // When    
            Action act = FluentActions.Invoking(() => command.Handle());        
            // Then
            act.Should().NotThrow();
            var author = _context.Authors.SingleOrDefault(x => x.Id == command.Id);
            author.Should().NotBeNull();
            author!.FirstName.Should().Be(command.Model.FirstName);
            author.MiddleName.Should().Be(command.Model.MiddleName == string.Empty ? null : command.Model.MiddleName);
            author.LastName.Should().Be(command.Model.LastName);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            var author = new Author
            {
                FirstName = "WhenValidInputsAreGiven",
                MiddleName = "Book",
                LastName = "ShouldBeUpdated",
                DateOfBirth = new DateTime(1920, 04, 13)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            command.Id = author.Id;
            command.Model = new UpdateAuthorModel
            {
                FirstName = "WhenValidInputsAreGivenChanged",
                MiddleName = "BookChanged",
                LastName = "ShouldBeUpdatedChanged",
                DateOfBirth = new DateTime(1978, 09, 22),
                IsActive = false
            };
            // When    
            Action act = FluentActions.Invoking(() => command.Handle());        
            // Then
            act.Should().NotThrow();
            author = _context.Authors.SingleOrDefault(x => x.Id == command.Id);
            author.Should().NotBeNull();
            author!.FirstName.Should().Be(command.Model.FirstName);
            author.MiddleName.Should().Be(command.Model.MiddleName == string.Empty ? null : command.Model.MiddleName);
            author.LastName.Should().Be(command.Model.LastName);
            author.DateOfBirth.Should().Be(command.Model.DateOfBirth);
            author.IsActive.Should().Be(command.Model.IsActive);
        }

        [Fact]
        public void WhenNotActiveAuthorIsGiven_AuthorIsActive_ShouldBeTrue()
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            var author = new Author
            {
                FirstName = "WhenNotActiveAuthorIsGiven",
                MiddleName = "AuthorIsActive",
                LastName = "ShouldBeTrue",
                DateOfBirth = new DateTime(1920, 04, 13)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            command.Id = author.Id;
            command.Model = new UpdateAuthorModel
            {
                IsActive = false
            };
            // When    
            Action act = FluentActions.Invoking(() => command.Handle());        
            // Then
            act.Should().NotThrow();
            author = _context.Authors.SingleOrDefault(x => x.Id == command.Id);
            author.Should().NotBeNull();
            author!.IsActive.Should().Be(command.Model.IsActive);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", null, "")]
        public void WhenNullOrEmptyNameInputsAreGiven_AuthorName_ShouldNotBeUpdated(string firstName, string? middleName, string lastName)
        {
            // Given
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            var author = _context.Authors.First();
            string? authorMiddleName = author.MiddleName;
            command.Id = author.Id;
            command.Model = new UpdateAuthorModel
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName
            };
            // When
            Action act = FluentActions.Invoking(() => command.Handle());
            // Then
            act.Should().NotThrow();
            author = _context.Authors.SingleOrDefault(x => x.Id == command.Id);
            author.Should().NotBeNull();
            author!.FirstName.Should().NotBe(command.Model.FirstName);
            author.MiddleName.Should().Be(authorMiddleName);
            author.LastName.Should().NotBe(command.Model.LastName);
        }

        [Fact]
        public void WhenStringEmptyMiddleNameIsGiven_AuthorMiddleName_ShouldBeNull()
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            var author = new Author
            {
                FirstName = "WhenStringEmptyMiddleNameIsGiven",
                MiddleName = "AuthorMiddleName",
                LastName = "ShouldBeNull",
                DateOfBirth = new DateTime(1920, 04, 13)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            command.Id = author.Id;
            command.Model = new UpdateAuthorModel
            {
                MiddleName = ""
            };
            // When    
            Action act = FluentActions.Invoking(() => command.Handle());        
            // Then
            act.Should().NotThrow();
            author = _context.Authors.SingleOrDefault(x => x.Id == command.Id);
            author!.MiddleName.Should().BeNull();
        }

        [Fact]
        public void WhenValidInputsAreGivenWithSpaceChar_AuthorName_ShouldBeUpdatedWithoutSpaces()
        {
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
            var author = new Author
            {
                FirstName = "WhenValidInputsWithSpaceCharIsGiven",
                MiddleName = "AuthorName",
                LastName = "ShouldBeUpdatedWithoutSpaces",
                DateOfBirth = new DateTime(1920, 04, 13)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();
            command.Id = author.Id;
            command.Model = new UpdateAuthorModel
            {
                FirstName = "         WhenValidInputsWithSpaceCharIsGivenSpace   ",
                MiddleName = "     AuthorNameSpace    ",
                LastName = "     ShouldBeUpdatedWithoutSpacesSpace   ",
            };
            // When    
            Action act = FluentActions.Invoking(() => command.Handle());        
            // Then
            act.Should().NotThrow();
            author = _context.Authors.SingleOrDefault(x => x.Id == command.Id);
            author!.FirstName.Should().Be(command.Model.FirstName.Trim());
            author!.MiddleName.Should().Be(command.Model.MiddleName.Trim());
            author!.LastName.Should().Be(command.Model.LastName.Trim());
        }
    }
}