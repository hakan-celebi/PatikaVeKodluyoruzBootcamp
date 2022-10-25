using AutoMapper;
using WebApi.BookOperations.GetBook;
using WebApi.BookOperations.GetBooks;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;
using static WebApi.BookOperations.UpdateBook.UpdateBookCommand;

namespace WebApi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBookModel, Book>();
            CreateMap<UpdateBookModel, Book>()
                .ForMember(dest => dest.GenreId, opt => opt.Condition(src => src.GenreId != default))
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != default))
                .ForMember(dest => dest.PageCount, opt => opt.Condition(src => src.PageCount != default))
                .ForMember(dest => dest.PublishDate, opt => { opt.Condition(src => src.PublishDate != default); opt.MapFrom(src => src.PublishDate.Date); });
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()));
            CreateMap<Book, BooksViewModel>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()));                           
        }
    }
}