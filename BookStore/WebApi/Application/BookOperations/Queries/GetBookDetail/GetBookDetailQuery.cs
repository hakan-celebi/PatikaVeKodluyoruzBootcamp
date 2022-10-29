using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Queries.GetBookDetail
{
    public class GetBookDetailQuery
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int Id { get; set; }
        public GetBookDetailQuery(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BookViewModel Handle()
        {
            var book = _context.Books.Include(x => x.Genre).Include(x => x.Author).AsNoTracking()
                .SingleOrDefault(x => x.Id == Id && x.IsActive && x.Genre.IsActive && x.Author.IsActive);
            if (book is null)
                throw new InvalidOperationException("The book is not found");
            BookViewModel vm = _mapper.Map<BookViewModel>(book);
            return vm;
        }
    }

    public class BookViewModel
    {
        public string Title { get; set; }
        public string GenreName { get; set; }
        public string AuthorName { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }
    }
}