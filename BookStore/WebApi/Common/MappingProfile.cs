using AutoMapper;
using WebApi.Application.AuthorOperations.Commands.CreateAuthor;
using WebApi.Application.AuthorOperations.Commands.UpdateAuthor;
using WebApi.Application.AuthorOperations.Queries.GetAuthorDetail;
using WebApi.Application.AuthorOperations.Queries.GetAuthors;
using WebApi.Application.BookOperations.Commands.CreateBook;
using WebApi.Application.BookOperations.Commands.UpdateBook;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.Application.BookOperations.Queries.GetBooks;
using WebApi.Application.GenreOperations.Commands.CreateGenre;
using WebApi.Application.GenreOperations.Commands.UpdateGenre;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.Application.GenreOperations.Queries.GetGenres;
using WebApi.Entities;

namespace WebApi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // BOOK
            CreateMap<CreateBookModel, Book>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date));
            CreateMap<UpdateBookModel, Book>()
                .ForMember(dest => dest.GenreId, opt => opt.Condition(src => src.GenreId != default))
                .ForMember(dest => dest.AuthorId, opt => opt.Condition(src => src.AuthorId != default))
                .ForMember(dest => dest.Title, opt => {
                    opt.Condition(src => src.Title != default && src.Title?.Trim() != string.Empty);
                    opt.MapFrom(src => src.Title!.Trim());
                })
                .ForMember(dest => dest.PageCount, opt => opt.Condition(src => src.PageCount != default))
                .ForMember(dest => dest.PublishDate, opt => 
                    { opt.Condition(src => src.PublishDate != default); opt.MapFrom(src => src.PublishDate.Date); });
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.ToShortDateString()))
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.AuthorName, opt => 
                    opt.MapFrom(src => NameConverter.ConvertToFullName(src.Author.FirstName, src.Author.MiddleName, src.Author.LastName)));
            CreateMap<Book, BooksViewModel>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.AuthorName, opt => 
                    opt.MapFrom(src => NameConverter.ConvertToFullName(src.Author.FirstName, src.Author.MiddleName, src.Author.LastName)));

            // GENRE
            CreateMap<CreateGenreModel, Genre>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
            CreateMap<UpdateGenreModel, Genre>()
                .ForMember(dest => dest.Name, opt => {
                    opt.Condition(src => src.Name != default && src.Name!.Trim() != string.Empty);
                    opt.MapFrom(src => src.Name!.Trim());
                });
            CreateMap<Genre, GenresViewModel>();
            CreateMap<Genre, GenreViewModel>();     

            // AUTHOR
            CreateMap<CreateAuthorModel, Author>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName == null ? null : src.MiddleName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Date));       
            CreateMap<UpdateAuthorModel, Author>()
                .ForMember(dest => dest.FirstName, opt => 
                    {
                        opt.Condition(src => src.FirstName != default && src.FirstName?.Trim() != string.Empty);
                        opt.MapFrom(src => src.FirstName!.Trim());
                    })
                .ForMember(dest => dest.MiddleName, opt => 
                    { 
                        opt.Condition(src => src.MiddleName != null); 
                        opt.MapFrom(src => (src.MiddleName == string.Empty ? null : src.MiddleName!.Trim()));
                    })
                .ForMember(dest => dest.LastName, opt => 
                    {
                        opt.Condition(src => src.LastName != default && src.LastName?.Trim() != string.Empty);
                        opt.MapFrom(src => src.LastName!.Trim());
                    })
                .ForMember(dest => dest.DateOfBirth, opt => 
                    { opt.Condition(src => src.DateOfBirth != default); opt.MapFrom(src => src.DateOfBirth.Date); });
            CreateMap<Author, AuthorViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => NameConverter.ConvertToFullName(src.FirstName, src.MiddleName, src.LastName)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));
            CreateMap<Author, AuthorsViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => NameConverter.ConvertToFullName(src.FirstName, src.MiddleName, src.LastName)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));
        }
    }
}