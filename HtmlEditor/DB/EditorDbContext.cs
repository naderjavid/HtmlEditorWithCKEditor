using Microsoft.EntityFrameworkCore;
using HtmlEditor.Entities;

namespace HtmlEditor.DB
{
    public class EditorDbContext : DbContext
    {
        public EditorDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<HtmlPage> HtmlPags { get; set; }
    }
}
