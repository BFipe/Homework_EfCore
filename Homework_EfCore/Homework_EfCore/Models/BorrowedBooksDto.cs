namespace Homework_EfCore.Models
{
    public class BorrowedBooksDto
    {
        public string UserFullName { get; set; }
        public string AuthorFullName { get; set; }
        public string BookName { get; set; }
        public DateTime UserBirthDate { get; set; }
        public int BookYear { get; set; }
    }
}
