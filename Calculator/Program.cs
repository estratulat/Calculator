using Calculator.Calculator.Parsing;
using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var expression = "-2(110+(10+110))*-110/110 + 1-20";
            var res = Parser.Parse(expression);
            var calc = new Calculator.Calculator();
            var result = calc.Calculate(res);
            Console.WriteLine(result);
        }
    }
}
//(-2)*(110+(10+110))*(-110)/110 + 1 - 20
