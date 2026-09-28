namespace Gaya_Server.Models
{
    public class OperationHistory
    {
        public int Id { get; set; }

        public string Operator { get; set; } = string.Empty;

        public string FieldA { get; set; } = string.Empty;

        public string FieldB { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}