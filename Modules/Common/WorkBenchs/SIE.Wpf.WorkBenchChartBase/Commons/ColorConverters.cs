using SIE.WorkBenchChartBase.Commons;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 预警级别与背景颜色转换器
    /// </summary>
    public class ColorConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var level = (ChartAlertLevel)value;
            switch (level)
            {
                case ChartAlertLevel.None:
                    return null;
                case ChartAlertLevel.Red:
                    return Brushes.Red;
                case ChartAlertLevel.Yellow:
                    return Brushes.Orange;
                case ChartAlertLevel.NoConfig:
                    return Brushes.Gray;
                case ChartAlertLevel.Green:
                default:
                    return Brushes.LightGreen;
            }
        }

        /// <summary>
        /// 颜色转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
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

    /// <summary>
    /// 预警级别与背景颜色描述转换器
    /// </summary>
    public class ColorDescriptionConverter : MarkupExtension, IValueConverter
    {
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
            var level = (ChartAlertLevel)value;
            switch (level)
            {
                case ChartAlertLevel.None:
                    return "".L10N();
                case ChartAlertLevel.Red:
                    return "红".L10N();
                case ChartAlertLevel.Yellow:
                    return "黄".L10N();
                case ChartAlertLevel.NoConfig:
                    return "无".L10N();
                case ChartAlertLevel.Green:
                default:
                    return "绿".L10N();
            }
        }

        /// <summary>
        /// 颜色转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
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
