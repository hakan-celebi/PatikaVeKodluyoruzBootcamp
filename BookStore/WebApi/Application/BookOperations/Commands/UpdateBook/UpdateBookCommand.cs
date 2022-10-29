using AutoMapper;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Commands.UpdateBook
{
    public class UpdateBookCommand
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int Id { get; set; }
        public UpdateBookModel Model { get; set; }
        
        public UpdateBookCommand(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle()
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == Id);
            if(book is null)
                throw new InvalidOperationException("The book is not exist");
            if(Model.Title is not null && _context.Books.Any(x => x.Title.ToLower() == Model.Title!.ToLower().Trim() && x.Id != Id))
                throw new InvalidOperationException("The book is already exist");
            book = _mapper.Map(Model, book);
            _context.SaveChanges();
        }
    }
    public class UpdateBookModel
    {
        public string? Title { get; set; }        
        public int PageCount { get; set; }
        public DateTime PublishDate { get; set; }
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}