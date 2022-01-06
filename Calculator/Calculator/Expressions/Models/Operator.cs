using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Calculator.Expressions.Models
{
    public class Operator : MathObject
    {
        public static readonly int MAX_PRIORITY = 1;
        private static readonly Dictionary<char, int> possibleValues = new Dictionary<char, int> { { '+', 0 }, { '-', 0 }, { '*', 1 }, { '/', 1 } };
        private char _value;

        public char Value { 
            get => _value;
            set => _value = IsOperator(value) ? value : _value;
        }
        public int Priority => possibleValues[Value];

        public static bool IsOperator(char op) => possibleValues.ContainsKey(op);
        public Operator(char value)
        {
            Value = value;
            if (Value is default(char)) _value = '+';
        }
    }
}
