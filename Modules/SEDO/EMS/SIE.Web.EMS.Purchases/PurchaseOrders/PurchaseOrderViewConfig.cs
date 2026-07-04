using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Common;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.EMS.Purchases.PurchaseOrders.Commands;
using SIE.Web.EMS.Purchases.WorkFlows;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单视图配置
    /// </summary>
    internal class PurchaseOrderViewConfig : WebViewConfig<PurchaseOrder>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.Purchases.Common.ApprovalBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.PurchaseOrders.Commands.AddPurOrderCommand", "SIE.Web.EMS.Purchases.Common.Commands.ApprovalStatusEditCommand",
                typeof(DeletePurOrderCommand).FullName, typeof(SubmitPurOrderCommand).FullName, typeof(CancelPurOrderCommand).FullName,
                typeof(ExaminePurOrderCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.OrderNo).ShowInList(130).HasLabel("采购订单号");
            View.Property(p => p.Description).ShowInList(200);
            View.Property(p => p.PurchaseOrderStatus).ShowInList(80).HasLabel("订单状态");
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.ApprovedDate).ShowInList(150);
            View.Property(p => p.PurchaseObjectType).ShowInList(80);
            View.Property(p => p.SupplierId).ShowInList(120).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.PurchaseCategroy).UseCatalogEditor(e => { e.CatalogType = PurchaseOrder.PurchaseClassify; e.CatalogReloadData = true; }).ShowInList(100);
            View.Property(p => p.VarietyQuantity).ShowInList(60);
            View.Property(p => p.TotalQty).ShowInList(70);
            View.Property(p => p.TotalAmount).ShowInList(150);
            View.Property(p => p.Currency).ShowInList(60);
            View.Property(p => p.AmountUnit).ShowInList(80);
            View.Property(p => p.TotalPlanned).ShowInList(150);
            View.Property(p => p.AmountPaid).ShowInList(150);
            View.Property(p => p.ContractCode).ShowInList(150);
            View.Property(p => p.BuyerId).ShowInList(120).HasLabel("采购人");
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.All);
            View.Property(p => p.CreateDate).Show(ShowInWhere.All);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.All);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.All);
            View.ChildrenProperty(p => p.DetailList).HasLabel("采购明细").HasOrderNo(1);
            View.ChildrenProperty(p => p.PaymentTermsList).HasLabel("付款条件").HasOrderNo(2);
            View.ChildrenProperty(p => p.AttachmentList).UseViewGroup("Readonly").HasLabel("附件").HasOrderNo(3);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<PurchaseOrder>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(PurchaseOrder).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.PurSeeView).HasLabel("审核记录").HasOrderNo(4);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(8);
            View.AddBehavior("SIE.Web.EMS.Purchases.PurchaseOrders.PurOrderBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SavePurOrderCommand).FullName);
            View.Property(p => p.OrderNo).HasLabel("采购订单号").ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.Description).ShowInDetail(columnSpan: 2);
            View.Property(p => p.FactoryId).ShowInDetail(columnSpan: 2).UseFactoryEditor().Cascade(p => p.DepartmentId, null);
            View.Property(p => p.DepartmentId).ShowInDetail(columnSpan: 2).UseUserBudgetDepartmentEditor();
            View.Property(p => p.PurchaseCategroy).ShowInDetail(columnSpan: 2).UseCatalogEditor(e => { e.CatalogType = PurchaseOrder.PurchaseClassify; e.CatalogReloadData = true; });
            View.Property(p => p.SupplierId).UseSupplierEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                m.DicLinkField = keyValues;
            }).ShowInDetail(columnSpan: 2).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ContractCode).ShowInDetail(columnSpan: 2).HasLabel("合同编号");
            View.Property(p => p.PurchaseObjectType).UsePurchaseObjectEditor().ShowInDetail(columnSpan: 2);
            View.Property(p => p.VarietyQuantity).ShowInDetail(columnSpan: 2).Readonly().HasLabel("品种数量");
            View.Property(p => p.TotalAmount).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.Currency).ShowInDetail(columnSpan: 2);
            View.Property(p => p.AmountUnit).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.BuyerId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).ShowInDetail(columnSpan: 2).HasLabel("采购人");
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 4);
            View.ChildrenProperty(p => p.DetailList).UseViewGroup(PurchaseOrderItemViewConfig.EditView).HasLabel("采购明细").HasOrderNo(1);
            View.ChildrenProperty(p => p.PaymentTermsList).UseViewGroup(PaymentTermsViewConfig.EditView).HasLabel("付款条件").HasOrderNo(2);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(3);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.OrderNo).ShowInList(130).HasLabel("采购订单号");
            View.Property(p => p.Description).ShowInList(200);
        }
    }
}