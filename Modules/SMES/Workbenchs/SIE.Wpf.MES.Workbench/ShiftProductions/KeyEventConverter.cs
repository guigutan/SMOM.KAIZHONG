using SIE.Domain;
using SIE.MES.Workbench;
using SIE.MES.Workbench.AlertLights;
using SIE.MES.Workbench.AndonAbnormals;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SIE.Wpf.MES.Workbench.ShiftProductions
{
    /// <summary>
    /// 安灯异常转换器
    /// </summary>
    public class KeyEventConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 安灯异常常量字符串
        /// </summary>
        private const string AndonAbnormal = "AndonAbnormal";

        /// <summary>
        /// 车间Id
        /// </summary>
        private double? _workShopId = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public KeyEventConverter()
        {
            KeyEventConverter owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, KeyEventConverterSubscribeHandle);
        }



        /// <summary>
        /// 车间变更事件处理函数
        /// </summary>
        /// <param name="obj">车间变更事件</param>
        private void KeyEventConverterSubscribeHandle(WorkShopChangedEvent obj)
        {
            _workShopId = obj.WorkShopId;
            var workShop = RF.GetById<Enterprise>(_workShopId);
            if (workShop == null)
                return;
            this.Convert(AndonAbnormal, null, null, null);
        }

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
            string result = string.Empty;
            var keyItem = value?.ToString();
            if (keyItem == AndonAbnormal)
            {
                var processStatusTypes = new List<ProcessStatusType>() { ProcessStatusType.Waitting, ProcessStatusType.Processing, ProcessStatusType.Closed };
                var date = RF.Find<AndonAbnormal>().GetDbTime();
                var count = RT.Service.Resolve<AlertLightsController>().GetAndonAbnormalCount(date.Date, processStatusTypes, _workShopId);
                result = "安灯异常(" + count.ToString() + ")";
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
}
