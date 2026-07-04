using SIE.Andon.Andons.Enum;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SIE.Wpf.Andon.Controls
{
    /// <summary>
    /// 安灯状态转换器
    /// </summary>
    public class AndonStateToColorConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换
        /// </summary>
        /// <param name="value">是否允许输入数值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化</param>
        /// <returns>允许输入数值则可见，否则不可见</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AndonManageState)
            {
                AndonManageState status = (AndonManageState)value;
                SolidColorBrush color;

                switch (status)
                {
                    case AndonManageState.Standby:
                        color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E22E2B"));
                        break;
                    case AndonManageState.Processing:
                        color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9BE09"));
                        break;
                    case AndonManageState.ToAccepted:
                        color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#06CA70"));
                        break;
                    default:
                        color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E22E2B"));
                        break;
                }

                return color;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        /// 逆向转换
        /// </summary>
        /// <param name="value">是否允许输入数值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">文化</param>
        /// <returns>允许输入数值则可见，否则不可见</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
