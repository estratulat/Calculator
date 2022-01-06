using Calculator.Calculator.Expressions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Calculator.Parsing
{
    public class DefaultParser : IParser
    {
        public List<MathObject> Parse(string stringExpression)
        {
            List<MathObject> constructedExpression = new List<MathObject>();

            int counterStart = AdjustForFirstNegative(stringExpression, constructedExpression);

            for (int currentIndex = counterStart; currentIndex < stringExpression.Length; currentIndex++)
            {
                if (Operator.IsOperator(stringExpression[currentIndex]))
                {
                    currentIndex = AddOperatorAndGetNextIndex(stringExpression, constructedExpression, currentIndex);
                }
                else if (IsNumeric(stringExpression[currentIndex]))
                {
                    currentIndex = AddNumberAndGetNextIndex(stringExpression, constructedExpression, currentIndex);
                }
                else if (stringExpression[currentIndex] == '(')
                {
                    currentIndex = AddBracketsExpressionAndGetNextIndex(stringExpression, constructedExpression, currentIndex);
                }
            }

            return constructedExpression;
        }

        private static int AddOperatorAndGetNextIndex(string stringExpression, List<MathObject> expression, int i)
        {
            expression.Add(new Operator(stringExpression[i]));

            if (i < stringExpression.Length - 2 && stringExpression[i + 1] == '-')
            {
                i = AddNumberAndGetNextIndex(stringExpression, expression, i + 2, false);
            }

            return i;
        }

        private int AddBracketsExpressionAndGetNextIndex(string stringExpression, List<MathObject> expression, int index)
        {
            InsertMultiplicationIfRequired(stringExpression, expression, index);

            index++;
            string stringSubExpression = ExtractStringMathObject(stringExpression, ref index, currentIndex => !(stringExpression[currentIndex] == ')'));

            expression.Add(new Expression(Parse(stringSubExpression)));

            return index;
        }

        private static int AddNumberAndGetNextIndex(string stringExpression, List<MathObject> expression, int numberStartIndex, bool asPositive = true)
        {
            string operand = ExtractStringMathObject(stringExpression, ref numberStartIndex, currentIndex => IsNumeric(stringExpression[currentIndex]));
            expression.Add(new Operand(int.Parse(asPositive ? operand : $"-{operand}")));

            numberStartIndex--;
            return numberStartIndex;
        }

        private static int AdjustForFirstNegative(string stringExpression, List<MathObject> expression)
        {
            if (stringExpression.ElementAtOrDefault(0) == '-')
            {
                var counter = AddNumberAndGetNextIndex(stringExpression, expression, 1, false);
                return ++counter;
            }

            return 0;
        }

        private static string ExtractStringMathObject(string expression, ref int startIndex, Func<int, bool> endCondition)
        {
            var start = startIndex;
            while (++startIndex < expression.Length && endCondition(startIndex)) { }

            var substring = expression.Substring(start, startIndex - start);
            return substring;
        }

        private static void InsertMultiplicationIfRequired(string stringExpression, List<MathObject> expression, int index)
        {
            if (index != 0 && IsNumeric(stringExpression[index - 1]))
                expression.Add(new Operator('*'));
        }

        private static bool IsNumeric(char c) => int.TryParse(c.ToString(), out int _);
    }
}
