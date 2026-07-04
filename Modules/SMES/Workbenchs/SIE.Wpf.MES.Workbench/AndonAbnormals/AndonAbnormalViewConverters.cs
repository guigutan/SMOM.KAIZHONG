using SIE.MES.Workbench.AlertLights;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.MES.Workbench.AndonAbnormals
{
    /// <summary>
    /// 安灯异常GridControl的Visible的转换器
    /// </summary>
    public class AndonAbnormalViewVisibleConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 转换器的OneWay方法
        /// </summary>
        /// <param name="value">输入Value</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言对象</param>
        /// <returns>转换后的Value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var viewValue = (string)value;
            if (viewValue.IsNullOrEmpty())
            {
                return System.Windows.Visibility.Hidden;
            }
            else
            {
                return System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// 转换器的TwoWay方法
        /// </summary>
        /// <param name="value">输入Value</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言对象</param>
        /// <returns>转换后的Value</returns>
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

    /// <summary>
    /// 安灯异常GridControl的Foreground转换器
    /// </summary>
    public class AndonAbnormalViewForegroundConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// GridControl的Foreground转换器
        /// </summary>
        /// <param name="value">输入Value</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言对象</param>
        /// <returns>转换后的Value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "#128BEF";
            var processStatus = (string)value;
            var curUIName = (string)parameter;
            if (processStatus == ProcessStatusType.Closed.ToLabel())
            {
                result = "#999999"; //灰色
            }
            else if (processStatus == ProcessStatusType.Processing.ToLabel())
            {
                if (curUIName == "CallTime")
                {
                    result = "#999999"; //灰色
                }
                else
                {
                    result = "#128BEF"; //蓝色
                }
            }
            else if (processStatus == ProcessStatusType.Waitting.ToLabel())
            {
                if (curUIName == "ProcessStatus")
                {
                    result = "#FFB400"; //黄色
                }
                else
                {
                    result = "#128BEF"; //蓝色
                }
            }

            return result;
        }

        /// <summary>
        /// 转换器的TwoWay方法
        /// </summary>
        /// <param name="value">输入Value</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言对象</param>
        /// <returns>转换后的Value</returns>
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

    /// <summary>
    /// 安灯异常的PackIcon的转换器类
    /// </summary>
    public class AndonAbnormalViewPackIconConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// PackIcon的OneWay的转换器
        /// </summary>
        /// <param name="value">输入Value</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">语言对象</param>
        /// <returns>转换后的Value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            var processStatus = (string)value;
            if (processStatus == ProcessStatusType.Closed.ToLabel())
            {
                result = "SettingFinish";
            }
            else if (processStatus == ProcessStatusType.Processing.ToLabel())
            {
                result = "CommentProcessing";
            }
            else if (processStatus == ProcessStatusType.Waitting.ToLabel())
            {
                result = "Waiting";
            }

            return result;
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
