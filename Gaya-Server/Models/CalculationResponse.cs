namespace Gaya_Server.Models
{
    /// <summary>
    /// The calculation just performed, plus history that does not repeat that result.
    /// </summary>
    public class CalculationResponse
    {
        /// <summary>
        /// The result of the current calculation.
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// The three calculations saved before the current one, for the same operator.
        /// </summary>
        public List<OperationHistory> LastOperations { get; set; } = new();

        /// <summary>
        /// How many times this operator was used since the start of the month, including the current calculation.
        /// </summary>
        public int MonthlyOperationCount { get; set; }
    }
}
