using FluentValidation;

namespace WebApi.Application.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreCommandValidator()
        {
            RuleFor(command => command.Model).NotNull().DependentRules(() => {
                Transform(command => command.Model.Name, v => v?.Trim()).NotNull().NotEmpty().MinimumLength(4);
            });
        }
    }
}