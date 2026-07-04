using SIE.DataTrace.TraceMainDatas;
using SIE.Domain;
using SIE.WorkFlow.Base.FlowInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯主数据查询实体视图
    /// </summary>
    public class TraceMainDataCriteriaViewConfig : WebViewConfig<TraceMainDataCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.WorkFlowType).Show(ShowInWhere.All);
                View.Property(p => p.FlowInstanceId).UseDataSource((e, p, keyword) =>
                {
                    return RT.Service.Resolve<TraceMainDataController>().GetDataTraceFlowInstaces(p, keyword);
                }).Show(ShowInWhere.All);
                View.Property(p => p.Context).Show(ShowInWhere.All);
            }
        }
    }
}
