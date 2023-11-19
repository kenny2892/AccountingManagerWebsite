using AccountingWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingWebsite.Data
{
    public class StatementContext : DbContext
    {
        public DbSet<StatementMapping> StatementMappings { get; set; }

        public StatementContext(DbContextOptions<StatementContext> options) : base(options)
        {
        }
    }
}
