using FluentValidation;
using WebApi.Common;

namespace WebApi.BookOperations.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(command => command.Id).NotNull().GreaterThan(0);
            RuleFor(command => command.Model).NotNull();
            RuleFor(command => command.Model.GenreId).IsInEnum();
            RuleFor(command => command.Model.PublishDate.Date).LessThan(DateTime.Now.Date);
            RuleFor(command=> command.Model.Title).MinimumLength(2);
        }
    }
}