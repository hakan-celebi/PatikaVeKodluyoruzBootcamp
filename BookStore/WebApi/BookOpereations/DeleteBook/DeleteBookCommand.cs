using WebApi.DBOperations;

namespace WebApi.BookOperations.DeleteBook
{
    public class DeleteBookCommand
    {
        private readonly BookStoreDbContext _context;
        public int? Id { get; set; }

        public DeleteBookCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            if (Id is null)
                throw new InvalidOperationException("Id is empty");
            var existingBook = _context.Books.SingleOrDefault(x => x.Id == Id);
            if(existingBook is null)
                throw new InvalidOperationException("The book is not exist");
            _context.Remove(existingBook);
            _context.SaveChanges();
        }
    }
}