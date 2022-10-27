using FluentValidation;

namespace WebApi.Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0);
            RuleFor(command => command.Model).NotNull();
            RuleFor(command => command.Model.Name).MinimumLength(4)
                .When(x => x.Model.Name != null && x.Model.Name!.Trim() != string.Empty);
        }
    }
}