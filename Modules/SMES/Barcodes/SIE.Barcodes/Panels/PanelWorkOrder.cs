using SIE.Core.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码打印工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PanelWorkOrderCriteria))]
    [Label("拼板码打印工单")]
    public class PanelWorkOrder : WorkOrder
    {
    }
}