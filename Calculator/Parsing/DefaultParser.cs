using Calculator.Calculator.Expressions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Calculator.Parsing
{
    public class DefaultParser : IParser
    {
        private char openBracketSymbol = '(';
        public List<MathObject> Parse(string stringExpression)
        {
            var extressionBuffer = new ExpressionBuffer(stringExpression);

            for (int elementIndex = extressionBuffer.StartIndex; elementIndex < stringExpression.Length; elementIndex++)
            {
                if (Operator.IsOperator(stringExpression[elementIndex]))
                {
                    elementIndex = extressionBuffer.AddOperatorAndGetNextIndex(elementIndex);
                }
                else if (IsNumeric(stringExpression[elementIndex]))
                {
                    elementIndex = extressionBuffer.AddNumberAndGetNextIndex(elementIndex);
                }
                else if (stringExpression[elementIndex] == openBracketSymbol)
                {
                    elementIndex = extressionBuffer.AddBracketsExpressionAndGetNextIndex(elementIndex, this);
                }
            }

            return extressionBuffer.GetExpression();
        }

        private static bool IsNumeric(char c) => int.TryParse(c.ToString(), out int _);

        private class ExpressionBuffer
        {
            private List<MathObject> ConstructedExpression { get; }
            private string Expression { get; }

            public int StartIndex { get; }
            public ExpressionBuffer(string expression)
            {
                Expression = expression;
                ConstructedExpression = new List<MathObject>();
                StartIndex = AdjustForFirstNegative();
            }

            public List<MathObject> GetExpression() => ConstructedExpression;

            public int AddOperatorAndGetNextIndex(int operatorIndex)
            {
                ConstructedExpression.Add(new Operator(Expression[operatorIndex]));

                if (operatorIndex < Expression.Length - 2 && Expression[operatorIndex + 1] == '-')
                {
                    operatorIndex = AddNumberAndGetNextIndex(operatorIndex + 2, false);
                }

                return operatorIndex;
            }

            public int AddBracketsExpressionAndGetNextIndex(int breacketIndex, IParser expressionParser)
            {
                InsertMultiplicationIfRequired(Expression, ConstructedExpression, breacketIndex);

                breacketIndex++;
                string stringSubExpression = ExtractStringMathObject(Expression, ref breacketIndex, currentIndex => !(Expression[currentIndex] == ')'));

                ConstructedExpression.Add(new Expression(expressionParser.Parse(stringSubExpression)));

                return breacketIndex;
            }

            public int AddNumberAndGetNextIndex(int numberIndex, bool asPositive = true)
            {
                string operand = ExtractStringMathObject(Expression, ref numberIndex, currentIndex => IsNumeric(Expression[currentIndex]));
                ConstructedExpression.Add(new Operand(int.Parse(asPositive ? operand : $"-{operand}")));

                numberIndex--;
                return numberIndex;
            }

            private int AdjustForFirstNegative()
            {
                if (Expression.ElementAtOrDefault(0) == '-')
                {
                    var counter = AddNumberAndGetNextIndex(1, false);
                    return ++counter;
                }

                return 0;
            }

            private string ExtractStringMathObject(string expression, ref int startIndex, Func<int, bool> endCondition)
            {
                var start = startIndex;
                while (++startIndex < expression.Length && endCondition(startIndex)) { }

                var substring = expression.Substring(start, startIndex - start);
                return substring;
            }

            private void InsertMultiplicationIfRequired(string stringExpression, List<MathObject> expression, int index)
            {
                if (index != 0 && IsNumeric(stringExpression[index - 1]))
                    expression.Add(new Operator('*'));
            }

        }
    }
}
