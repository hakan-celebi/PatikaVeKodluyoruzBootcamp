using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }                
        public int PageCount { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        
        public Genre Genre { get; set; }
        public Author Author { get; set; }
    }
}