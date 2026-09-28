namespace Gaya_Server.Models
{
    /// <summary>
    /// A calculation operator that can be shown or hidden without deleting the row.
    /// </summary>
    public class Operator
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// True when the operator is offered for calculation. False hides it.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
