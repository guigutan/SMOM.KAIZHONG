using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// DetailCalculatorControl.xaml 的交互逻辑
    /// </summary>
    public partial class DetailCalculatorControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DetailCalculatorControl()
        {
            InitializeComponent();
            txtResult.DataContext = this;
            txtResult.PreviewKeyDown += TxtResult_PreviewKeyDown;
        }

        /// <summary>
        /// 文本事件
        /// </summary>
        /// <param name="sender">文本框</param>
        /// <param name="e">参数</param>
        protected virtual void TxtResult_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        /// <summary>
        /// 是否值格式错误
        /// </summary>
        public bool HasError
        {
            get { return double.IsNaN(Value) || double.IsInfinity(Value); }
        }

        #region 数值 Value
        /// <summary>
        /// 数值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// 数值
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(DetailCalculatorControl), new PropertyMetadata(0d, (s, e) => ((DetailCalculatorControl)s).OnValueChanged(e)));

        /// <summary>
        /// 值变更事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!supressValueChanged)
                Expression = e.NewValue.ToString();
        }
        #endregion

        #region 显示值 Expression
        /// <summary>
        /// 显示值
        /// </summary>
        public string Expression
        {
            get { return (string)GetValue(ExpressionProperty); }
            set { SetValue(ExpressionProperty, value); }
        }

        /// <summary>
        /// 显示值
        /// </summary>
        public static readonly DependencyProperty ExpressionProperty =
            DependencyProperty.Register("Expression", typeof(string), typeof(DetailCalculatorControl), new PropertyMetadata("0"));
        #endregion

        /// <summary>
        /// 值变更
        /// </summary>
        protected bool supressValueChanged;

        /// <summary>
        /// 计算器键盘点击事件
        /// </summary>
        /// <param name="sender">按钮</param>
        /// <param name="e">参数</param>
        protected virtual void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var p = btn.CommandParameter.ToString();
            if (p == "=")
                Equal();
            else if (p == ".")
                Dot();
            else if (p == "+" || p == "-" || p == "*" || p == "/")
                Operate(p);
            else if (p == "B")
                Back();
            else if (p == "C")
                Clear();
            else
                Num(p);
        }

        /// <summary>
        /// 清除值
        /// </summary>
        protected virtual void Clear()
        {
            Value = 0;
        }

        /// <summary>
        /// 等于
        /// </summary>
        protected virtual void Equal()
        {
            if (!CanEqual) return;
            Compute();
            if (double.IsNaN(Value))
                Expression = "NaN";
            else if (double.IsNegativeInfinity(Value))
                Expression = "-∞";
            else if (double.IsPositiveInfinity(Value))
                Expression = "∞";
            else
                Expression = Value.ToString();
            equaled = true;
        }

        /// <summary>
        /// 小数点
        /// </summary>
        protected virtual void Dot()
        {
            if (!CanDot) return;
            Expression += ".";
            equaled = false;
        }

        /// <summary>
        /// 回退
        /// </summary>
        protected virtual void Back()
        {
            if (!CanEqual || equaled || Expression.Length <= 1)
                Expression = "0";
            else
                Expression = Expression.Substring(0, Expression.Length - 1);
            Compute();
        }

        /// <summary>
        /// 数值键
        /// </summary>
        /// <param name="num">数值</param>
        protected virtual void Num(string num)
        {
            if (NewNum)
                Expression = num;
            else
                Expression += num;
            equaled = false;
            Compute();
        }

        /// <summary>
        /// 操作符
        /// </summary>
        /// <param name="op">操作符</param>
        protected virtual void Operate(string op)
        {
            if (!CanEqual) return;
            equaled = false;
            Expression = Expression.TrimEnd('.');
            if (op == "-")
            {
                if (Expression.EndsWith("*-") || Expression.EndsWith("/-"))
                {
                    return;
                }
                else if (Expression.EndsWith("*") || Expression.EndsWith("/"))
                {
                    Expression += op;
                    return;
                }
            }

            Expression = Expression.TrimEnd('+', '*', '/', '-') + op;
        }

        /// <summary>
        /// 是否等于运算
        /// </summary>
        protected bool equaled;

        /// <summary>
        /// 新数值
        /// </summary>
        protected bool NewNum
        {
            get { return Expression == "0" || !CanEqual || equaled; }
        }

        /// <summary>
        /// 是否可以输入小数点
        /// </summary>
        protected bool CanDot
        {
            get
            {
                if (!CanEqual) return false;
                foreach (var c in Expression.Reverse())
                {
                    if (!char.IsDigit(c))
                    {
                        if (c == '.')
                            return false;
                        return true;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 是否可以等于运算
        /// </summary>
        protected bool CanEqual
        {
            get { return Expression != "∞" && Expression != "NaN" && Expression != "-∞"; }
        }

        /// <summary>
        /// 结果运算
        /// </summary> 
        protected virtual void Compute()
        {
            var expr = Expression.TrimEnd('+', '*', '/', '-', '.');
            System.Data.DataTable dt = new System.Data.DataTable();
            double value;
            if (!double.TryParse(dt.Compute(expr, null).ToString(), out value))
            {
                value = double.PositiveInfinity;
            }
            supressValueChanged = true;
            Value = value;
            supressValueChanged = false;
            dt.Dispose();
        }

        /// <summary>
        /// 键盘按击事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            var handled = e.Handled;
            e.Handled = true;
            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                Num((e.Key - Key.NumPad0).ToString());
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
                Num((e.Key - Key.D0).ToString());
            else if (e.Key == Key.Add || e.Key == Key.OemPlus)
                Operate("+");
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
                Operate("-");
            else if (e.Key == Key.Multiply)
                Operate("*");
            else if (e.Key == Key.Divide)
                Operate("/");
            else if (e.Key == Key.Decimal)
                Dot();
            else if (e.Key == Key.Enter)
                Equal();
            else if (e.Key == Key.Back)
                Back();
            else if (e.Key == Key.Delete)
                Clear();
            else
            {
                e.Handled = handled;
                base.OnKeyDown(e);
            }
        }
    }
}