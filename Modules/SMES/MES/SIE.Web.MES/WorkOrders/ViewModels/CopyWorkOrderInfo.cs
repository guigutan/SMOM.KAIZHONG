using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Web.Items.ViewModels;
using System;

namespace SIE.Web.MES.WorkOrders.ViewModels
{
    /// <summary>
    /// 复制工单信息类
    /// </summary>
    [Serializable]
    public class CopyWorkOrderInfo
    {
        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder CopyWorkOrder { get; set; }

        /// <summary>
        /// 打印模板
        /// </summary>
        public LabelPrintTemplate PrintTemplate { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal BatchQty { get; set; }

        /// <summary>
        /// 工单产品扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        ///工单产品扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 工单属性值列表
        /// </summary>
        public EntityList<PropertyValueViewModel> PropertyValues { get; } = new EntityList<PropertyValueViewModel>();
    }
}
