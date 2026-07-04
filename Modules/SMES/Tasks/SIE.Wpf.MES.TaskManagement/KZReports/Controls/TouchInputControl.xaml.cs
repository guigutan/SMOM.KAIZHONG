using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// TouchInputControl.xaml 的交互逻辑
    /// </summary>
    public partial class TouchInputControl : UserControl
    {

        KZTaskReportViewModelBase model;
        /// <summary>
        /// 依赖属性：输入的数值
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(TouchInputControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 输入的数值
        /// </summary>
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public string OldValue { get; set; }
        /// <summary>
        /// 确定事件
        /// </summary>
        public event EventHandler Confirm;

        /// <summary>
        /// 取消事件
        /// </summary>
        public event EventHandler Cancel;

        public TouchInputControl(KZTaskReportViewModelBase _model, object value)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = this;
            OldValue = value.ToString();
            Value = value.ToString();
            NumberInput.Text = Value;
        }

        public TouchInputControl(object value)
        {
            InitializeComponent();
            this.DataContext = this;
            OldValue = value.ToString();
            Value = value.ToString();
            NumberInput.Text = Value;
        }

        /// <summary>
        /// 数字按钮点击事件
        /// </summary>
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (NumberInput.Text == "0")
                    NumberInput.Text = "";
                NumberInput.Text += button.Content.ToString();
                Value = NumberInput.Text;
                System.Diagnostics.Debug.WriteLine($"RoutedEventArgs: {button.Content.ToString()}");
            }
        }

        /// <summary>
        /// 退格按钮点击事件
        /// </summary>
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NumberInput.Text))
            {
                NumberInput.Text = NumberInput.Text.Substring(0, NumberInput.Text.Length - 1);
                Value = NumberInput.Text;
            }
        }

        /// <summary>
        /// 清空按钮点击事件
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NumberInput.Text = string.Empty;
            Value = string.Empty;
        }

        /// <summary>
        /// 小数点按钮点击事件
        /// </summary>
        private void DotButton_Click(object sender, RoutedEventArgs e)
        {
            // 确保只添加一个小数点
            if (!NumberInput.Text.Contains("."))
            {
                // 如果为空，先添加0
                if (string.IsNullOrEmpty(NumberInput.Text))
                {
                    NumberInput.Text = "0.";
                }
                else
                {
                    NumberInput.Text += ".";
                }
                Value = NumberInput.Text;
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // 验证输入是否为有效的数字
            if (string.IsNullOrEmpty(NumberInput.Text))
            {
                NumberInput.Text = "0";
                Value = "0";
                //MessageBox.Show("请输入数字", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //return;
            }

            // 触发确定事件
            Confirm?.Invoke(this, EventArgs.Empty);
            close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NumberInput.Text = OldValue.ToString();
            Value = OldValue.ToString();
            // 触发取消事件
            Cancel?.Invoke(this, EventArgs.Empty);
            close();
        }

        /// <summary>
        /// 当Value属性改变时更新输入框
        /// </summary>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ValueProperty)
            {
                NumberInput.Text = Value;
            }
        }
        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }

        private void NumberButton_Click(object sender, TouchEventArgs e)
        {
            if (sender is Button button)
            {
                if (NumberInput.Text == "0")
                    NumberInput.Text = "";
                NumberInput.Text += button.Content.ToString();
                Value = NumberInput.Text;
                System.Diagnostics.Debug.WriteLine($"TouchEventArgs: {button.Content.ToString()}");
            }
        }
    }
}
