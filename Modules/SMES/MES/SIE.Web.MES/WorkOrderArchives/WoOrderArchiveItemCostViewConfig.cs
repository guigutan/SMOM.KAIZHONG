using SIE.MES.WorkOrderArchives;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案物料耗用视图配置
    /// </summary>
    public class WoOrderArchiveItemCostViewConfig : WebViewConfig<WoOrderArchiveItemCostViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessName).ShowInList(width: 150);
                View.Property(p => p.WorkStep).ShowInList(width: 150);
                View.Property(p => p.ItemCode).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.ItemExPro).ShowInList(width: 150);
                View.Property(p => p.SingleQty).ShowInList(width: 150);
                View.Property(p => p.RequireQty).ShowInList(width: 150);
                View.Property(p => p.TotalQty).ShowInList(width: 150);
            }
        }
    }
}
