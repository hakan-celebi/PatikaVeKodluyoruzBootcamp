using FluentValidation;

namespace WebApi.Application.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreCommandValidator()
        {
            RuleFor(command => command.Model).NotNull();
            RuleFor(command => command.Model.Name).NotNull().NotEmpty().MinimumLength(4);
        }
    }
}