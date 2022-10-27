using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;

namespace WebApi.Application.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommand
    {
        private readonly BookStoreDbContext _context;
        public int Id {get; set; }

        public DeleteGenreCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var genre = _context.Genres.SingleOrDefault(x => x.Id == Id);
            if(genre is null)
                throw new InvalidOperationException("The genre is not found");
            if(genre.Books.Any(x => x.GenreId == Id))
                throw new InvalidOperationException("The genre is using by a book");
            _context.Remove(genre);
            _context.SaveChanges();
        }
    }
}