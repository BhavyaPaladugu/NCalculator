using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;

namespace NCalculatorConsoleApplication
{
    /// <summary>
    /// Parses input and stores digits and operators in stack with respect to their precedence and compiles them using Expressions.
    /// </summary>
    public class Calculator : ICalculator
    {
        private readonly Stack<Expression> expressionStack = new Stack<Expression>();
        private readonly Stack<char> operatorStack = new Stack<char>();
        private string _input;

        #region Private Methods
        private Expression ReadOperand(TextReader reader)
        {
            var operand = string.Empty;
            int peek;
            while ((peek = reader.Peek()) > -1)
            {
                var next = (char)peek;
                if (char.IsDigit(next))
                {
                    reader.Read();
                    operand += next;
                }
                else
                {
                    break;
                }
            }
            return Expression.Constant(decimal.Parse(operand));
        }

        private Operation ReadOperation(TextReader reader)
        {
            var operation = (char)reader.Read();
            return (Operation)operation;
        }

        private void EvaluateWhile(Func<bool> condition)
        {
            while (condition())
            {
                var right = expressionStack.Pop();
                var left = expressionStack.Pop();

                expressionStack.Push(((Operation)operatorStack.Pop()).Apply(left, right));
            }
        }

        #endregion Private Methods

        #region Public Methods
        /// <summary>
        /// Validate the input string. Throws an exception if not successful 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>bool</returns>
        public bool ValidateInput(string input)
        {
            using (var reader = new StringReader(input))
            {
                int peek;
                while ((peek = reader.Peek()) > -1)
                {
                    var next = (char)peek;

                    if (char.IsDigit(next))
                    {
                        ReadOperand(reader);
                        continue;
                    }
                    if (Operation.IsDefined(next))
                    {
                        ReadOperation(reader);
                        continue;
                    }
                    if (next == ' ')
                    {
                        reader.Read();
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Encountered invalid character {0}", next),
                            "expression");
                    }
                }
            }
            _input = input;
            return true;
        }

        /// <summary>
        /// Calculates the given expression
        /// </summary>
        /// <returns>Output decimal value rounded to one decimal</returns>
        public decimal Calculate()
        {
            if (string.IsNullOrWhiteSpace(_input))
            {
                return 0;
            }

            operatorStack.Clear();
            expressionStack.Clear();

            using (var reader = new StringReader(_input))
            {
                int peek;
                while ((peek = reader.Peek()) > -1)
                {
                    var next = (char)peek;

                    if (char.IsDigit(next))
                    {
                        expressionStack.Push(ReadOperand(reader));
                        continue;
                    }
                    if (Operation.IsDefined(next))
                    {
                        var currentOperation = ReadOperation(reader);
                        EvaluateWhile(() => operatorStack.Count > 0 && currentOperation.Precedence <= ((Operation)operatorStack.Peek()).Precedence);
                        operatorStack.Push(next);
                        continue;
                    }
                    if (next != ' ')
                    {
                        throw new ArgumentException(string.Format("Encountered invalid character {0}", next), "expression");
                    }
                }
            }

            EvaluateWhile(() => operatorStack.Count > 0);

            var compiled = Expression.Lambda<Func<decimal>>(expressionStack.Pop()).Compile();
            return compiled();
        }

        #endregion Public Methods


    }
}
