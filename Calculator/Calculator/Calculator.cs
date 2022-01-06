using Calculator.Calculator.Expressions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Calculator
{
    public class Calculator
    {
        public int Calculate(List<MathObject> expression)
        {
            var operatorSequence = GetOperationSequenceIndices(expression);

            operatorSequence = operatorSequence.Reverse();
            return ConstructExpressionTree(expression, 0, expression.Count - 1, operatorSequence, 0).Value;
        }
        private IEnumerable<int> GetOperationSequenceIndices(List<MathObject> expression)
        {
            List<int> sequence = new List<int>();
            var currentPriority = Operator.MAX_PRIORITY;

            while (currentPriority >= 0)
            {
                for (int i = 0; i < expression.Count; i++)
                {
                    var op = expression[i] as Operator;
                    if (op?.Priority == currentPriority)
                    {
                        sequence.Add(i);
                    }
                }
                currentPriority--;
            }
            return sequence;
        }

        private Operand ConstructExpressionTree(List<MathObject> expression, int startIndex, int endIndex, IEnumerable<int> operatorSequenceIndices, int operatorTurn)
        {
            if (endIndex == startIndex)
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
                ConstructExpressionTree(expression, startIndex, operatorSequenceIndices.ElementAt(operatorTurn) - 1, operatorSequenceIndices, operatorTurn + 1),
                ConstructExpressionTree(expression, operatorSequenceIndices.ElementAt(operatorTurn) + 1, endIndex, operatorSequenceIndices, operatorTurn + 1),
                expression[operatorSequenceIndices.ElementAt(operatorTurn)] as Operator);
        }

        private class ExpressionNode : Operand
        {
            public Operand First { get; set; }
            public Operand Last { get; set; }

            public Operator Operator { get; set; }

            public override int Value => GetExpressionResult();
            public ExpressionNode(Operand first, Operand last, Operator @operator)
            {
                First = first;
                Last = last;
                Operator = @operator;
            }

            private int GetExpressionResult()
            {
                switch (Operator.Value)
                {
                    case '+':
                        return First.Value + Last.Value;
                    case '-':
                        return First.Value - Last.Value;
                    case '*':
                        return First.Value * Last.Value;
                    case '/':
                        return First.Value / Last.Value;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
