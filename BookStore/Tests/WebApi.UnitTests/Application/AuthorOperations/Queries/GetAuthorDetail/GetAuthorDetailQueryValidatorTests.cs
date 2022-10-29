using FluentAssertions;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;
using Xunit;

namespace Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQueryValidatorTests
    {
        public GetAuthorDetailQueryValidatorTests() { }

        [Fact]
        public void WhenInvalidAuthorIdGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(null, null);
            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            query.Id = default;
            // When
            var result = validator.Validate(query);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidAuthorIdIsGiven_Validation_ShouldNotBeReturnErrors()
        {
            // Given
            GetAuthorDetailQuery query = new GetAuthorDetailQuery(null, null);
            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            query.Id = 5;
            // When
             var result = validator.Validate(query);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}