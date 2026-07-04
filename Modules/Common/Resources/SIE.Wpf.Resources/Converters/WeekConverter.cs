using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.Resources.Converters
{
    /// <summary>
    /// 星期转换器，多语言实现
    /// </summary>
    public class WeekConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">文化</param>
        /// <returns>转换后值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //显示周一至周日，需要先拼接后翻译
            return "周{0}".FormatArgs(value).L10N();
        }

        /// <summary>
        /// 逆向转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">文化</param>
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