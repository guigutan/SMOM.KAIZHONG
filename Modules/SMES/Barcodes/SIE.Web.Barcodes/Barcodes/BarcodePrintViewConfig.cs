using SIE.Barcodes;
using SIE.Core.WorkOrders;
using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.Web.Barcodes
{
    /// <summary>
    /// 条码打印视图配置类
    /// </summary>
    public class BarcodePrintViewConfig : WebViewConfig<PrintWorkOrder>
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
            View.FormEdit();
            View.DeclareGroup(BarcodePrintView);
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
            View.UseCommands("SIE.Web.Barcodes.BarcodePrintCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderProductCode).ShowInList(width: 150).HasLabel("产品编码");
                View.Property(p => p.WorkOrderProductName).ShowInList(width: 150).HasLabel("产品名称");
                View.Property(p => p.State).Readonly().Show();
                View.Property(p => p.PlanQty).ShowInList(width: 100);
                View.Property(p => p.PrintedQty).ShowInList(width: 100);
                View.Property(p => p.PlanBeginDate).ShowInList(width: 150);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(Barcode), (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var workorder = args.Parent.CastTo<WorkOrder>();
                    if (workorder == null) return new EntityList<Barcode>();
                    return RT.Service.Resolve<BarcodeController>().GetBarcodes(workorder.Id, sortInfo: (List<OrderInfo>)args.SortInfo, pagingInfo: args.PagingInfo);
                }, BarcodeViewConfig.WoPrintView, allowPaging: true).HasLabel("条码明细").Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置条码打印视图
        /// </summary>
        protected void ConfigPrintView()
        {
            View.UseCommands("SIE.Web.Barcodes.BarcodePrintCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.WorkOrderProductCode).ShowInList(width: 150).HasLabel("产品编码");
                View.Property(p => p.WorkOrderProductName).ShowInList(width: 150).HasLabel("产品名称");
                View.Property(p => p.State).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.IsPause).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PlanQty).HasLabel("计划数量").Show(ShowInWhere.All);
                View.Property(p => p.PrintedQty).HasLabel("已打印数量").Show(ShowInWhere.All);
                View.Property(p => p.PlanBeginDate).HasLabel("计划开始日期").ShowInList(width: 150);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
                View.AttachChildrenProperty(typeof(Barcode), (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var workorder = args.Parent.CastTo<WorkOrder>();
                    if (workorder == null) return new EntityList<Barcode>();
                    return RT.Service.Resolve<BarcodeController>().GetBarcodes(workorder.Id, sortInfo: (List<OrderInfo>)args.SortInfo, pagingInfo: args.PagingInfo);
                }, BarcodeViewConfig.WoPrintView, allowPaging: true).HasLabel("条码明细").Show(ChildShowInWhere.All);
            }
        }
    }
}