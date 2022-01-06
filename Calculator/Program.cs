using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var expression = "-2(110+(10+110))*-110/110 + 1-20";
            var calc = new Calculator.Calculator();
            var result = calc.Calculate(expression);
            Console.WriteLine(result);
        }
    }
}
//(-2)*(110+(10+110))*(-110)/110 + 1 - 20
