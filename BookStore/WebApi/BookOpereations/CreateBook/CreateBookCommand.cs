using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.BookOperations.CreateBook
{
    public class CreateBookCommand
    {
        private readonly BookStoreDbContext _context;
        public CreateBookModel Model { get; set; }

        public CreateBookCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            if(Model is null)
                throw new InvalidOperationException("The book data is empty");
            var existingBook = _context.Books.SingleOrDefault(x => x.Title == Model.Title);
            if(existingBook is not null)
                throw new InvalidOperationException("The book is already exist");
            var book = new Book
            {
                Title = Model.Title,
                PublishDate = Model.PublishDate,
                PageCount = Model.PageCount,
                GenreId = Model.GenreId
            };
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public class CreateBookModel
        {
            public string? Title { get; set; }
            public int GenreId { get; set; }
            public int PageCount { get; set; }
            public DateTime PublishDate { get; set; }
        }
    }
}