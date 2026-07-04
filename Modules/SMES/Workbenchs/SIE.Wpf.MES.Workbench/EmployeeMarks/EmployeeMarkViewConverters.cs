using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.MES.Workbench.EmployeeMarks
{
    /// <summary>
    /// 作业评分的前5名的Icon是否显示
    /// </summary>
    public class Top5IconVisibleConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// OneWay的转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>转换后的对象</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var curRank = int.Parse(value.ToString());
            if (curRank <= 3)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        /// <summary>
        /// TwoWay的转换器
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>TwoWay的转换后的对象</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写ProvideValue方法
        /// </summary>
        /// <param name="serviceProvider">服务提供对象</param>
        /// <returns>当前对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// 作业评分的前5名的Icon的前景色
    /// </summary>
    public class Top5IconForegroundConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// OneWay的转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>转换后的对象</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var curRank = int.Parse(value.ToString());
            string foreGround = string.Empty;
            switch (curRank)
            {
                case 1:
                    foreGround = "#FFD014";
                    break;
                case 2:
                    foreGround = "#BDC5D3";
                    break;
                case 3:
                    foreGround = "#C49E70";
                    break;
                default:
                    foreGround = string.Empty;
                    break;
            }

            return foreGround;
        }

        /// <summary>
        /// TwoWay的转换器
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>TwoWay的转换后的对象</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写ProvideValue方法
        /// </summary>
        /// <param name="serviceProvider">服务提供对象</param>
        /// <returns>当前对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// 作业评分的前5名的排名是否显示
    /// </summary>
    public class Top5RankVisibleConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// OneWay的转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>转换后的对象</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var curRank = int.Parse(value.ToString());
            if (curRank <= 3)
                return Visibility.Hidden;
            else
                return Visibility.Visible;
        }

        /// <summary>
        /// TwoWay的转换器
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>TwoWay的转换后的对象</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写ProvideValue方法
        /// </summary>
        /// <param name="serviceProvider">服务提供对象</param>
        /// <returns>当前对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// 我的排名是否显示
    /// </summary>
    public class MeSelfRankDataVisibleConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// OneWay的转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>转换后的对象</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var curRank = (string)value;
            if (string.IsNullOrEmpty(curRank))
                return Visibility.Hidden;
            else
                return Visibility.Visible;
        }

        /// <summary>
        /// TwoWay的转换器
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标值</param>
        /// <param name="parameter">转换参数</param>
        /// <param name="culture">当前语言环境</param>
        /// <returns>TwoWay的转换后的对象</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写ProvideValue方法
        /// </summary>
        /// <param name="serviceProvider">服务提供对象</param>
        /// <returns>当前对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
