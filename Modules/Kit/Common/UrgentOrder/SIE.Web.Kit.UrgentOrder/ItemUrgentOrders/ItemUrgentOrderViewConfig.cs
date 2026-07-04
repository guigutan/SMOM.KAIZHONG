using SIE.Domain;
using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.MetaModel.View;
using SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Commands;
using System.Collections.Generic;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders
{
    /// <summary>
    /// 物料加急单维护
    /// </summary>
    internal class ItemUrgentOrderViewConfig : WebViewConfig<ItemUrgentOrder>
    {
        /// <summary>
        /// 看板推送信息
        /// </summary>
        public const string BoardPushDataViewGroup = "BoardPushDataViewGroup";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ItemUrgentOrder.NoProperty);
            View.InlineEdit().UseDefaultCommands();
            View.DeclareExtendViewGroup(new string[] { BoardPushDataViewGroup });

            if (ViewGroup == BoardPushDataViewGroup)
            {
                View.DomainName("看板推送信息");
                BoardPushDataView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(WebCommandNames.Add, typeof(ItemUrgentOrderAddCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Copy, typeof(ItemUrgentOrderCopyCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Save, typeof(ItemUrgentOrderSaveCommand).FullName);
            View.RemoveCommands(WebCommandNames.Edit, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly();
                View.Property(p => p.ItemId).HasLabel("物料编号").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = dic;
                }).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
                View.Property(p => p.ItemName).HasLabel("物料描述").Readonly();
                View.Property(p => p.OrderState).HasLabel("状态");
                View.Property(p => p.Qty).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; });
                View.Property(p => p.DemandTime).UseDateTimeEditor().Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
                View.AttachDetailChildrenProperty(typeof(ItemUrgentOrder), (c) =>
                {
                    var pagingDataArgs = c as ChildPagingDataArgs;
                    var item = c.Parent as ItemUrgentOrder;
                    item = RF.GetById<ItemUrgentOrder>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return item;
                }, BoardPushDataViewGroup).HasLabel("看板推送信息").OrderNo = 10;
            }
        }

        /// <summary>
        /// 配置看板推送信息视图
        /// </summary>
        protected void BoardPushDataView()
        {
            View.AddBehavior("SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Behaviors.ItemUrgentOrderBehavior");
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(2);
                View.Property(p => p.IsReceive).UseCheckEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsInspectIqc).UseCheckEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsInstorage).UseCheckEditor().Show(ShowInWhere.All);
                View.Property(p => p.IsStockUp).UseCheckEditor().Show(ShowInWhere.All);
                View.Property(p => p.ReceiveQty).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; }).Show(ShowInWhere.All).Readonly(p => p.IsReceive == false);
                View.Property(p => p.InspectIqcQty).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; }).Show(ShowInWhere.All).Readonly(p => p.IsInspectIqc == false);
                View.Property(p => p.ReceiveState).Show(ShowInWhere.All).Readonly(p => p.IsReceive == false);
                View.Property(p => p.InspectIqcState).Show(ShowInWhere.All).Readonly(p => p.IsInspectIqc == false);
                View.Property(p => p.InstorageQty).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; }).Show(ShowInWhere.All).Readonly(p => p.IsInstorage == false);
                View.Property(p => p.StockUpQty).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; }).Show(ShowInWhere.All).Readonly(p => p.IsStockUp == false);
                View.Property(p => p.InstorageState).Show(ShowInWhere.All).Readonly(p => p.IsInstorage == false);
                View.Property(p => p.StockUpState).Show(ShowInWhere.All).Readonly(p => p.IsStockUp == false);
                View.Property(p => p.StockUpNo).Show(ShowInWhere.All).Readonly(p => p.IsStockUp == false);
            }
        }
    }
}
