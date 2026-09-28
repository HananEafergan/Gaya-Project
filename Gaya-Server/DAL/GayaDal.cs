using Gaya_Server.Models;

namespace Gaya_Server.DAL
{
    public class GayaDAL
    {
        private readonly GayaDbContext _context;

        public GayaDAL(GayaDbContext context)
        {
            _context = context;
        }

        public string[] GetActiveOperators()
        {
            return [.. _context.Operators
                .Where(o => o.IsActive)
                .Select(o => o.Name)];
        }
    }
}