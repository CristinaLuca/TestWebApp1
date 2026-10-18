namespace BookStore_MVC_2026.Data
{
    using BookStore_MVC_2026.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
        }

        // connects the C# model Book to a table in the DB
        public DbSet<Book> Books { get; set; }

    }
}
