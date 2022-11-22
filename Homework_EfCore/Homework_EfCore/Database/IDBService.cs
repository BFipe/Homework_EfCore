using Homework_EfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework_EfCore.Database
{
    public interface IDBService
    {
        public Task<User> AddUser(UserForm userForm);

        public Task<Author> AddAuthor(AuthorForm authorForm);

        public Task<Book> AddBook(BookForm bookForm);

        public Task<List<User>> AllUsers();
        public Task<List<Author>> AllAuthors();
        public Task<List<Book>> AllBooks();

        public Task<UserBookInfo> GiveBookToUser(UserBookInfo userBookInfo);

        public Task<UserBookInfo> ReturnBookFromUser(UserBookInfo userBookInfo);

        public Task<string> RemoveUselessUsers();

        public Task<List<BorrowedBooksViewModel>> ReturnBorrowedBooksData();      
    }
}

