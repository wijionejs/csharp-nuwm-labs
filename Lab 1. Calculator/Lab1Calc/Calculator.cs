using System;
using System.Collections.Generic;

namespace Lab1Calc
{
    public static class Calculator
    {
        private const char Addition = '+';
        private const char Subtraction = '-';
        private const char Multiplication = '*';
        private const char Division = '/';
        private const char OpenParenthesis = '(';
        private const char CloseParenthesis = ')';

        private static readonly Dictionary<char, Func<float, float, float>> OpMap = new Dictionary<char, Func<float, float, float>>
        {
            { Addition, (a, b) => a + b },
            { Subtraction, (a, b) => a - b },
            { Multiplication, (a, b) => a * b },
            { Division, (a, b) => a / b }
        };

        private static readonly Dictionary<char, int> PrecedenceMap = new Dictionary<char, int>
        {
            { Addition, 1 },
            { Subtraction, 1 },
            { Multiplication, 2 },
            { Division, 2 }
        };

        public static float Evaluate(string expression)
        {
            var operands = new Stack<float>();
            var operators = new Stack<char>();

            var index = 0;

            while (index < expression.Length)
            {
                if (char.IsDigit(expression[index]))
                {
                    var number = "";

                    while (index < expression.Length && (char.IsDigit(expression[index]) || expression[index] == '.'))
                    {
                        number += expression[index];
                        index++;
                    }

                    operands.Push(float.Parse(number));
                }
                else if (expression[index] == OpenParenthesis)
                {
                    var closeIndex = FindClosingParenthesis(expression, index);
                    var subExpression = expression.Substring(index + 1, closeIndex - index - 1);
                    operands.Push(Evaluate(subExpression));

                    index = closeIndex + 1;
                }
                else if (OpMap.ContainsKey(expression[index]))
                {
                    while (operators.Count > 0 && PrecedenceMap[operators.Peek()] >= PrecedenceMap[expression[index]])
                    {
                        EvaluateTopOperation(operands, operators);
                    }

                    operators.Push(expression[index]);
                    index++;
                }
                else
                {

                    index++;
                }
            }
            
            while (operators.Count > 0)
            {
                EvaluateTopOperation(operands, operators);
            }

            return operands.Pop();
        }

        private static void EvaluateTopOperation(Stack<float> operands, Stack<char> operators)
        {
            if (operands.Count < 2)
            {
                return;
            }

            var b = operands.Pop();
            var a = operands.Pop();

            var op = operators.Pop();

            operands.Push(OpMap[op](a, b));
        }

        private static int FindClosingParenthesis(string expression, int openIndex)
        {
            var counter = 1;

            for (var i = openIndex + 1; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case OpenParenthesis:
                        counter++;
                        break;
                    case CloseParenthesis:
                        counter--;
                        break;
                }

                if (counter == 0)
                    return i;
            }

            throw new ArgumentException("Mismatched parentheses in expression.");
        }
    }
}
