using FluentValidation;

namespace WebApi.Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(command => command.Model).NotNull().DependentRules(() => {
                Transform(command => command.Model.FirstName, v => v?.Trim()).NotNull().NotEmpty().MinimumLength(2);
                RuleFor(command => command.Model.MiddleName).MinimumLength(2)
                    .When(x => x.Model.MiddleName != default && x.Model.MiddleName.Trim() != string.Empty);
                Transform(command => command.Model.LastName, v => v?.Trim()).NotNull().NotEmpty().MinimumLength(2);
                RuleFor(command => command.Model.DateOfBirth).NotEqual(new DateTime()).LessThan(DateTime.Now.Date);
            });
            
        }
    }
}