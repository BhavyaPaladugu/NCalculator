using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NCalculatorConsoleApplication
{
    internal sealed class Operation
    {
        private readonly Func<Expression, Expression, Expression> operation;

        public static readonly Operation Addition = new Operation(1, Expression.Add, "Addition");
        public static readonly Operation Subtraction = new Operation(1, Expression.Subtract, "Subtraction");
        public static readonly Operation Multiplication = new Operation(2, Expression.Multiply, "Multiplication");
        public static readonly Operation Division = new Operation(2, Expression.Divide, "Division");

        private static readonly Dictionary<char, Operation> Operations = new Dictionary<char, Operation>
        {
            { '+', Addition },
            { '-', Subtraction },
            { '*', Multiplication},
            { '/', Division }
        };

        public int Precedence { get; private set; }
        public string Name { get; private set; }

        private Operation(int precedence, Func<Expression, Expression, Expression> operation, string name)
        {
            Precedence = precedence;
            this.operation = operation;
            Name = name;
        }
        /// <summary>
        /// Used to get the value of operator.
        /// </summary>
        /// <param name="operation"></param>
        public static explicit operator Operation(char operation)
        {
            Operation result;

            if (Operations.TryGetValue(operation, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// Creates BinaryExpression that represents an arithmetic operation 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public Expression Apply(Expression left, Expression right)
        {
            return operation(left, right);
        }

        public static bool IsDefined(char operation)
        {
            return Operations.ContainsKey(operation);
        }
    }
}
