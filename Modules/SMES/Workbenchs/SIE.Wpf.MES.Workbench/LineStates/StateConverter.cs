using SIE.Equipments.Abnormal;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace SIE.Wpf.MES.Workbench.LineStates
{
    /// <summary>
    /// 
    /// </summary>
    public class StateConverterExtension : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 产线状态转换类
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ExceptionStopType))
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7BB961"));
            var state = (ExceptionStopType)value;
            if (state == ExceptionStopType.Normal)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5BB65E"));
            else if (state == ExceptionStopType.Maintain)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#128BEF"));
            else if (state == ExceptionStopType.StopLine)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7200"));
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7200"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
