using SIE.MES.QTimes;
using SIE.MES.WorkOrders;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.QTimes
{
    /// <summary>
    /// QT超时报表查询实体视图配置
    /// </summary>
    public class QTimeReportViewModelCriteriaViewConfig : WebViewConfig<QTimeReportViewModelCriteria>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(p, k);
                }).HasLabel("产线".L10N()+"*").Show();
                View.Property(p => p.WorkOrder).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, k);
                }).Show();
                View.Property(p => p.Sn).Show();
                View.Property(p => p.ProCode).Show();
                View.Property(p => p.ProName).Show();
                View.Property(p => p.StartProcess).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).Show();
                View.Property(p => p.EndProcess).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).Show();
                View.Property(p => p.CollectTime).HasLabel("采集时间".L10N() + "*").UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
                View.Property(p => p.IsOverTime).Show();
            }
        }
    }
}
