using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace SIE.Wpf.MES.Workbench.Converters
{
    public class OnDutyToForegoundConverter : MarkupExtension, IMultiValueConverter
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

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string foreground = "#A6A6A6";
            foreach (var item in values)
            {
                if (item.GetType().Name == "NamedObject")
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(foreground));
            }
            string ondutyId = values[0]?.ToString();   //当班员工ID 
            string actualOndutyId = values[1]?.ToString(); //实际当班员工ID
            bool isAbsenteeism = (bool)values[2]; //实际当班员工是否缺勤
            if (!ondutyId.IsNullOrEmpty() && !actualOndutyId.IsNullOrEmpty() && !isAbsenteeism)
            {
                if (ondutyId == actualOndutyId)
                    foreground = "#5BB65E";
                else
                    foreground = "#E33043";
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(foreground));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}