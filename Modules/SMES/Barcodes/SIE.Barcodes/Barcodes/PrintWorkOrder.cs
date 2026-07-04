using SIE.Core.WorkOrders;
using SIE.MetaModel;
using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码打印实体
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PrintWorkOrderCriteria))]
    public class PrintWorkOrder : WorkOrder
    {
    }

    /// <summary>
    /// 工单 实体配置
    /// </summary>
    internal class WorkOrderConfig : EntityConfig<WorkOrder>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}