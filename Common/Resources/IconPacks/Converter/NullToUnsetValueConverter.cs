using System;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Globalization;
using System.Windows;
#endif

namespace Resources.IconPacks.Converter
{
    /// <summary>
    /// 空的未设置值转换器
    /// </summary>
    public class NullToUnsetValueConverter : MarkupConverter
    {
        private static NullToUnsetValueConverter _instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static NullToUnsetValueConverter()
        {
        }

        /// <summary>
        /// 提供值
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider对象</param>
        /// <returns>object对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new NullToUnsetValueConverter());
        }

        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域</param>
        /// <returns>静态值</returns>
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// 转换回滚
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">区域</param>
        /// <returns>静态值</returns>
        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}