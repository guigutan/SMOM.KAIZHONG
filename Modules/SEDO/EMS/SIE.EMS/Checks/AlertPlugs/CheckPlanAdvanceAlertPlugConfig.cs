using Newtonsoft.Json;
using SIE.Common.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.AlertPlugs
{
    /// <summary>
    /// 点检计划提前预警
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检计划提前预警")]
    public class CheckPlanAdvanceAlertPlugConfig : AlertConfig
    {
        #region 预警触发值 AlertValue
        /// <summary>
        /// 预警触发值
        /// </summary>
        [Label("预警触发值")]
        public static readonly Property<int> AlertValueProperty = P<CheckPlanAdvanceAlertPlugConfig>.Register(e => e.AlertValue);

        /// <summary>
        /// 预警触发值
        /// </summary>
        public int AlertValue
        {
            get { return this.GetProperty(AlertValueProperty); }
            set { this.SetProperty(AlertValueProperty, value); }
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

            var config = JsonConvert.DeserializeObject<CheckPlanAdvanceAlertPlugConfig>(value);
            this.AlertValue = config.AlertValue;
        }

        /// <summary>
        /// 转换字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            var o = new
            {
                AlertValue = this.AlertValue,
            };
            string ret = JsonConvert.SerializeObject(o);
            return ret;
        }
    }
}
