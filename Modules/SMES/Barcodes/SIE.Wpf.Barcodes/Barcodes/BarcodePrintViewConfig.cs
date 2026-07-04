using SIE.Barcodes;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Wpf.Barcodes.Commonds;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.Barcodes
{
    /// <summary>
    /// 条码打印视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class BarcodePrintViewConfig : WPFViewConfig<PrintWorkOrder>
    {
        /// <summary>
        /// 条码打印ViewGroup
        /// </summary>
        public const string BarcodePrintView = "BarcodePrintView";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BarcodePrintView);
            View.DomainName("条码打印");
            if (ViewGroup == BarcodePrintView)
            {
                ConfigPrintView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(PrintWorkOrder));
            View.UseCommands(typeof(BarcodePrintCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码");
                View.Property(p => p.WorkOrderProductName).HasLabel("产品名称");
                View.Property(p => p.PlanQty).UseSpinEditor(p => p.Decimals = 0).UseListSetting(e => e.ListGridWidth = 100);
                View.Property(p => p.PrintedQty).UseSpinEditor(p => p.Decimals = 0).UseListSetting(e => e.ListGridWidth = 100);
                View.Property(p => p.PlanBeginDate).UseListSetting(e => e.ListGridWidth = 160);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(Barcode), (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var workorder = args.Parent.CastTo<WorkOrder>();
                    if (workorder == null) return new EntityList<Barcode>();
                    return RT.Service.Resolve<BarcodeController>().GetBarcodes(workorder.Id, sortInfo: (List<OrderInfo>)args.SortInfo, pagingInfo: args.PagingInfo);
                }).HasLabel("条码明细").UseViewGroup(BarcodeViewConfig.WoPrintView).Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置条码打印主表视图
        /// </summary>
        protected void ConfigPrintView()
        {
            View.AssignAuthorize(typeof(PrintWorkOrder));
            View.UseCommands(typeof(BarcodePrintCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Show(ShowInWhere.All);
                View.Property(p => p.PlanQty).UseSpinEditor(p => p.Decimals = 0).HasLabel("计划数量").UseListSetting(e => e.ListGridWidth = 100).Show(ShowInWhere.All);
                View.Property(p => p.PrintedQty).UseSpinEditor(p => p.Decimals = 0).HasLabel("已打印数量").UseListSetting(e => e.ListGridWidth = 100).Show(ShowInWhere.All);
                View.Property(p => p.PlanBeginDate).HasLabel("计划开始日期").UseListSetting(e => e.ListGridWidth = 160).Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(Barcode), (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var workorder = args.Parent.CastTo<WorkOrder>();
                    if (workorder == null) return new EntityList<Barcode>();
                    return RT.Service.Resolve<BarcodeController>().GetBarcodes(workorder.Id, sortInfo: (List<OrderInfo>)args.SortInfo, pagingInfo: args.PagingInfo);
                }).HasLabel("条码明细").UseViewGroup(BarcodePrintViewConfig.BarcodePrintView)
                .Show(ChildShowInWhere.All);
            }
        }
    }
}