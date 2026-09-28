using Gaya_Server.Models;

namespace Gaya_Server.DAL
{
    /// <summary>
    /// Reads and writes operators and calculation history.
    /// </summary>
    public class GayaDAL
    {
        private readonly GayaDbContext _context;

        public GayaDAL(GayaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the names of operators marked active.
        /// </summary>
        public string[] GetActiveOperators()
        {
            return [.. _context.Operators
                .Where(o => o.IsActive)
                .Select(o => o.Name)];
        }

        /// <summary>
        /// Returns every operator row, active or not.
        /// </summary>
        public List<Operator> GetAllOperators()
        {
            return [.. _context.Operators];
        }

        /// <summary>
        /// Updates <see cref="Operator.IsActive"/> for an existing id.
        /// </summary>
        /// <exception cref="Exception">Thrown when no operator has <paramref name="id"/>.</exception>
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

        /// <summary>
        /// Inserts one calculation into the history table.
        /// </summary>
        public void SaveOperation(OperationHistory operation)
        {
            _context.OperationHistory.Add(operation);
            _context.SaveChanges();
        }

        /// <summary>
        /// Returns the three most recent saved calculations for <paramref name="operatorName"/>.
        /// </summary>
        public List<OperationHistory> GetLastOperations(string operatorName)
        {
            return [.. _context.OperationHistory
                .Where(o => o.Operator == operatorName)
                .OrderByDescending(o => o.CreatedAt)
                .Take(3)];
        }

        /// <summary>
        /// Counts calculations for <paramref name="operatorName"/> since the start of the current month.
        /// </summary>
        public int GetMonthlyOperationCount(string operatorName)
        {
            DateTime startOfMonth = new(
                DateTime.Now.Year,
                DateTime.Now.Month,
                1);

            return _context.OperationHistory
                .Count(o =>
                    o.Operator == operatorName &&
                    o.CreatedAt >= startOfMonth);
        }

        /// <summary>
        /// Returns whether <paramref name="operatorName"/> exists and is currently shown.
        /// </summary>
        public bool IsOperatorActive(string operatorName)
        {
            return _context.Operators.Any(o => o.Name == operatorName && o.IsActive);
        }
    }
}