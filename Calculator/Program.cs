using Calculator.Calculator;
using Calculator.Calculator.Parsing;
using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var expression = "10+110*110/110";
            var res = Parser.Parse(expression);
            var calc = new Calculator.Calculator();
            var result = calc.Calculate(res);
            Console.WriteLine(result);
        }
    }
}
