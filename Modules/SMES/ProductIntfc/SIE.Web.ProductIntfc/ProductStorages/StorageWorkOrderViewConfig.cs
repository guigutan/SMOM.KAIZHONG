using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ProductIntfc.ProductStorages;
using SIE.Web.Common.Configs.Commands;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库视图配置
    /// </summary>
    public class StorageWorkOrderViewConfig : WebViewConfig<StorageWorkOrder>
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
            View.UseDefaultCommands();
            View.ClearCommands(false).UseCommands(WebCommandNames.ExportXls);
            View.UseCommands(ConfigCommands.ModuleConfigCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().HasLabel("工单号").ShowInList(150);
                View.Property(p => p.Product).Readonly().HasLabel("产品编码").ShowInList(150);
                View.Property(p => p.WorkOrderProductName).Readonly().HasLabel("产品名称").ShowInList(150);
                View.Property(p => p.ProjectMaintainCode).Readonly().HasLabel("项目号").ShowInList();
                View.Property(p => p.ProductType).Readonly().UseEnumEditor().HasLabel("基本分类").ShowInList();
                View.Property(p => p.Type).Readonly().UseEnumEditor().HasLabel("工单类型").ShowInList();
                View.Property(p => p.PlanQty).Readonly().HasLabel("计划数量").ShowInList();
                View.Property(p => p.WorkShop).Readonly().HasLabel("车间").ShowInList();
                View.Property(p => p.Resource).Readonly().HasLabel("资源").ShowInList();
                View.Property(p => p.State).Readonly().ShowInList().HasOrderNo(9);
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
            //View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.WorkOrderOutputProductList).Show(ChildShowInWhere.Hide);
        }
    }
}
