using Homework_EfCore.Database;
using Homework_EfCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework_EfCore.Controllers;


[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IDBService _dbservice;

    public HomeController(IDBService dBService)
    {
        _dbservice = dBService;
    }

    [HttpPost]
    [Route("/AddUser")]
    public async Task<User> AddUser([FromBody] UserForm userForm)
    {
        return await _dbservice.AddUser(userForm);
    }

    [HttpPost]
    [Route("/AddAuthor")]
    public async Task<Author> AddAuthor([FromBody] AuthorForm authorForm)
    {
        return await _dbservice.AddAuthor(authorForm);
    }

    [HttpPost]
    [Route("/AddBook")]
    public async Task<Book> AddBook([FromBody] BookForm bookForm)
    {
        return await _dbservice.AddBook(bookForm);
    }

    [HttpGet]
    [Route("/AllUsers")]
    public async Task<List<User>> AllUsers()
    {
        return await _dbservice.AllUsers();
    }

    [HttpGet]
    [Route("/AllAuthors")]
    public async Task<List<Author>> AllAuthors()
    {
        return await _dbservice.AllAuthors();
    }

    [HttpGet]
    [Route("/AllBooks")]
    public async Task<List<Book>> AllBooks()
    {
        return await _dbservice.AllBooks();
    }

    [HttpGet]
    [Route("/AllBorrowedBooks")]
    public async Task<List<BorrowedBooksDto>> BorrowedBooks()
    {
        return await _dbservice.ReturnBorrowedBooksData();
    }

    [HttpPost]
    [Route("/GiveBook")]
    public async Task<UserBookInfo> GiveBook([FromBody] UserBookInfo userBookInfo)
    {
        return await _dbservice.GiveBookToUser(userBookInfo);
    }

    [HttpDelete]
    [Route("/ReturnBook")]
    public async Task<UserBookInfo> ReturnBook([FromBody] UserBookInfo userBookInfo)
    {
        return await _dbservice.ReturnBookFromUser(userBookInfo);
    }

    [HttpDelete]
    [Route("/RemoveUsersWithoutBooks")]
    public async Task<string> RemoveUselessUsers()
    {
        return await _dbservice.RemoveUselessUsers();
    }


}