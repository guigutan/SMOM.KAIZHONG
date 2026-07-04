using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AlertPlugs
{
    /// <summary>
    /// 设备校验任务超期提醒
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备校验任务超期提醒")]
    public class CalibrationTimeOutAlertPlugConfig : AlertConfig
    {
        #region 超时时间(天) TimeOut
        /// <summary>
        /// 超时时间(天)
        /// </summary>
        [Label("超时时间(天)")]
        public static readonly Property<int> TimeOutProperty = P<CalibrationTimeOutAlertPlugConfig>.Register(e => e.TimeOut);

        /// <summary>
        /// 超时时间(天)
        /// </summary>
        public int TimeOut
        {
            get { return GetProperty(TimeOutProperty); }
            set { SetProperty(TimeOutProperty, value); }
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

            var config = JsonConvert.DeserializeObject<CalibrationTimeOutAlertPlugConfig>(value);
            this.TimeOut = config.TimeOut;
        }

        /// <summary>
        /// 转换字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            var o = new
            {
                TimeOut = this.TimeOut,
            };
            string ret = JsonConvert.SerializeObject(o);
            return ret;
        }
    }
}
