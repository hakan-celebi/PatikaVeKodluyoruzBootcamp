using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.BookOperations.GetBook
{
    public class GetBookQuery
    {
        private readonly BookStoreDbContext _context;
        public int? Id { get; set; }
        public GetBookQuery(BookStoreDbContext context)
        {
            _context = context;
        }

        public BookViewModel Handle()
        {
            if (Id is null)
                throw new InvalidOperationException("Id is empty");
            var book = _context.Books.SingleOrDefault(x => x.Id == Id);
            if (book is null)
                throw new InvalidOperationException("The book is not found");
            BookViewModel vm = new BookViewModel
            {
                Title = book.Title,
                GenreName = ((GenreEnum)book.GenreId).ToString(),
                PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy"),
                PageCount = book.PageCount
            };
            return vm;
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