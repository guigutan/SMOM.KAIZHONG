using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.MES.DashBoard.DashBoards.Converters
{
    /// <summary>
    /// 定义字体颜色转换器，根据状态判断，1.状态为正常，绿色；2.为停线，红色；3.为休息，蓝色
    /// </summary>
    public class StateColorToConvert : IValueConverter
    {
        /// <summary>
        /// 状态转换为颜色
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>返回转换的颜色</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new BrushConverter();
            if (value == null)
            {
                return (Brush)converter.ConvertFromString("#228B22".L10N());//绿色
            }

            string reValue = value?.ToString();
            if (reValue == "停线")
            {
                return (Brush)converter.ConvertFromString("#FF0000".L10N());//红色
            }
            else if (reValue == "休息")
            {
                return (Brush)converter.ConvertFromString("#0000FF".L10N());//蓝色
            }
            else
            {
                return (Brush)converter.ConvertFromString("#228B22".L10N());//绿色
            }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言化</param>
        /// <returns>返回结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}