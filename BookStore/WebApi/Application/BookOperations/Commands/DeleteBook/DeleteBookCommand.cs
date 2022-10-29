using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommand
    {
        private readonly IBookStoreDbContext _context;
        public int Id { get; set; }

        public DeleteBookCommand(IBookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var existingBook = _context.Books.SingleOrDefault(x => x.Id == Id);
            if(existingBook is null)
                throw new InvalidOperationException("The book is not exist");
            _context.Books.Remove(existingBook);
            _context.SaveChanges();
        }
    }
}