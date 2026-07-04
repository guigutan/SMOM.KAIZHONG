using SIE.MES.BatchWIP.Products.ViewModels.BatchWipProductReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 工位库龄查询实体
    /// </summary>
    internal class BatchWipProductReportCriteriaViewConfig:WebViewConfig<BatchWipProductReportCriteria>
    {
        protected override void ConfigQueryView()
        {
            //base.ConfigQueryView();
            View.Property(p => p.Station).ShowInDetail();
            View.Property(p => p.BatchNo).ShowInDetail();
            View.Property(p => p.WorkOrder).ShowInDetail();
            View.Property(p => p.Item).ShowInDetail();
        }
    }
}
