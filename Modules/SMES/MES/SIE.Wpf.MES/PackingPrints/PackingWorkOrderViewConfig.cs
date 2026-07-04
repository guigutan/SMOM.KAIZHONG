using SIE.Domain;
using SIE.MES.PackingPrints;
using SIE.Wpf.MES.PackingPrints.Commonds;

namespace SIE.Wpf.MES.PackingPrints
{
    /// <summary>
    /// 包装条码打印视图
    /// </summary>
    internal class PackingWorkOrderViewConfig : WPFViewConfig<PackingWorkOrder>
    {
        /// <summary>
        /// 包装条码打印视图
        /// </summary>
        public const string PackingPrintView = "PackingPrintView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(PackingWorkOrder));
            View.DeclareExtendViewGroup(PackingPrintView);
            if (ViewGroup == PackingPrintView)
                ConfigPackingPrintView();
        }

        /// <summary>
        /// 包装条码打印视图
        /// </summary>
        void ConfigPackingPrintView()
        {
            View.UseCommands(typeof(PackingBarcodePrintCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.No).HasLabel("工单号").Readonly().ShowInList(150);
                View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Readonly().ShowInList(150);
                View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Readonly().ShowInList(150);
                View.Property(p => p.PlanQty).Readonly().Show();
                View.Property(p => p.PanelQty).Readonly().Show();
                View.Property(p => p.State).Readonly().Show();
                View.Property(p => p.PlanBeginDate).Readonly().ShowInList(150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide).HasLabel("工单BOM信息");
                View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Hide).HasLabel("工单与日志关系");
                View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工单与工序清单关系");
                View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Hide).HasLabel("工单与工序BOM关系");
                //View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide).HasLabel("工单与属性值关系");
                View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Hide).HasLabel("工单与包装规则关系");
                View.AttachChildrenProperty(typeof(PackingBarcode), (e) =>
                {
                    var child = e as ChildPagingDataArgs;
                    var packingWorkOrder = e.Parent as PackingWorkOrder;
                    if (packingWorkOrder == null) return new EntityList<PackingBarcode>();
                    return RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcodeListByWorkOrderId(packingWorkOrder.Id, child.PagingInfo, child.SortInfo);
                }).HasLabel("打印明细").Show(ChildShowInWhere.All);
            }
        }
    }
}
