using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Homework_EfCore.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
        public List<UserBooks> UserBooks { get; set; } = new List<UserBooks>();

    }
}
