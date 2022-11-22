using Homework_EfCore.Exceptions;
using Homework_EfCore.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Homework_EfCore.Database
{
    public class DBService : IDBService
    {
        public async Task<User> AddUser(UserForm userForm)
        {
            using (MyDBContext db = new MyDBContext())
            {
                if (db.Users.Any(q => q.Email == userForm.Email))
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
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                return user;
            }
        }

        public async Task<Author> AddAuthor(AuthorForm authorForm)
        {
            using (MyDBContext db = new MyDBContext())
            {
                if (db.Authors.Any(q => q.FirstName +" "+ q.LastName == authorForm.FirstName +" "+ authorForm.LastName))
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
                await db.Authors.AddAsync(author);
                await db.SaveChangesAsync();
                return author;
            }
        }

        public async Task<Book> AddBook(BookForm bookForm)
        {
            using (MyDBContext db = new MyDBContext())
            {
                if (await db.Books.AnyAsync(q => q.Name == bookForm.BookName && q.Author.FirstName +" "+ q.Author.LastName == bookForm.AuthorFirstName+" "+bookForm.AuthorLastName))
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
                await db.Books.AddAsync(book);
                await db.SaveChangesAsync();
                return book;
            }
        }

        public async Task<List<User>> AllUsers()
        {
            using (MyDBContext db = new MyDBContext()) return await db.Users.ToListAsync();
        }

        public async Task<List<Author>> AllAuthors()
        {
            using (MyDBContext db = new MyDBContext()) return await db.Authors.ToListAsync();
        }

        public async Task<List<Book>> AllBooks()
        {
            using (MyDBContext db = new MyDBContext()) return await db.Books.ToListAsync();
        }

        public async Task<UserBookInfo> GiveBookToUser(UserBookInfo userBookInfo)
        {
            using (MyDBContext db = new MyDBContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(q => q.Email == userBookInfo.UserEmail);
                var book = await db.Books.SingleOrDefaultAsync(q => q.Name == userBookInfo.BookName && q.Author.FirstName == userBookInfo.AuthorFirstName && q.Author.LastName == userBookInfo.AuthorLastName);

                if (user == null)
                {
                    throw new ObjectNotFound(userBookInfo.UserEmail);
                }
                if (book == null)
                {
                    throw new ObjectNotFound(userBookInfo.BookName);
                }

                if (!await db.UserBooks.AnyAsync(q => q.UserId == user.UserId && q.BookId == book.BookId))
                {
                    await db.UserBooks.AddAsync(new UserBooks()
                    {
                        UserId = user.UserId,
                        BookId = book.BookId,
                    });
                    await db.SaveChangesAsync();
                    return new UserBookInfo() { BookName = book.Name, UserEmail = user.Email , AuthorFirstName = userBookInfo.AuthorFirstName, AuthorLastName = userBookInfo.AuthorLastName };
                }
                else
                {
                    throw new Exception($"User {user.Email} already took this book {book.Name}");
                }
            }
        }

        public async Task<UserBookInfo> ReturnBookFromUser(UserBookInfo userBookInfo)
        {
            using (MyDBContext db = new MyDBContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(q => q.Email == userBookInfo.UserEmail);
                var book = await db.Books.SingleOrDefaultAsync(q => q.Name == userBookInfo.BookName);

                if (user == null)
                {
                    throw new ObjectNotFound(userBookInfo.UserEmail);
                }
                if (book == null)
                {
                    throw new ObjectNotFound(userBookInfo.BookName);
                }

                var data = await db.UserBooks.SingleOrDefaultAsync(q => q.UserId == user.UserId && q.BookId == book.BookId);

                if (data == null)
                {
                    throw new Exception($"User {user.Email} never take this book {book.Name}");
                }

                db.UserBooks.Remove(data);
                await db.SaveChangesAsync();
                return new UserBookInfo() { BookName = book.Name, UserEmail = user.Email };
            }
        }

        public async Task<string> RemoveUselessUsers()
        {
            using (MyDBContext db = new MyDBContext())
            {
                var users = db.Users.Where(q => !q.Books.Any());
                if (users.Any())
                {
                    string message = "Deleted users:";
                    db.RemoveRange(users);
                    foreach (var user in users)
                    {
                        message += $"\n{user.FirstName} {user.LastName}";
                    }
                    await db.SaveChangesAsync();
                    return message;
                }
                else
                {
                    return "No users was removed";
                }
            }
        }

        public async Task<List<BorrowedBooksViewModel>> ReturnBorrowedBooksData()
        {
            using (MyDBContext db = new MyDBContext())
            {
                return await db.UserBooks.Select(row => new BorrowedBooksViewModel
                {
                    UserFullName = row.User.FirstName + " " + row.User.LastName,
                    UserBirthDate = row.User.BirthDate,
                    AuthorFullName = row.Book.Author.FirstName + " " + row.Book.Author.LastName,
                    BookName = row.Book.Name,
                    BookYear = row.Book.Year
                }).ToListAsync();
            }
        }


        private async Task<Author> _ReturnAuthorByName(string authorFirstName, string authorLastName)
        {
            using (MyDBContext db = new MyDBContext())
            {
                return await db.Authors.AsNoTracking().SingleOrDefaultAsync(q => q.FirstName == authorFirstName && q.LastName == authorLastName);
            }
        }
    }
}