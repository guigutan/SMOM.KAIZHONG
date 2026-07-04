using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using SIE.Web.Items.ViewModels;
using SIE.Web.MES.WorkOrders.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单BOM视图配置
    /// </summary>
    internal class WorkOrderBomViewConfig : WebViewConfig<WorkOrderBom>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.DeclareExtendViewGroup(WorkOrderViewConfig.ReadonlyView);
            if (ViewGroup == WorkOrderViewConfig.ReadonlyView)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal();
            View.UseCommands("SIE.Web.MES.WorkOrders.Commands.WorkBomAddCommand", "SIE.Web.MES.WorkOrders.Commands.WorkBomEditCommand", WebCommandNames.Delete);
            View.Property(p => p.LineNo).Readonly().Show(ShowInWhere.All);
            View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                dic.Add(nameof(e.IsAllowEdit), nameof(e.Item.EnableExtendProperty));
                m.DicLinkField = dic;
            }).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ItemController>().GetItems(keyword, pagingInfo);
            }).HasLabel("物料编码").Show(ShowInWhere.All).HasOrderNo(10);
            View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All).Readonly().HasOrderNo(20);

            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
            {
                p.IsAllRequired = true;
                p.SourceEntityType = "WorkOrderBom";
                p.ItemIdField = "ItemId";
                p.DbField = "ItemExtProp";
            }).HasOrderNo(35).Show(ShowInWhere.All).Readonly(p => !p.IsAllowEdit);
            View.Property(p => p.Rsnum);
            View.Property(p => p.Rspos);
            View.Property(p => p.Posnr);
            View.Property(p => p.Bwart);
            View.Property(p => p.Enmng);
            View.Property(p => p.Lgort);
            View.Property(p => p.Werks);
            View.Property(p => p.Meins);
            View.Property(p => p.IsAlternative).Show(ShowInWhere.All).HasOrderNo(26);

            View.Property(p => p.RequireQty).UseItemUnitEditor(p => {
                p.MinValue = 0;
            }).Show(ShowInWhere.All).HasOrderNo(30);
            //.UseSpinEditor(e =>
            // {
            //     e.MinValue = 0;
            //     e.AllowDecimals = true;
            // }

            View.Property(p => p.SingleQty).HasLabel("单位耗用量").UseItemUnitEditor(e =>
            {
                e.MinValue = 0;
                //e.AllowDecimals = true;
            }).Show(ShowInWhere.All).HasOrderNo(40);
            View.Property(p => p.AlterGroup).Show(ShowInWhere.All).HasOrderNo(41);
            View.Property(p => p.Alter).Show(ShowInWhere.All).HasOrderNo(42);
            View.Property(p => p.Priority).UseSpinEditor(p => {
                p.AllowBlank = true;
                p.MinValue = 0; 
                p.Step = 1; 
            }).Show(ShowInWhere.All).HasOrderNo(43);
            View.Property(p => p.IsRecoilItem).Show(ShowInWhere.All).HasOrderNo(50);
            View.Property(p => p.IsVritualItem).Show(ShowInWhere.All).HasOrderNo(60);
            View.Property(p => p.IsByBill).Show(ShowInWhere.All).HasOrderNo(70);
            View.Property(p => p.Remark).Show(ShowInWhere.All).HasOrderNo(80);

        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        void ReadOnlyView()
        {
            View.DisableEditing();
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Show().Readonly();
                View.Property(p => p.Item).HasLabel("物料编码").Show(ShowInWhere.All).Readonly().HasOrderNo(10);
                View.Property(p => p.ItemName).Show(ShowInWhere.All).Readonly().HasOrderNo(20);
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.SourceEntityType = "WorkOrderBom";
                    p.ItemIdField = "ItemId";
                    p.DbField = "ItemExtProp";
                }).Readonly().HasOrderNo(25).Show(ShowInWhere.All);
                View.Property(p => p.IsAlternative).Show(ShowInWhere.All).Readonly().HasOrderNo(26);
                View.Property(p => p.RequireQty).Show(ShowInWhere.All).Readonly().HasOrderNo(30);

                View.Property(p => p.SingleQty).Show(ShowInWhere.All).Readonly().HasOrderNo(40);
                View.Property(p => p.AlterGroup).Show(ShowInWhere.All).Readonly().HasOrderNo(41);
                View.Property(p => p.Alter).Show(ShowInWhere.All).Readonly().HasOrderNo(42);
                View.Property(p => p.Priority).Show(ShowInWhere.All).Readonly().HasOrderNo(43);
                View.Property(p => p.IsRecoilItem).Show(ShowInWhere.All).Readonly().HasOrderNo(50);
                View.Property(p => p.IsVritualItem).Show(ShowInWhere.All).Readonly().HasOrderNo(60);
                View.Property(p => p.IsByBill).Show(ShowInWhere.All).Readonly().HasOrderNo(70);
                View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly().HasOrderNo(80);

                View.Property(p => p.Rsnum).Show();
                View.Property(p => p.Rspos).Show();
                View.Property(p => p.Posnr).Show();
                View.Property(p => p.Bwart).Show();
                View.Property(p => p.Enmng).Show();
                View.Property(p => p.Lgort).Show();
                View.Property(p => p.Werks);
                View.Property(p => p.Meins);

            }
        }
    }
}
