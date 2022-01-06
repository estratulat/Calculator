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

            for (int i = 0; i < stringExpression.Length; i++)
            {
                var element = stringExpression[i];

                if (Operator.IsOperator(element))
                {
                    expression.Add(new Operator(element));
                    continue;
                }
                if(IsNumeric(stringExpression[i]))
                {
                    var start = i;
                    while(++i < stringExpression.Length && IsNumeric(stringExpression[i])) { }

                    expression.Add(new Operand(int.Parse(stringExpression.Substring(start, i - start))));
                    i--;
                }
                if(stringExpression[i] == '(')
                {
                    var start = i;
                    while (++i < stringExpression.Length && !(stringExpression[i] == ')')) { }

                    var stringSubExpression = new string(stringExpression.Skip(start + 1).Take(i - start - 1).ToArray());
                    expression.Add(new Expression(Parse(stringSubExpression)));
                }
            }
            return expression;
        }

        private static bool IsNumeric(char c) => int.TryParse(c.ToString(), out int _);
    }
}
