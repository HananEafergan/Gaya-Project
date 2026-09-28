using Gaya_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Gaya_Server.DAL
{
    /// <summary>
    /// Entity Framework context for the Gaya database.
    /// </summary>
    public class GayaDbContext(DbContextOptions<GayaDbContext> options) : DbContext(options)
    {
        public DbSet<Operator> Operators { get; set; }
        public DbSet<OperationHistory> OperationHistory { get; set; }
    }
}