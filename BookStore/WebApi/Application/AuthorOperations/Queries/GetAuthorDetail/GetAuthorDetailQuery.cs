using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Queries.GetAuthorDetail
{
    public class GetAuthorDetailQuery
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int Id {get; set; }

        public GetAuthorDetailQuery(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public AuthorViewModel Handle()
        {
            var author = _context.Authors.SingleOrDefault(x => x.Id == Id && x.IsActive);
            if (author is null)
                throw new InvalidOperationException("The Author is not found");
            AuthorViewModel vm = _mapper.Map<AuthorViewModel>(author);
            return vm;
        }
    }

    public class AuthorViewModel
    {
        public string Name { get; set;}
        public string DateOfBirth { get; set; }
    }
}