using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCalculatorConsoleApplication
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your input ");
            string input = Console.ReadLine();
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    string[] expressions = input.Split(',').Select(sValue => sValue.Trim()).ToArray();
                    StringBuilder sb = new StringBuilder();
                    foreach (string exp in expressions)
                    {
                        ICalculator cal = new Calculator();
                        if (cal.ValidateInput(exp))
                        {
                            decimal result = cal.Calculate();
                            sb.Append(result.ToString("0.0"));
                            sb.Append(",");
                        }
                    }
                    Console.WriteLine(sb.ToString().TrimEnd(','));
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
