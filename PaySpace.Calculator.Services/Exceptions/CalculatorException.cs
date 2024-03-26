namespace PaySpace.Calculator.Services.Exceptions
{
    public sealed class CalculatorException(string message = "Invalid Postal code. Calculator not found") : InvalidOperationException(message);
}