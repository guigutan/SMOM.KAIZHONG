using SIE.MES.WorkOrders;
using SIE.MetaModel;
using System;


namespace SIE.MES.PackingPrints
{
    /// <summary>
    /// 包装条码打印实体
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PackingWorkOrderCriteria))]
    public class PackingWorkOrder : WorkOrder
    {

    }
}
