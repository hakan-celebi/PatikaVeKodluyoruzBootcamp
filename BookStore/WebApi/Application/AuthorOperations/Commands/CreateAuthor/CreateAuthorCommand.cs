using AutoMapper;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.Application.AuthorOperations.Commands.CreateAuthor
{
    public class CreateAuthorCommand
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public CreateAuthorModel Model { get; set; }

        public CreateAuthorCommand(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(x => 
                x.FirstName.ToLower() == Model.FirstName!.ToLower().Trim() &&
                (x.MiddleName != null ? x.MiddleName.ToLower() : string.Empty) == (Model.MiddleName != null ? Model.MiddleName.ToLower().Trim() : string.Empty) &&
                x.LastName.ToLower() == Model.LastName!.ToLower().Trim()
            );
            if(author is not null)
                throw new InvalidOperationException("The author is already exist");
            author = _mapper.Map<Author>(Model);
            _context.Authors.Add(author);
            _context.SaveChanges();
        }
    }

    public class CreateAuthorModel
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}