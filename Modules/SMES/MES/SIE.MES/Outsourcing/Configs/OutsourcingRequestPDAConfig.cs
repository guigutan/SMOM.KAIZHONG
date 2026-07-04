using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Configs
{
    /// <summary>
    /// 委外PDA配置项
    /// </summary>
    [System.ComponentModel.DisplayName("委外PDA配置项")]
    [System.ComponentModel.Description("委外PDA配置项")]
    public class OutsourcingRequestPDAConfig: ModuleConfig<OutsourcingRequestPDAConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly OutsourcingRequestPDAConfigValue defaultValue = new OutsourcingRequestPDAConfigValue { ConfirmDay = null };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override OutsourcingRequestPDAConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 委外PDA配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("委外PDA配置值")]
    public class OutsourcingRequestPDAConfigValue : ConfigValue
    {
        #region 发货确认筛选时间 ConfirmDay
        /// <summary>
        /// 发货确认筛选时间
        /// </summary>
        [Label("发货确认筛选时间")]
        public static readonly Property<int?> ConfirmDayProperty = P<OutsourcingRequestPDAConfigValue>.Register(e => e.ConfirmDay);

        /// <summary>
        /// 发货确认筛选时间
        /// </summary>
        public int? ConfirmDay
        {
            get { return this.GetProperty(ConfirmDayProperty); }
            set { this.SetProperty(ConfirmDayProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示 
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return "发货确认筛选时间:{0}天".L10nFormat(ConfirmDay == null ? "NULL" : ConfirmDay.ToString());
        }
    }
}
