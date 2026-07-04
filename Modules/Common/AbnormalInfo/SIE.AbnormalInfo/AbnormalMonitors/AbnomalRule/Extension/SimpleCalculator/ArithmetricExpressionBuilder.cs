using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.AbnormalInfo.AbnormalMonitors.SimpleCalculator
{
    class ArithmetricExpressionBuilder
    {
        private static char[] ArthmetricOperators = new char[] { '+', '-', '*', '/', '^', '%', '(', ')' };
        private static IDictionary<char, ExpressionType> ArthmetricOperatorsMapping =
            new Dictionary<char, ExpressionType>()
            {
                { '+', ExpressionType.Add },
                { '-', ExpressionType.Subtract },
                { '*', ExpressionType.Multiply },
                { '/', ExpressionType.Divide },
                { '%', ExpressionType.Modulo },
                { '^', ExpressionType.Power },
            };
        private static IDictionary<char, byte> ArithmetricOperatorsPriority =
            new Dictionary<char, byte>()
            {
                { '+', byte.MinValue },
                { '-', byte.MinValue },
                { '*', 1 },
                { '/', 1 },
                { '%', 1 },
                { '^', 2 },
                { '(', byte.MaxValue },
                { ')', byte.MaxValue },
            };

        private string _expr;
        private string[] _postFixExpr;
        // product role
        private BinaryExpression _exprTree;

        /// <summary>
        /// 构建后缀表达式
        /// </summary>
        private void BuildPostFixExpr()
        {
            if (_postFixExpr == null)
            {
                var stack = new Stack<char>();
                int sIndex = 0;
                var list = new List<string>();

                // 表达式开头可能出现 "-" 号
                if (_expr.Length > 0 && IsArithmetricOperator(_expr[0]))
                    _expr = _expr.Insert(0, "0");

                for (int i = 0; i< _expr.Length; i++)
                {
                    if (IsArithmetricOperator(_expr[i]))
                    {
                        sIndex = i + 1;
                        if (stack.Count <= 0)
                        {
                            stack.Push(_expr[i]);
                        }
                        else
                        {
                            if (_expr[i] == ')')
                            {
                                while (stack.Peek() != '(')
                                {
                                    list.Add(stack.Pop().ToString());
                                }
                                stack.Pop();
                            }
                            else if(i+1 < _expr.Length && _expr[i] == '(' && 
                                _expr[i+1] != '(' && IsArithmetricOperator(_expr[i+1]))
                            {
                                _expr = _expr.Insert(i+1, "0");   // 5-(-5) 的情况
                                stack.Push(_expr[i]);
                            }
                            else if (stack.Peek() != '(' && ArithmetricOperatorsPriority[stack.Peek()] >=
                                     ArithmetricOperatorsPriority[_expr[i]])
                            {
                                while (stack.Count > 0 && stack.Peek() != '(' && ArithmetricOperatorsPriority[stack.Peek()] >=
                                     ArithmetricOperatorsPriority[_expr[i]])
                                    list.Add(stack.Pop().ToString());
                                stack.Push(_expr[i]);
                            }
                            else
                                stack.Push(_expr[i]);
                        }
                    }
                    else
                    {
                        if ((_expr.Length > i+1 && IsArithmetricOperator(_expr[i+1])) || i == _expr.Length - 1)
                            list.Add(_expr.Substring(sIndex, i-sIndex+1));
                    }
                }

                while (stack.Count > 0)
                {
                    if (stack.Peek() == '(')
                    {
                        stack.Pop();
                        continue;
                    }
                    list.Add(stack.Pop().ToString());
                }
                _postFixExpr = list.ToArray();
            }
        }

        public ArithmetricExpressionBuilder(string expr)
        {
            _expr=expr ?? throw new ArgumentNullException(nameof(expr));
        }

        /// <summary>
        /// 从后缀表达式生成一个表达式树
        /// </summary>
        /// <param name="postFix">后缀表达式</param>
        private void BuildExpressionFromPostFix(string[] postFix)
        {
            var stack = new Stack<Expression>();

            BinaryExpression ex;
            Expression l, r;
            for (int i = 0;i < postFix.Length;i++)
            {
                if(IsArithmetricOperator(postFix[i]))
                {
                    r = stack.Pop();
                    l = stack.Pop();
                    ex = Expression.MakeBinary(
                        ArthmetricOperatorsMapping[postFix[i][0]],
                        l, r);

                    stack.Push(ex);
                }
                else
                {
                    stack.Push(
                        Expression.Constant(
                            double.Parse(postFix[i])));
                }
            }
            if (stack.Peek() is BinaryExpression binaryExpression)
                _exprTree = binaryExpression;
            else if (stack.Peek() is ConstantExpression constantExpression)
                _exprTree = Expression.Add(constantExpression, Expression.Constant(0d)); // 单独一个表达式 5 
        }

        public BinaryExpression Build()
        {
            BuildPostFixExpr();
            BuildExpressionFromPostFix(_postFixExpr);
            return _exprTree;
        }

        internal static bool IsArithmetricOperator(string text)
        {
            return text.Length <= 1 && ArthmetricOperators.Contains(text[0]);
        }

        internal static bool IsArithmetricOperator(char c)
        {
            return ArthmetricOperators.Contains(c);
        }
    }
}
