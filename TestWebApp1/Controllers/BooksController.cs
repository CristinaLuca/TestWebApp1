using Microsoft.AspNetCore.Mvc;

namespace BookStore_MVC_2026.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            // create list of books
            var books = new List<string>
            {
                "Harry Potter",
                "1984"
            };

            return View(books);
        }
    }
}
