namespace Gaya_Server.Models
{
    public class CalculationResponse
    {
        public string Result { get; set; } = string.Empty;

        public List<OperationHistory> LastOperations { get; set; } = new();

        public int MonthlyOperationCount { get; set; }
    }
}
