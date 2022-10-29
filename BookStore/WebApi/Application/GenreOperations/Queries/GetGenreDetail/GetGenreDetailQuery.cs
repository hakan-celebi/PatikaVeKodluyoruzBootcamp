using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailQuery
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int Id {get; set; }

        public GetGenreDetailQuery(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public GenreViewModel Handle()
        {
            var genre = _context.Genres.SingleOrDefault(x => x.Id == Id && x.IsActive);
            if (genre is null)
                throw new InvalidOperationException("The genre is not found");
            GenreViewModel vm = _mapper.Map<GenreViewModel>(genre);
            return vm;
        }
    }

    public class GenreViewModel
    {
        public string Name { get; set;}
    }
}