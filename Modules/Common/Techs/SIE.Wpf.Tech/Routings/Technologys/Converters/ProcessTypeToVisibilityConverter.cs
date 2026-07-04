using SIE.Tech.Processs;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.Tech.Routings.Technologys.Converters
{
    /// <summary>
    /// 工序类型为批次的重复过站属性隐藏
    /// </summary>
    public class ProcessTypeToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 当在派生类中实现时，返回一个对象，该对象作为此标记扩展的目标属性的值。
        /// </summary>
        /// <param name="serviceProvider">标记扩展服务的服务提供者</param>
        /// <returns>对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">Bool值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>SolidColorBrush对象</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            var type = (ProcessType)value;
            if ((int)type >= 25) //批次工序
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">SolidColorBrush对象</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>Bool值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}