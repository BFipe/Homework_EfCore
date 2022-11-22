using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Homework_EfCore.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int Year { get; set; }

        public List<User> Users { get; set; } = new List<User>();
        public List<UserBooks> UserBooks { get; set; } = new List<UserBooks>();

    }

   
}
