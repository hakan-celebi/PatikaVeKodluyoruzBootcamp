using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.BookOperations.GetBook
{
    public class GetBookQuery
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int? Id { get; set; }
        public GetBookQuery(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BookViewModel Handle()
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == Id);
            if (book is null)
                throw new InvalidOperationException("The book is not found");
            BookViewModel vm = _mapper.Map<BookViewModel>(book);
            return vm;
        }

        public override bool Equals(object? obj)
        {
            return obj is GetBookQuery query &&
                   EqualityComparer<IMapper>.Default.Equals(_mapper, query._mapper);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_mapper);
        }
    }

    public class BookViewModel
    {
        public string? Title { get; set; }
        public string? GenreName { get; set; }
        public int PageCount { get; set; }
        public string? PublishDate { get; set; }
    }
}