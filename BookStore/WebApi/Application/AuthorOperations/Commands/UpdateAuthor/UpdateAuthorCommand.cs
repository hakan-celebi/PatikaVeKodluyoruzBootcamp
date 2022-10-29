using AutoMapper;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.Application.AuthorOperations.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        public int Id { get; set; }
        public UpdateAuthorModel Model { get; set; }
        public UpdateAuthorCommand(IBookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(x => x.Id == Id);
            if (author is null)
                throw new InvalidOperationException("The Author is not found");
            author = _mapper.Map(Model, author);
            string fullName = NameConverter.ConvertToFullName(author.FirstName, author.MiddleName, author.LastName);
            if (_context.Authors.Any(x => x.Id != Id && NameConverter.ConvertToFullName(x.FirstName, x.MiddleName, x.LastName) == fullName))
                throw new InvalidOperationException("The Author is already exist");
            _context.SaveChanges();
        }
    }

    public class UpdateAuthorModel
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
    }
}