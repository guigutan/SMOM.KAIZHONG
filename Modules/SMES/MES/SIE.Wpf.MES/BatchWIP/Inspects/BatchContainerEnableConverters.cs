using SIE.Core.Barcodes;
using SIE.Tech.Processs;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 载具条码编辑框、生成批次条码按钮是否可用转换器类
    /// </summary>
    public class BatchContainerEnableConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 值转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地语言</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            var barcodeType = (BarcodeType)value;
            var param = (string)parameter;
            if (barcodeType == BarcodeType.ContainerNo)
            {
                if (param == "ContainerCtl")
                {
                    result = true;
                }
                else if (param == "BatchCtl")
                {
                    result = false;
                }
                else
                {
                    //
                }
            }
            else if (barcodeType == BarcodeType.BatchBarocde)
            {
                if (param == "ContainerCtl")
                {
                    result = false;
                }
                else if (param == "BatchCtl")
                {
                    result = true;
                }
                else
                {
                    //
                }
            }
            else
            {
                //
            }

            return result;
        }

        /// <summary>
        /// 值转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地语言</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当前对象
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider接口对象</param>
        /// <returns>返回当前对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// 载具条码编辑框、生成批次条码按钮背景色转换器类
    /// </summary>
    public class BatchContainerBackgroundConverters : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 值转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地语言</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            var barcodeType = (BarcodeType)value;
            var param = (string)parameter;
            if (barcodeType == BarcodeType.ContainerNo)
            {
                if (param == "ContainerCtl")
                {
                    result = "#008000"; //绿色
                }
                else if (param == "BatchCtl")
                {
                    result = "#999999"; //灰色
                }
                else
                {
                    //
                }
            }
            else if (barcodeType == BarcodeType.BatchBarocde)
            {
                if (param == "ContainerCtl")
                {
                    result = "#999999";
                }
                else if (param == "BatchCtl")
                {
                    result = "#008000";
                }
                else
                {
                    //
                }
            }
            else
            {
                //
            }

            return result;
        }

        /// <summary>
        /// 值转换方法
        /// </summary>
        /// <param name="value">原值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">本地语言</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当前对象
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider接口对象</param>
        /// <returns>返回当前对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
