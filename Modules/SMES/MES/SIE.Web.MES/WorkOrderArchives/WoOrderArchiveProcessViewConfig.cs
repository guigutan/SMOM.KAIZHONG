using SIE.MES.WorkOrderArchives;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案生产采集视图配置
    /// </summary>
    public class WoOrderArchiveProcessViewConfig : WebViewConfig<WoOrderArchiveProcessViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Index).ShowInList(width: 150);
                View.Property(p => p.ProcessName).ShowInList(width: 150);
                View.Property(p => p.QtyMove).ShowInList(width: 150);
                View.Property(p => p.QtyPass).ShowInList(width: 150);
                View.Property(p => p.QtyFailed).ShowInList(width: 150);
                View.Property(p => p.QtyStacked).ShowInList(width: 150);
            }
        }
    }
}
