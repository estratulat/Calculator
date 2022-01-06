using Calculator.Calculator.Expressions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Calculator
{
    public class Calculator
    {
        private IEnumerable<int> GetOperationSequenceIndices(List<MathObject> expression) 
        {
            List<int> sequence = new List<int>();
            var currentPriority = Operator.MAX_PRIORITY;
            while(currentPriority >= 0)
            {
                for (int i = 0; i < expression.Count; i++)
                {
                    MathObject element = expression[i];
                    var op = element as Operator;
                    if (op is null) continue;

                    if (op.Priority == currentPriority)
                    {
                        sequence.Add(i);
                    }
                }
                currentPriority--;
            }
            return sequence;
        }

        public int Calculate(List<MathObject> expression)
        {
            var operatorSequence = GetOperationSequenceIndices(expression);
            operatorSequence = operatorSequence.Reverse();
            return ConstructExpressionTree(expression, 0, expression.Count - 1, operatorSequence, 0).Value;
        }

        private Operand ConstructExpressionTree(List<MathObject> expression, int startIndex, int ensIndex, IEnumerable<int> operatorSequenceIndices, int operatorStep)
        {
            if (ensIndex == startIndex)
            {
                switch (expression[startIndex])
                {
                    case Operand op:
                        return op;
                    case Expression exp:
                        return new Operand(Calculate(exp.mathObjects));
                    default:
                        break;
                }
            }
            return new ExpressionNode(
                ConstructExpressionTree(expression, startIndex, operatorSequenceIndices.ElementAt(operatorStep) - 1, operatorSequenceIndices, operatorStep + 1),
                ConstructExpressionTree(expression, operatorSequenceIndices.ElementAt(operatorStep) + 1, ensIndex, operatorSequenceIndices, operatorStep + 1),
                expression[operatorSequenceIndices.ElementAt(operatorStep)] as Operator);
        }

        public class ExpressionNode : Operand
        {
            public Operand First { get; set; }
            public Operand Last { get; set; }

            public Operator Operator { get; set; }

            public override int Value { get => PerformOperation().Value; }
            public ExpressionNode(Operand first, Operand last, Operator @operator)
            {
                First = first;
                Last = last;
                Operator = @operator;
            }

            private Operand PerformOperation()
            {
                switch (Operator.Value)
                {
                    case '+':
                        return new Operand(First.Value + Last.Value);
                    case '-':
                        return new Operand(First.Value - Last.Value);
                    case '*':
                        return new Operand(First.Value * Last.Value);
                    case '/':
                        return new Operand(First.Value / Last.Value);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
