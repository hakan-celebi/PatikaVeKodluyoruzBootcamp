using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;

namespace Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQueryValidatorTests
    {
        public GetBookDetailQueryValidatorTests() { }

        [Fact]
        public void WhenInvalidBookIdGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetBookDetailQuery query = new GetBookDetailQuery(null, null);
            GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
            query.Id = default;
            // When
            var result = validator.Validate(query);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidBookIdIsGiven_Validation_ShouldNotBeReturnErrors()
        {
            // Given
            GetBookDetailQuery query = new GetBookDetailQuery(null, null);
            GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
            query.Id = 5;
            // When
             var result = validator.Validate(query);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}