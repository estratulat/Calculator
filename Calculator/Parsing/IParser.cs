using Calculator.Calculator.Expressions.Models;
using System.Collections.Generic;

namespace Calculator.Calculator.Parsing
{
    public interface IParser
    {
        List<MathObject> Parse(string stringExpression);
    }
}