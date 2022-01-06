using Calculator.Calculator.Expressions.Models;
using System;
using System.Collections.Generic;
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
                if(int.TryParse(stringExpression[i].ToString(), out int _))
                {
                    var start = i;
                    while(++i < stringExpression.Length && int.TryParse(stringExpression[i].ToString(), out int _)) { }

                    expression.Add(new Operand(int.Parse(stringExpression.Substring(start, i - start))));
                    i--;
                }
            }
            return expression;
        }
    }
}
