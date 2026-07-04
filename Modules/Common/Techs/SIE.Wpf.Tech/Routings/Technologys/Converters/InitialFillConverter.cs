using SIE.Tech.Routings.Technologys;
using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace SIE.Wpf.Tech.Routings.Technologys.Converters
{
    /// <summary>
    /// 活动接口转换SolidColorBrush对象值转换器
    /// </summary>
    public class InitialFillConverterExtension : MarkupExtension, IValueConverter
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
        /// <param name="value">IActivity接口</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>SolidColorBrush对象</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var activity = value as IActivity;
            if (activity.IsSelected)
                return new SolidColorBrush(Color.FromArgb(255, 255, 181, 0));
            if (activity.ProcessState == ProcessState.Current)
                return new SolidColorBrush(Colors.LightSteelBlue);
            return new SolidColorBrush(Colors.Green);
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">SolidColorBrush对象</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>IActivity接口</returns>
        /// <exception cref="NotImplementedException">未实现</exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
