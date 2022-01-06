using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Calculator.Expressions.Models
{
    public class Operand : MathObject
    {
        protected Operand() {  }
        public Operand(int value)
        {
            Value = value;
        }
        public virtual int Value { get; set; }
    }
}
