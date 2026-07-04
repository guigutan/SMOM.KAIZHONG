using SIE.MES.QTimes;
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
    /// QTime标准维护查询实体
    /// </summary>
    public class QTimeStandardCriteriaViewConfig : WebViewConfig<QTimeStandardCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(p, k);
                }).Show();
                View.Property(p => p.StartProcess).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).Show();
                View.Property(p => p.EndProcess).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).Show();
                View.Property(p => p.IsAlert).Show(ShowInWhere.Hide);
                View.Property(p => p.State).Show();
            }
        }
    }
}
