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
    /// WPF Binding值转换器
    /// </summary>
    /// <seealso cref="Resources.IconPacks.Converter.MarkupConverter" />
    public class FlipToScaleXValueConverter : MarkupConverter
    {
        private static FlipToScaleXValueConverter _instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static FlipToScaleXValueConverter()
        {
        }

        /// <summary>
        /// 提供值
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider对象</param>
        /// <returns>object对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new FlipToScaleXValueConverter());
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
            if (value is PackIconFlipOrientation)
            {
                var flip = (PackIconFlipOrientation)value;
                var scaleX = flip == PackIconFlipOrientation.Horizontal || flip == PackIconFlipOrientation.Both ? -1 : 1;
                return scaleX;
            }
            return DependencyProperty.UnsetValue;
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
    /// <summary>
    /// ValueConverter which converts the PackIconFlipOrientation enumeration value to ScaleY value of a ScaleTransformation.
    /// </summary>
    /// <seealso cref="Resources.IconPacks.Converter.MarkupConverter" />
    public class FlipToScaleYValueConverter : MarkupConverter
    {
        private static FlipToScaleYValueConverter _instance;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static FlipToScaleYValueConverter()
        {
        }

        /// <summary>
        /// 提供值
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider对象</param>
        /// <returns>object对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new FlipToScaleYValueConverter());
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
            if (value is PackIconFlipOrientation)
            {
                var flip = (PackIconFlipOrientation)value;
                var scaleY = flip == PackIconFlipOrientation.Vertical || flip == PackIconFlipOrientation.Both ? -1 : 1;
                return scaleY;
            }
            return DependencyProperty.UnsetValue;
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