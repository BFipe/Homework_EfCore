using Homework_EfCore.Exceptions;
using Homework_EfCore.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Homework_EfCore.Database
{
    public class DBService : IDBService
    {
        private readonly MyDBContext _myDBContext;
        public DBService(MyDBContext myDBContext)
        {
            _myDBContext = myDBContext;
        }
        public async Task<User> AddUser(UserForm userForm)
        {

            if (_myDBContext.Users.Any(q => q.Email == userForm.Email))
            {
                throw new ObjectAlreadyExists(userForm.Email);
            }
            User user = new User()
            {
                Email = userForm.Email,
                BirthDate = userForm.BirthDate,
                FirstName = userForm.FirstName,
                LastName = userForm.LastName
            };
            await _myDBContext.Users.AddAsync(user);
            await _myDBContext.SaveChangesAsync();
            return user;

        }

        public async Task<Author> AddAuthor(AuthorForm authorForm)
        {

            if (_myDBContext.Authors.Any(q => q.FirstName + " " + q.LastName == authorForm.FirstName + " " + authorForm.LastName))
            {
                throw new ObjectAlreadyExists(authorForm.FirstName + " " + authorForm.LastName);
            }
            Author author = new Author()
            {
                BirthDate = authorForm.BirthDate,
                FirstName = authorForm.FirstName,
                LastName = authorForm.LastName,
                Country = authorForm.Country
            };
            await _myDBContext.Authors.AddAsync(author);
            await _myDBContext.SaveChangesAsync();
            return author;

        }

        public async Task<Book> AddBook(BookForm bookForm)
        {
            if (await _myDBContext.Books.AnyAsync(q => q.Name == bookForm.BookName && q.Author.FirstName + " " + q.Author.LastName == bookForm.AuthorFirstName + " " + bookForm.AuthorLastName))
            {
                throw new ObjectAlreadyExists(bookForm.BookName);
            }

            if (bookForm.BookYear < 0 || bookForm.BookYear > DateTime.Now.Year)
            {
                throw new IncorrectValue(bookForm.BookYear.ToString());
            }
            Author author = await _ReturnAuthorByName(bookForm.AuthorFirstName, bookForm.AuthorLastName);
            Book book = new Book()
            {
                Name = bookForm.BookName,
                Year = bookForm.BookYear,
                AuthorId = author.AuthorId
            };
            await _myDBContext.Books.AddAsync(book);
            await _myDBContext.SaveChangesAsync();
            return book;
        }

        public async Task<List<User>> AllUsers()
        {
            return await _myDBContext.Users.ToListAsync();
        }

        public async Task<List<Author>> AllAuthors()
        {
            return await _myDBContext.Authors.ToListAsync();
        }

        public async Task<List<Book>> AllBooks()
        {
            return await _myDBContext.Books.ToListAsync();
        }

        public async Task<UserBookInfo> GiveBookToUser(UserBookInfo userBookInfo)
        {

            var user = await _myDBContext.Users.SingleOrDefaultAsync(q => q.Email == userBookInfo.UserEmail);
            var book = await _myDBContext.Books.SingleOrDefaultAsync(q => q.Name == userBookInfo.BookName && q.Author.FirstName == userBookInfo.AuthorFirstName && q.Author.LastName == userBookInfo.AuthorLastName);

            if (user == null)
            {
                throw new ObjectNotFound(userBookInfo.UserEmail);
            }
            if (book == null)
            {
                throw new ObjectNotFound(userBookInfo.BookName);
            }

            if (!await _myDBContext.UserBooks.AnyAsync(q => q.UserId == user.UserId && q.BookId == book.BookId))
            {
                await _myDBContext.UserBooks.AddAsync(new UserBooks()
                {
                    UserId = user.UserId,
                    BookId = book.BookId,
                });
                await _myDBContext.SaveChangesAsync();
                return new UserBookInfo() { BookName = book.Name, UserEmail = user.Email, AuthorFirstName = userBookInfo.AuthorFirstName, AuthorLastName = userBookInfo.AuthorLastName };
            }
            else
            {
                throw new Exception($"User {user.Email} already took this book {book.Name}");
            }

        }

        public async Task<UserBookInfo> ReturnBookFromUser(UserBookInfo userBookInfo)
        {

            var user = await _myDBContext.Users.SingleOrDefaultAsync(q => q.Email == userBookInfo.UserEmail);
            var book = await _myDBContext.Books.SingleOrDefaultAsync(q => q.Name == userBookInfo.BookName);

            if (user == null)
            {
                throw new ObjectNotFound(userBookInfo.UserEmail);
            }
            if (book == null)
            {
                throw new ObjectNotFound(userBookInfo.BookName);
            }

            var data = await _myDBContext.UserBooks.SingleOrDefaultAsync(q => q.UserId == user.UserId && q.BookId == book.BookId);

            if (data == null)
            {
                throw new Exception($"User {user.Email} never take this book {book.Name}");
            }

            _myDBContext.UserBooks.Remove(data);
            await _myDBContext.SaveChangesAsync();
            return new UserBookInfo() { BookName = book.Name, UserEmail = user.Email };

        }

        public async Task<string> RemoveUselessUsers()
        {

            var users = _myDBContext.Users.Where(q => !q.Books.Any());
            if (users.Any())
            {
                string message = "Deleted users:";
                _myDBContext.RemoveRange(users);
                foreach (var user in users)
                {
                    message += $"\n{user.FirstName} {user.LastName}";
                }
                await _myDBContext.SaveChangesAsync();
                return message;
            }
            else
            {
                return "No users was removed";
            }

        }

        public async Task<List<BorrowedBooksDto>> ReturnBorrowedBooksData()
        {

            return await _myDBContext.UserBooks.Select(row => new BorrowedBooksDto
            {
                UserFullName = row.User.FirstName + " " + row.User.LastName,
                UserBirthDate = row.User.BirthDate,
                AuthorFullName = row.Book.Author.FirstName + " " + row.Book.Author.LastName,
                BookName = row.Book.Name,
                BookYear = row.Book.Year
            }).ToListAsync();

        }


        private async Task<Author> _ReturnAuthorByName(string authorFirstName, string authorLastName)
        {

            return await _myDBContext.Authors.AsNoTracking().SingleOrDefaultAsync(q => q.FirstName == authorFirstName && q.LastName == authorLastName);

        }
    }
}