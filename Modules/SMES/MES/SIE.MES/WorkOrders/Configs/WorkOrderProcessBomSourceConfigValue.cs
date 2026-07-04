using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单工序BOM配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单工序BOM设置")]
    public class WorkOrderProcessBomSourceConfigValue : ConfigValue
    {
        #region 工序bom来源 ProcessBomType
        /// <summary>
        /// 工序bom来源
        /// </summary>
        [Label("工序bom来源")]
        public static readonly Property<WorkOrderBomSourceType> ProcessBomTypeProperty = P<WorkOrderProcessBomSourceConfigValue>.Register(e => e.ProcessBomType);

        /// <summary>
        /// 工序bom类型
        /// </summary>
        public WorkOrderBomSourceType ProcessBomType
        {
            get { return this.GetProperty(ProcessBomTypeProperty); }
            set { this.SetProperty(ProcessBomTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>显示</returns>
        public override string Display()
        {
            return string.Concat("工单工序BOM来源：".L10N(), ProcessBomType == WorkOrderBomSourceType.ProductRoutingVersionBom ? "产品工序BOM".L10N() : "工艺路线工序BOM".L10N());
        }
    }
}
