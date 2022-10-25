using FluentValidation;

namespace WebApi.BookOperations.GetBook
{
    public class GetBookQueryValidator : AbstractValidator<GetBookQuery>
    {
        public GetBookQueryValidator()
        {
            RuleFor(query => query.Id).NotNull().GreaterThan(0);
        }
    }
}