using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.WorkBenchChartBase.ChartGroups
{
    /// <summary>
    /// ChartInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ChartInfo : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChartInfo()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// 转换
    /// </summary>
    public class VisibleConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 是否转换
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value?.ToString().IsNotEmpty() == true;
            if (Invert)
                result = !result;
            if (result)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
