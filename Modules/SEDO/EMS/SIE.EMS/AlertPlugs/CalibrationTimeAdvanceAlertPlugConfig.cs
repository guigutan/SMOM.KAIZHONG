using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AlertPlugs
{
    /// <summary>
    /// 设备校验任务提前生成
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备校验任务提前生成")]
    public class CalibrationTimeAdvanceAlertPlugConfig : AlertConfig
    {
        #region 提前时间 TimeAdvance
        /// <summary>
        /// 提前时间
        /// </summary>
        [Label("提前时间(天)")]
        public static readonly Property<int> TimeAdvanceProperty = P<CalibrationTimeAdvanceAlertPlugConfig>.Register(e => e.TimeAdvance);

        /// <summary>
        /// 提前时间
        /// </summary>
        public int TimeAdvance
        {
            get { return this.GetProperty(TimeAdvanceProperty); }
            set { this.SetProperty(TimeAdvanceProperty, value); }
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

            var config = JsonConvert.DeserializeObject<CalibrationTimeAdvanceAlertPlugConfig>(value);
            this.TimeAdvance = config.TimeAdvance;
        }

        /// <summary>
        /// 转换字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            var o = new
            {
                TimeAdvance = this.TimeAdvance,
            };
            string ret = JsonConvert.SerializeObject(o);
            return ret;
        }
    }
}
