using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand
    {
        private readonly IBookStoreDbContext _context;
        public int Id {get; set; }

        public DeleteAuthorCommand(IBookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(x => x.Id == Id);
            if(author is null)
                throw new InvalidOperationException("The Author is not found");
            if(_context.Books.Any(x => x.AuthorId == Id))
                throw new InvalidOperationException("The Author is using by a book");
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
    }
}