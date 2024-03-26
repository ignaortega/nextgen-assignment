namespace PaySpace.Calculator.Services.Exceptions
{
    public sealed class CalculatorException(string message) : InvalidOperationException(message);
}