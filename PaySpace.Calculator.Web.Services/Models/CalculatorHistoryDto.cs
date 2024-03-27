namespace PaySpace.Calculator.Web.Services.Models
{
    public sealed class CalculatorHistory
    {
        public long Id { get; set; }
        public string PostalCode { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Income { get; set; }

        public decimal Tax { get; set; }

        public string Calculator { get; set; }
    }
}