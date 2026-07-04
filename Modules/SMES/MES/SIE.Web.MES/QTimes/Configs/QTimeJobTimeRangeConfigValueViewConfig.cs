using SIE.MES.QTimes.Configs;
using SIE.MES.QTimes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.QTimes.Configs
{
    /// <summary>
    /// QT获取条码时间范围配置项视图配置
    /// </summary>
    public class QTimeJobTimeRangeConfigValueViewConfig : WebViewConfig<QTimeJobTimeRangeConfigValue>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.CollectTime).Show();
                View.Property(p => p.Day)
                    .DefaultValue(1)
                    .UseSpinEditor(p => { p.MinValue = 1; p.DecimalPrecision = 0; }).Show()
                    .Readonly(p => p.CollectTime != SIE.MES.QTimes.Enums.QTimeConfigTimeRange.Customize);
            }
        }
    }
}
