namespace Gaya_Server.Models
{
    /// <summary>
    /// Inputs for one calculation. Numeric operations parse <see cref="FieldA"/> and <see cref="FieldB"/>; Concat uses them as text.
    /// </summary>
    public class CalculationRequest
    {
        public string Operator { get; set; } = string.Empty;
        public string FieldA { get; set; } = string.Empty;
        public string FieldB { get; set; } = string.Empty;
    }
}