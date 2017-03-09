namespace NCalculatorConsoleApplication
{
    interface ICalculator
    {
        bool ValidateInput(string input);
        decimal Calculate();
    }
}
