using FluentValidation;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        private readonly IBookStoreDbContext _context;
        public UpdateBookCommandValidator(IBookStoreDbContext context)
        {
            _context = context;
            RuleFor(command => command.Id).GreaterThan(0);
            RuleFor(command => command.Model).NotNull().DependentRules(() => {
                RuleFor(command=> command.Model.Title).MinimumLength(2)
                    .When(x => x.Model.Title != null && x.Model.Title?.Trim() != string.Empty);
                RuleFor(command => command.Model.PageCount).GreaterThan(0)
                    .When(x => x.Model.PageCount != default);    
                RuleFor(command => command.Model.PublishDate.Date).LessThan(DateTime.Now.Date);
                RuleFor(command => command.Model.GenreId).Must(genreId => _context.Genres.Any(x => x.Id == genreId))
                    .When(x => x.Model.GenreId != default);
                RuleFor(command => command.Model.AuthorId).Must(authorId => _context.Authors.Any(x => x.Id == authorId))
                    .When(x => x.Model.AuthorId != default);
            });            
        }
    }
}