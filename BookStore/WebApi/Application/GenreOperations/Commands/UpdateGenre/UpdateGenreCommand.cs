using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.Application.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommand
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int Id { get; set; }
        public UpdateGenreModel Model { get; set; }
        public UpdateGenreCommand(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle()
        {
            var genre = _context.Genres.SingleOrDefault(x => x.Id == Id);
            if (genre is null)
                throw new InvalidOperationException("The genre is not found");
            if (Model.Name is not null && _context.Genres.Any(x => x.Name.ToLower() == Model.Name!.ToLower().Trim() && x.Id != Id))
                throw new InvalidOperationException("The genre is already exist");
            genre = _mapper.Map(Model, genre);
            _context.SaveChanges();
        }
    }

    public class UpdateGenreModel
    {
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}