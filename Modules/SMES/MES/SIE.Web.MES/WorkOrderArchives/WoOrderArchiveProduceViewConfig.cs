using SIE.MES.WorkOrderArchives;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案工单产出子表视图配置
    /// </summary>
    public class WoOrderArchiveProduceViewConfig : WebViewConfig<WoOrderArchiveProduceViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProCode).ShowInList(width: 150);
                View.Property(p => p.ProName).ShowInList(width: 150);
                View.Property(p => p.BarCode).ShowInList(width: 150);
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.Qty).ShowInList(width: 150);
                View.Property(p => p.DrawQty).ShowInList(width: 150);
                View.Property(p => p.IsOver).Readonly().ShowInList(width: 150);
            }
            
        }
    }
}
