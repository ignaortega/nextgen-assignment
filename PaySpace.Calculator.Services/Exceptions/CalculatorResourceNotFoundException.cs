namespace PaySpace.Calculator.Services.Exceptions
{
    public sealed class CalculatorResourceNotFoundException(string message) : InvalidOperationException(message);
}