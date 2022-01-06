using Calculator.Calculator.Expressions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Calculator.Parsing
{
    public class Parser
    {
        public static List<MathObject> Parse(string stringExpression)
        {
            List<MathObject> expression = new List<MathObject>();

            int counterStart = NegativeStartNumberAdjustment(stringExpression, expression);
            for (int i = counterStart; i < stringExpression.Length; i++)
            {
                if (Operator.IsOperator(stringExpression[i]))
                {
                    expression.Add(new Operator(stringExpression[i]));
                    if (i < stringExpression.Length - 2 && stringExpression[i + 1] == '-')
                    {
                        i = AddNumberAndGetNextIndex(stringExpression, expression, i + 2, false);
                    }
                    continue;
                }
                if (IsNumeric(stringExpression[i]))
                {
                    i = AddNumberAndGetNextIndex(stringExpression, expression, i);
                    continue;
                }
                if (stringExpression[i] == '(')
                {
                    InsertMultiplicationIfRequired(stringExpression, expression, i);

                    var start = i;
                    while (++i < stringExpression.Length && !(stringExpression[i] == ')')) { }

                    var stringSubExpression = new string(stringExpression.Skip(start + 1).Take(i - start - 1).ToArray());
                    expression.Add(new Expression(Parse(stringSubExpression)));
                }
            }
            return expression;
        }

        private static int NegativeStartNumberAdjustment(string stringExpression, List<MathObject> expression)
        {
            if (stringExpression.ElementAtOrDefault(0) == '-')
            {
                var counter = AddNumberAndGetNextIndex(stringExpression, expression, 1, false);
                return ++counter;
            }

            return 0;
        }

        private static int AddNumberAndGetNextIndex(string stringExpression, List<MathObject> expression, int numberStartIndex, bool asPositive = true)
        {
            var start = numberStartIndex;
            while (++numberStartIndex < stringExpression.Length && IsNumeric(stringExpression[numberStartIndex])) { }

            var substring = stringExpression.Substring(start, numberStartIndex - start);
            
            expression.Add(new Operand(int.Parse(asPositive ? substring : $"-{substring}")));
            numberStartIndex--;
            return numberStartIndex;
        }

        private static void InsertMultiplicationIfRequired(string stringExpression, List<MathObject> expression, int index)
        {
            if (index != 0 && IsNumeric(stringExpression[index - 1]))
                expression.Add(new Operator('*'));
        }

        private static bool IsNumeric(char c) => int.TryParse(c.ToString(), out int _);
    }
}
