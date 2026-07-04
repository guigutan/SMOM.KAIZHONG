using SIE.Tech.Routings.Technologys;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.Tech.Routings.Technologys.Converters
{
    /// <summary>
    /// 规则转换Visibility枚举值转换器
    /// </summary>
    class IRuleToVisibilityConverterExtension : MarkupExtension, IValueConverter
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
        /// <param name="value">规则</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>Visibility枚举</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var rule = value as IRule;
            if (rule == null || rule.Type == RuleType.Line || !rule.IsSelected)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">Visibility枚举</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地化</param>
        /// <returns>规则</returns>
        /// <exception cref="NotImplementedException">未实现</exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
