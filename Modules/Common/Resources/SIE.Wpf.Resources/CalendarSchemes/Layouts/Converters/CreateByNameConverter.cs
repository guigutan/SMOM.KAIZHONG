using SIE.Domain;
using SIE.Resources;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SIE.Wpf.Resources.CalendarSchemes.Layouts.Converters
{
    /// <summary>
    /// 创建人转换器
    /// </summary>
    class CreateByNameConverter : IValueConverter
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">提供有关特定区域性的信息</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double id = value == null ? RT.IdentityId : (double)value; 
            return RF.GetById<Employee>(id);
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">提供有关特定区域性的信息</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
