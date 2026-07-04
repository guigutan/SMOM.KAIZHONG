using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings;
using SIE.Web.Items.ViewModels;
using System;

namespace SIE.Web.MES.WorkOrders.ViewModels
{
    /// <summary>
    /// 产品属性变更信息类
    /// </summary>
    [Serializable]
    public class ProductChangedInfo
    {
        /// <summary>
        /// 工单bom信息
        /// </summary>
        public EntityList<WorkOrderBom> BomInfos { get; } = new EntityList<WorkOrderBom>();

        /// <summary>
        /// 工单BOM属性值列表
        /// </summary>
        public EntityList<PropertyValueViewModel> WorkOrderBomPropertys { get; } = new EntityList<PropertyValueViewModel>();

        /// <summary>
        /// 工单包装规则信息
        /// </summary>
        public EntityList<WorkOrderPackageRuleDetail> PackageRuleInfos { get; } = new EntityList<WorkOrderPackageRuleDetail>();

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public RoutingVersion RoutingVersion { get; set; }

        /// <summary>
        /// 打印模板设置
        /// </summary>
        public LabelPrintTemplate Template { get; set; }

        /// <summary>
        /// 产品是否单体追溯
        /// </summary>
        public bool IsSingle { get; set; }

        /// <summary>
        /// 产品拼板数
        /// </summary>
        public int PanelQty { get; set; }

        /// <summary>
        /// 是否组合板工单
        /// </summary>
        public bool IsPanelWorkOrder { get; set; }
    }
}
