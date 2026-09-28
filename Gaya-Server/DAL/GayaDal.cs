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

        public List<Operator> GetAllOperators()
        {
            return [.. _context.Operators];
        }

        public void UpdateOperator(int id, bool isActive)
        {
            var operatorToUpdate = _context.Operators.FirstOrDefault(o => o.Id == id);
            if(operatorToUpdate != null)
            {
                operatorToUpdate.IsActive = isActive;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"Operator with ID {id} not found.");
            }
        }

        public bool IsOperatorActive(string operatorName)
        {
            return _context.Operators.Any(o => o.Name == operatorName && o.IsActive);
        }
    }
}