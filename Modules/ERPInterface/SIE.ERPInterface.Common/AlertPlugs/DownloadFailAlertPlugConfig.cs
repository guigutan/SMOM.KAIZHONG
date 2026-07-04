using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.ERPInterface.Common.AlertPlugs
{
    /// <summary>
    /// ERP接口调度下载失败预警配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("ERP接口调度下载失败预警配置")]
    public class DownloadFailAlertPlugConfig : AlertConfig
    {
        #region 预警天数范围 Period
        /// <summary>
        /// 预警天数范围
        /// </summary>
        [Label("预警天数范围")]
        public static readonly Property<int> PeriodProperty = P<DownloadFailAlertPlugConfig>.Register(e => e.Period);

        /// <summary>
        /// 预警天数范围
        /// </summary>
        public int Period
        {
            get { return this.GetProperty(PeriodProperty); }
            set { this.SetProperty(PeriodProperty, value); }
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">值</param>
        public override void Initialize(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return;

            var config = JsonConvert.DeserializeObject<DownloadFailAlertPlugConfig>(value);
            this.Period = config.Period;
        }

        /// <summary>
        /// 转换字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            var o = new
            {
                Period = this.Period,
            };
            string ret = JsonConvert.SerializeObject(o);
            return ret;
        }
    }
}
