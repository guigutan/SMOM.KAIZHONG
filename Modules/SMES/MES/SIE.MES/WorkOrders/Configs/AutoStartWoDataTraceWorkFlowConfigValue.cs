using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单创建是否自动发起追溯流程 配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单创建是否自动发起追溯流程 配置值")]
    public class AutoStartWoDataTraceWorkFlowConfigValue : ConfigValue
    {
        #region 是否自动发起 IsAutoStart
        /// <summary>
        /// 是否自动发起
        /// </summary>
        [Label("是否自动发起")]
        public static readonly Property<bool> IsAutoStartProperty = P<AutoStartWoDataTraceWorkFlowConfigValue>.Register(e => e.IsAutoStart);

        /// <summary>
        /// 是否自动发起
        /// </summary>
        public bool IsAutoStart
        {
            get { return this.GetProperty(IsAutoStartProperty); }
            set { this.SetProperty(IsAutoStartProperty, value); }
        }
        #endregion

    }
}
