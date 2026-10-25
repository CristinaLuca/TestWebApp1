using BookStore_MVC_2026.Data;
using BookStore_MVC_2026.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

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


        /**
        * Seed a database
        */
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            //check if the file exists
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select an Excel file.");
                return View();
            }

            //check if the file selected is of the right type
            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Please upload an .xlsx file.");
                return View();
            }

            List<Book> books = new List<Book>();

            try
            {
                using var stream = file.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);

                if (worksheet.LastRowUsed() == null)
                {
                    ModelState.AddModelError("", "No book records were found.");
                    return View();
                }

                //go one by one through the book stored  (the records) and save them to the DB
                for (int i = 2; i <= worksheet.LastRowUsed().RowNumber(); i++)
                {
                    var row = worksheet.Row(i);
                    string title = row.Cell(1).GetString().Trim();
                    string author = row.Cell(2).GetString().Trim();
                    decimal price = row.Cell(3).GetValue<decimal>();

                    //create a book (instance of class Book) and add it to the list of books
                    books.Add(
                            new Book
                            {
                                Title = title,
                                Author = author,
                                Price = price,
                            }
                    );
                }

                //save the list of books to the DB
                await appDBcontext.Books.AddRangeAsync(books);
                await appDBcontext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "The import failed. Check the Excel file and database configuration.");
                return View();
            }



        }


    }
}
