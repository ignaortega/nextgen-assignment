namespace PaySpace.Calculator.API.Models
{
    public sealed class CalculateRequest
    {
        public string? PostalCode { get; set; }

        public decimal Income { get; set; }
        public long? Id { get; set; }
    }
}