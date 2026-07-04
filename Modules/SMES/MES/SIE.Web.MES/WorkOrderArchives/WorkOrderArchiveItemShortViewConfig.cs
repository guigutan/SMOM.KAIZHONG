using SIE.MES.WorkOrderArchives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案缺料情况视图配置
    /// </summary>
    public class WorkOrderArchiveItemShortViewConfig : WebViewConfig<WorkOrderArchiveItemShortViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.MES.WorkOrderArchives.Behaviors.ItemShortBehavior");
            using (View.OrderProperties())
            {
                View.DisableEditing();
                View.Property(p => p.ShortQty).FixColumn().ShowInList(width: 150);
                View.Property(p => p.ItemCode).FixColumn().ShowInList(width: 150);
                View.Property(p => p.ItemName).FixColumn().ShowInList(width: 150);
                View.Property(p => p.ItemExtPropName).ShowInList(width: 150);
                View.Property(p => p.ConsumeModel).ShowInList(width: 150);
                View.Property(p => p.SingleQty).ShowInList(width: 150);
                View.Property(p => p.UnitName).ShowInList(width: 150);
                View.Property(p => p.AvailableQty).UseListSetting(p => p.HelpInfo = "线边仓已接收未上料的标签剩余数量").ShowInList(width: 150);
                View.Property(p => p.FeedQty).UseListSetting(p => p.HelpInfo = "已上料到当前工单或工单产线的标签剩余数量").ShowInList(width: 150);
                View.Property(p => p.StockOrderQty).UseListSetting(p => p.HelpInfo = "已提交待执行的备料量").ShowInList(width: 150);
                View.Property(p => p.ToTakeQty).UseListSetting(p => p.HelpInfo = "已建单待执行的备料需求数量").ShowInList(width: 150);
                View.Property(p => p.SameNeedQty).ShowInList(width: 150);
                View.Property(p => p.TotalNeedQty).ShowInList(width: 150);
                View.Property(p => p.HasCostQty).ShowInList(width: 150);
                View.Property(p => p.ResidueNeedQty).ShowInList(width: 150);
                View.Property(p => p.SetQty).ShowInList(width: 150);
            }
        }
    }
}
