using FluentAssertions;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using Xunit;

namespace Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailQueryValidatorTests
    {
        public GetGenreDetailQueryValidatorTests()
        {
        }

        [Fact]
        public void WhenInvalidGenreIdGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Given
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();
            query.Id = default;
            // When
            var result = validator.Validate(query);
            // Then
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Select(x => x.PropertyName).Should().Contain("Id");
        }

        [Fact]
        public void WhenValidGenreIdIsGiven_Validation_ShouldNotBeReturnErrors()
        {
            // Given
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();
            query.Id = 5;
            // When
             var result = validator.Validate(query);
            // Then
            result.Errors.Count.Should().Be(0);
        }
    }
}