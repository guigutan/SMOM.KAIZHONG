using SIE.MES.WIP.Reworks;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// 是否显示转换器
    /// </summary>
    public class CheckBoxVisibleConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// IsUnbound的OneWay的转换器
        /// </summary>
        /// <param name="value">输入Value</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言对象</param>
        /// <returns>转换后的Value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rewkOprt = (ReworkOperate)value;
            if (rewkOprt == ReworkOperate.PermuteUnbound)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        /// <summary>
        ///  PackIcon的TwoWay的转换器
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>转换后的对象</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 目标属性值
        /// </summary>
        /// <param name="serviceProvider">提供的对象</param>
        /// <returns>返回目标属性值</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
