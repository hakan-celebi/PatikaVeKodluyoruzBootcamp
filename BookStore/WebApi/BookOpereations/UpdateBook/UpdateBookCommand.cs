using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.BookOperations.UpdateBook
{
    public class UpdateBookCommand
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int? Id { get; set; }
        public UpdateBookModel Model { get; set; }

        public UpdateBookCommand(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle()
        {
            var existingBook = _context.Books.SingleOrDefault(x => x.Id == Id);
            if(existingBook is null)
                throw new InvalidOperationException("The book is not exist");
            existingBook = _mapper.Map(Model, existingBook);
            _context.SaveChanges();
        }

        public class UpdateBookModel
        {
            public string? Title { get; set; }
            public int GenreId { get; set; }
            public int PageCount { get; set; }
            public DateTime PublishDate { get; set; }
        }
    }
}