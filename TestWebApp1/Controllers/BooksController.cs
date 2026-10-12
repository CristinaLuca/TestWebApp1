using Microsoft.AspNetCore.Mvc;

namespace TestWebApp1.Controllers
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

            //passes the list books to the View
            return View(books);
        }
    }
}
