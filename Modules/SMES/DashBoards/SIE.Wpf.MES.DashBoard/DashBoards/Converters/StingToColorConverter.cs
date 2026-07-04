using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.MES.DashBoard.DashBoards.Converters
{
    /// <summary>
    /// 字符串颜色转换器
    /// </summary>
    public class StingToColorConverter : IValueConverter
    {
        /// <summary>
        /// string类型转换为Brush类型
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>转换后的结果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString("#333434".L10N());//默认颜色
            if (value == null || string.IsNullOrEmpty(value?.ToString()))
            {
                return brush;
            }

            string reValue = value.ToString();
            try
            {
                return (Brush)converter.ConvertFromString(reValue);
            }
            catch (Exception)
            {
                return brush; //转换异常时显示默认颜色
            }
        }

        /// <summary>
        /// UI值变化回调方法
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>无</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}