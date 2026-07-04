using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.MES.Wip.Converters
{
    /// <summary>
    /// 换料条码显示按钮是否可见转换器
    /// </summary>
    public class ReloadSnTipsVisibiityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns>转换后值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value?.ToString();
            if (string.IsNullOrEmpty(result))
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns>转换后值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当在派生类中实现时，返回一个对象，该对象作为此标记扩展的目标属性的值。
        /// </summary>
        /// <param name="serviceProvider">标记扩展服务的服务提供者</param>
        /// <returns>对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}