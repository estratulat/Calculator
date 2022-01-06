using System;
using System.Collections.Generic;

namespace Calculator.Calculator.Expressions.Models
{
    public class Operator : MathObject
    {
        public static readonly int MAX_PRIORITY = 1;
        private static readonly Dictionary<char, (int Priority, Func<Operand, Operand, int> Action)> possibleValues = new Dictionary<char, (int, Func<Operand, Operand, int>)> { 
            { '+', (0, (x,y) => x.Value + y.Value) }, 
            { '-', (0, (x,y) => x.Value - y.Value) }, 
            { '*', (1, (x,y) => x.Value * y.Value) }, 
            { '/', (1, (x,y) => x.Value / y.Value) } 
        };
        public char Value { get; }
        public int Priority => possibleValues[Value].Priority;
        public int Apply(Operand First, Operand Last) => possibleValues[Value].Action(First, Last);

        public static bool IsOperator(char op) => possibleValues.ContainsKey(op);
        public Operator(char value)
        {
            Value = IsOperator(value) ? value : '+';
        }

    }
}
