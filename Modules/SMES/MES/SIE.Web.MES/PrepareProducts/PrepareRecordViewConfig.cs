using SIE.MES.PrepareProducts;
using SIE.Web.MES.PrepareProducts.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备记录视图配置
    /// </summary>
    public class PrepareRecordViewConfig : WebViewConfig<PrepareRecord>
    {
        /// <summary>
        /// 自定义视图
        /// </summary>
        public const string PrepareRecordViewStr = "PrepareRecordViewStr";

        public const string ExecuteViewStr = "ExecuteViewStr";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(PrepareRecordViewStr, ExecuteViewStr);
            if (ViewGroup == PrepareRecordViewStr)
            {
                PrepareRecordListView();
            }
            if (ViewGroup == ExecuteViewStr)
            {
                ExecuteView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected void PrepareRecordListView()
        {
            View.DisableEditing();
            View.UseCommands("SIE.Web.MES.PrepareProducts.Commands.PrepareRecordExecuteCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.PrepareState).ShowInList(width: 150);
                View.Property(p => p.ProductCode).ShowInList(width: 150);
                View.Property(p => p.ProductName).ShowInList(width: 150);
                View.Property(p => p.ItemExtPropName).ShowInList(width: 150);
                View.Property(p => p.ProductType).HasLabel("基本分类").ShowInList(width: 150);
                View.Property(p => p.State).ShowInList(width: 150);
                View.Property(p => p.Type).ShowInList(width: 150);
                View.Property(p => p.PlanQty).ShowInList(width: 150);
                View.Property(p => p.Factory).ShowInList(width: 150);
                View.Property(p => p.WorkShop).ShowInList(width: 150);
                View.Property(p => p.Resource).ShowInList(width: 150);
                View.Property(p => p.PlanBeginDate).ShowInList(width: 150);
                View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.WorkOrderOutputProductList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.LayoutInfoList).Show(ChildShowInWhere.Hide);
            }
        }

        protected void ExecuteView()
        {
            View.DisableEditing();
            View.UseCommand(typeof(ComfrimCommand).FullName);
            View.AddBehavior("SIE.Web.MES.PrepareProducts.Behaviors.PrepareRecordBehavior");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.PrepareState).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ItemExtPropName).Show();
                View.Property(p => p.ProductType).HasLabel("基本分类").Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.PlanQty).Show();
                View.Property(p => p.Factory).Show();
                View.Property(p => p.WorkShop).Show();
                View.Property(p => p.Resource).Show();
                View.Property(p => p.PlanBeginDate).Show();
                View.ChildrenProperty(p => p.PrepareRecordDetail).Show(ChildShowInWhere.All).LazyLoad(false).ViewGroup = PrepareRecordDetailViewConfig.ExecuteViewStr;
                View.ChildrenProperty(p => p.LayoutInfoList).Show(ChildShowInWhere.Hide);
            }
        }

    }
}
