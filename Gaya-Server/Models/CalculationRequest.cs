namespace Gaya_Server.Models
{
    public class CalculationRequest
    {
        public string Operator { get; set; } = string.Empty;
        public string FieldA { get; set; } = string.Empty;
        public string FieldB { get; set; } = string.Empty;
    }
}