using Calculator.Calculator.Expressions.Models;
using Calculator.Calculator.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Calculator
{
    public class Calculator
    {
        public IParser Parser { get; set; }

        public Calculator() : this(new DefaultParser()) { }
        public Calculator(IParser parser)
        {
            Parser = parser;
        }

        public int Calculate(string expression) => Calculate(Parser.Parse(expression));
        public int Calculate(List<MathObject> expression)
        {
            var operatorSequence = GetOperationSequenceIndices(expression);

            var expressiontreeBuffer = new ExpressionTreeBuffer(expression, operatorSequence.Reverse(), expression => Calculate(expression));

            return expressiontreeBuffer.GetExpressionTreeResult(0, expression.Count - 1, 0).Value;
        }

        private IEnumerable<int> GetOperationSequenceIndices(List<MathObject> expression)
        {
            List<int> sequence = new List<int>();
            var currentPriority = Operator.MAX_PRIORITY;

            while (currentPriority >= 0)
            {
                AddOperatorsToSequence(expression, sequence, currentPriority);
                currentPriority--;
            }
            return sequence;
        }

        private static void AddOperatorsToSequence(List<MathObject> expression, List<int> sequence, int currentPriority)
        {
            for (int i = 0; i < expression.Count; i++)
            {
                var op = expression[i] as Operator;
                if (op?.Priority == currentPriority)
                {
                    sequence.Add(i);
                }
            }
        }

        private class ExpressionTreeBuffer
        {
            public ExpressionTreeBuffer(List<MathObject> expression, IEnumerable<int> operatorSequenceIndices, Func<List<MathObject>, int> bracketExpressionCalculator)
            {
                Expression = expression;
                OperatorSequenceIndices = operatorSequenceIndices;
                ExpressionCalculator = bracketExpressionCalculator;
            }

            List<MathObject> Expression { get; }
            IEnumerable<int> OperatorSequenceIndices { get; }
            Func<List<MathObject>, int> ExpressionCalculator { get; }

            public Operand GetExpressionTreeResult(int startIndex, int endIndex, int operatorTurn)
            {
                var oneElementTree = endIndex == startIndex;
                if (oneElementTree)
                {
                    switch (Expression[startIndex])
                    {
                        case Operand op:
                            return op;
                        case Expression exp:
                            return new Operand(ExpressionCalculator(exp.mathObjects));
                        default:
                            break;
                    }
                }
                return new ExpressionNode(
                    GetExpressionTreeResult(startIndex, OperatorSequenceIndices.ElementAt(operatorTurn) - 1, operatorTurn + 1),
                    GetExpressionTreeResult(OperatorSequenceIndices.ElementAt(operatorTurn) + 1, endIndex, operatorTurn + 1),
                    Expression[OperatorSequenceIndices.ElementAt(operatorTurn)] as Operator);
            }
            private class ExpressionNode : Operand
            {
                public Operand First { get; set; }
                public Operand Last { get; set; }

                public Operator Operator { get; set; }

                public override int Value => Operator.Apply(First, Last);
                public ExpressionNode(Operand first, Operand last, Operator @operator)
                {
                    First = first;
                    Last = last;
                    Operator = @operator;
                }
            }
        }
    }
}
