using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工管理
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductionDispatchReportCriteria))]
    [Label("生产任务报表")]
    public class ProductionDispatchReport : DispatchTask
    {

    }
}
