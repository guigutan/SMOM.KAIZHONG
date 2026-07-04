using SIE.Core.WorkOrders;
using SIE.DIST;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.Items.ViewModels;
using System.Collections.Generic;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 配送管理视图配置
    /// </summary>
    public class GoodsIssueViewConfig : WebViewConfig<GoodsIssue>
    {
        /// <summary>
        /// 查看工单视图ViewGroup
        /// </summary>
        public static readonly string EditView = "EditView";


        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);

            if (ViewGroup == EditView) 
            {
                EditConfigView();
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands("SIE.Web.DIST.GoodsIssueAddCommand");
            View.UseCommands("SIE.Web.DIST.GoodsIssueEditCommand");
            View.UseCommands("SIE.Web.DIST.GoodsIssueDeleteCommand");
            View.UseCommands("SIE.Web.DIST.GoodsIssue.Commands.DistributionCommand");
            View.Property(p => p.ItemCode).HasLabel("物料编码").ShowInList(150).FixColumn(true);
            View.Property(p => p.ItemName).HasLabel("物料名称").ShowInList(150).FixColumn(true);
            View.Property(p => p.WorkOrderNo).HasLabel("工单").ShowInList(150).FixColumn(true);
            View.Property(p => p.SendNo);
            View.Property(p => p.BatchNo);
            View.Property(p => p.UnitName);
            View.Property(p => p.Qty);
            View.Property(p => p.RemainderQty);
            View.Property(p => p.DistributionQty);
            View.Property(p => p.DefectQty);
            View.Property(p => p.NormalReturnQty);
            View.Property(p => p.DefectReturnQty);
            View.ChildrenProperty(p => p.PropertyValueList).Visible(false); //用来保存的物料属性的
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseDefaultCommands().RemoveCommands(WebCommandNames.Delete);
            View.UseDetail(columnCount: 2);
            View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>
                {
                    { nameof(e.ItemName), nameof(e.Item.Name) }
                };
                m.DicLinkField = dic;
            });
            View.Property(p => p.ItemName).HasLabel("物料名称").Readonly(true);
            View.Property(p => p.WorkOrder).UseDataSource((e, p, s) =>
            {
                return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, s);
            }).UsePagingLookUpEditor(p => p.DisplayField = WorkOrder.NoProperty.Name);
            View.Property(p => p.SendNo).Readonly(p => p.DistributionQty > 0);
            View.Property(p => p.BatchNo).Readonly(p => p.DistributionQty > 0);
            View.Property(p => p.Unit).Readonly(false).Readonly(p => p.DistributionQty > 0);
            View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 1);
        }

        /// <summary>
        /// 配送管理自定义表单视图
        /// </summary>
        void EditConfigView()
        {
            View.AddBehavior("SIE.Web.DIST.GoodsIssueBahavor");
            View.ClearCommands();
            View.UseCommands("SIE.Web.DIST.GoodsIssueSaveCommand");
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("基本信息", 2, false))
                {
                    View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>{
                        { nameof(e.ItemName), nameof(e.Item.Name) }
                        };
                        m.DicLinkField = dic;
                    }).Show(ShowInWhere.Detail);
                    View.Property(p => p.ItemName).HasLabel("物料名称").Readonly(true).Show(ShowInWhere.All);
                    View.Property(p => p.WorkOrder).UseDataSource((e, p, s) =>
                    {
                        return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, s);
                    }).UsePagingLookUpEditor(p => p.DisplayField = WorkOrder.NoProperty.Name).Show(ShowInWhere.All);
                    View.Property(p => p.SendNo).Readonly(p => p.DistributionQty > 0).Show(ShowInWhere.All);
                    View.Property(p => p.BatchNo).Readonly(p => p.DistributionQty > 0).Show(ShowInWhere.All);
                    View.Property(p => p.Unit).Readonly(false).Readonly(p => p.DistributionQty > 0).Show(ShowInWhere.All)
                         .UseListSetting(e => { e.HelpInfo = "配送数量大于0不可编辑"; });
                    View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 1).Show(ShowInWhere.All);
                }
                View.ChildrenProperty(p => p.PropertyValueList).Visible(false);
                View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
                {
                    var list = new EntityList<PropertyValueViewModel>();
                    return list;
                }, GoodsIssuePropertyValueViewConfig.GoodsIssuePropertyValueView).Show(ChildShowInWhere.All).HasLabel("属性值");
            }
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrderNo).HasLabel("工单号").Readonly(false);
            View.Property(p => p.ItemId);
        }
    }
}
