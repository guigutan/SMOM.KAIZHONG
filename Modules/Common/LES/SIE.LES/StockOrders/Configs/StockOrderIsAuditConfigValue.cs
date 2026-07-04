using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 手工单据是否需审核配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("手工单据是否需审核配置")]
    public class StockOrderIsAuditConfigValue : ConfigValue
    {
        #region 手工单据是否需审核 IsAudit
        /// <summary>
        /// 手工单据是否需审核
        /// </summary>
        [Label("手工单据是否需审核")]
        public static readonly Property<bool> IsAuditProperty = P<StockOrderIsAuditConfigValue>.Register(e => e.IsAudit);

        /// <summary>
        /// 手工单据是否需审核
        /// </summary>
        public bool IsAudit
        {
            get { return this.GetProperty(IsAuditProperty); }
            set { this.SetProperty(IsAuditProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示接口配置
        /// </summary>
        /// <returns>返回接口配置</returns>
        public override string Display()
        {
            return IsAudit ? "是".L10N() : "否".L10N();
        }
    }
}