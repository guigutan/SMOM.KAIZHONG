using SIE.MES.WorkOrderArchives;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案待用标签视图配置
    /// </summary>
    public class WorkOrderArchiveItemlLabelViewConfig : WebViewConfig<WorkOrderArchiveItemlLabelViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Label).ShowInList(width: 150);
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.ItemCode).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.ItemExPro).ShowInList(width: 150);
                View.Property(p => p.Warehouse).ShowInList(width: 150);
                View.Property(p => p.Storage).ShowInList(width: 150);
                View.Property(p => p.Qty).ShowInList(width: 150);
                View.Property(p => p.IsSerialNumber).Readonly().ShowInList(width: 150);
            }
        }
    }
}
