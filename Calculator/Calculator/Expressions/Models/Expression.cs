using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Calculator.Expressions.Models
{
    public class Expression : MathObject
    {
        public List<MathObject> mathObjects;

        public Expression(List<MathObject> mathObjects)
        {
            this.mathObjects = mathObjects;
        }
    }
}
