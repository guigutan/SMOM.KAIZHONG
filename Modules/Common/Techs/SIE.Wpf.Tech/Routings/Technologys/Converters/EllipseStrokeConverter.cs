using SIE.Tech.Processs;
using SIE.Utils;
using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace SIE.Wpf.Tech.Routings.Technologys.Converters
{
    /// <summary>
    /// 采集结果枚举转SolidColorBrush对象值转换器
    /// </summary>
    public class EllipseStrokeConverterExtension : MarkupExtension, IValueConverter
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
        /// <param name="value">采集结果</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>SolidColorBrush对象</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value?.ToString() == EnumViewModel.EnumToLabel(ResultTypeForDesign.Pass).L10N())
            {
                return new SolidColorBrush(Colors.Green);
            }

            if (value?.ToString() == EnumViewModel.EnumToLabel(ResultTypeForDesign.Fail).L10N())
            {
                return new SolidColorBrush(Colors.Red);
            }

            if (value?.ToString() == EnumViewModel.EnumToLabel(ResultTypeForDesign.Any).L10N())
            {
                return new SolidColorBrush(Colors.Yellow);
            }

            return new SolidColorBrush(Colors.Blue);
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">SolidColorBrush对象</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>采集结果</returns>
        /// <exception cref="NotImplementedException">未实现</exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
