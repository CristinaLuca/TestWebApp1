using BookStore_MVC_2026.Data;
using BookStore_MVC_2026.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore_MVC_2026.Controllers
{
    public class BooksController : Controller
    {
        //create an object of the database context defined in Program.cs
        //It represents a session with the database
        private readonly AppDBContext appDBcontext;

        public BooksController(AppDBContext context)
        {
            appDBcontext = context;
        }


        public async Task<IActionResult> Index()
        {
            //get the list from the DB asynchronously (it doesn’t block the server while waiting for the database)
            var books = await appDBcontext.Books.ToListAsync();

            return View(books);
        }
    }
}
