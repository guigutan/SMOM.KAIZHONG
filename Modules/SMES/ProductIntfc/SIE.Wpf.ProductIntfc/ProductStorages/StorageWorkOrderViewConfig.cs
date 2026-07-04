using SIE.Domain;
using SIE.ProductIntfc.ProductStorages;
using System.Collections.Generic;

namespace SIE.Wpf.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库视图配置
    /// </summary>
    public class StorageWorkOrderViewConfig : WPFViewConfig<StorageWorkOrder>
    {
        /// <summary>
        /// 成品入库视图组
        /// </summary>
        public static string StorageWorkOrderView { get; } = "StorageWorkOrderViewConfig";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(StorageWorkOrderView);
            if (ViewGroup == StorageWorkOrderView)
                ConfigStorageWorkOrderView();
        }

        /// <summary>
        /// 配置成品入库视图
        /// </summary>
        private void ConfigStorageWorkOrderView()
        {
            View.ClearCommands();
            View.UseCommands(WPFCommandNames.Export);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).HasLabel("工单号").ShowInList();
                View.Property(p => p.Product).HasLabel("产品编码").ShowInList();
                View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").ShowInList();
                View.Property(p => p.ProductType).UseEnumEditor().HasLabel("基本分类").ShowInList();
                View.Property(p => p.Type).UseEnumEditor().HasLabel("工单类型").ShowInList();
                View.Property(p => p.PlanQty).HasLabel("计划数量").ShowInList();
                View.Property(p => p.WorkShop).HasLabel("车间").ShowInList();
                View.Property(p => p.Resource).HasLabel("资源").ShowInList();
                View.Property(p => p.State).ShowInList().HasOrderNo(9);
            }
            View.AttachChildrenProperty(typeof(ToStorageBarcode), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var storework = args.Parent as StorageWorkOrder;
                return RT.Service.Resolve<ProductStorageController>().GetToStoreBarcode(storework.Id, false, args.PagingInfo, (List<OrderInfo>)args.SortInfo);
            }).HasLabel("待入库").OrderNo = 10;
            View.AttachChildrenProperty(typeof(InStorageBill), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var storework = args.Parent as StorageWorkOrder;
                return RT.Service.Resolve<ProductStorageController>().GetInStoreBarcode(storework.Id, args.PagingInfo, (List<OrderInfo>)args.SortInfo);
            }).HasLabel("已入库").OrderNo = 20;
            View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Hide);            
            View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide);
        }
    }
}