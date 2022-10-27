using FluentValidation;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0);
            RuleFor(command => command.Model).NotNull();
            RuleFor(command => command.Model.FirstName).MinimumLength(2)
                .When(x => x.Model.FirstName != null && x.Model.FirstName?.Trim() != string.Empty);
            RuleFor(command => command.Model.MiddleName).MinimumLength(2)
                .When(x => x.Model.MiddleName != null && x.Model.MiddleName?.Trim() != string.Empty);
            RuleFor(command => command.Model.LastName).MinimumLength(2)
                .When(x => x.Model.LastName != null && x.Model.LastName?.Trim() != string.Empty);
            RuleFor(command => command.Model.DateOfBirth).LessThan(DateTime.Now.Date);
        }
    }
}