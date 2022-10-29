using FluentValidation;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        private readonly IBookStoreDbContext _context;
        public CreateBookCommandValidator(IBookStoreDbContext context)
        {
            _context = context;
            RuleFor(command => command.Model).NotNull().DependentRules(() => {
                RuleFor(command=> command.Model.Title).NotNull().NotEmpty().MinimumLength(2);            
                RuleFor(command => command.Model.PageCount).GreaterThan(0);
                RuleFor(command => command.Model.PublishDate.Date).NotEqual(new DateTime()).LessThan(DateTime.Now.Date);
                RuleFor(command => command.Model.GenreId).GreaterThan(0)
                    .Must(genreId => _context.Genres.Any(x => x.Id == genreId && x.IsActive));
                RuleFor(command => command.Model.AuthorId).GreaterThan(0)
                    .Must(authorId => _context.Authors.Any(x => x.Id == authorId && x.IsActive));
            });
            
        }
    }
}