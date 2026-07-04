using SIE.Domain;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.EMS.Purchases.PaymentPlans.Commands;
using SIE.Web.EMS.Purchases.WorkFlows;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PaymentPlans
{
    /// <summary>
    /// 付款计划视图配置
    /// </summary>
    internal class PaymentPlanViewConfig : WebViewConfig<PaymentPlan>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.Purchases.Common.ApprovalBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.PaymentPlans.Commands.AddPaymentPlanCommand", "SIE.Web.EMS.Purchases.Common.Commands.ApprovalStatusEditCommand",
                typeof(DeletePaymentPlanCommand).FullName, typeof(SubmitPaymentPlanCommand).FullName, typeof(CancelPaymentPlanCommand).FullName,
                typeof(ExaminePaymentPlanCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.PaymentOrderNo).ShowInList(130);
            View.Property(p => p.PurchaseOrderId).ShowInList(130).HasLabel("采购订单号");
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.ApprovedTime).ShowInList(150);
            View.Property(p => p.SupplierCode).ShowInList(120);
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.PaymentTermsId).ShowInList(100).HasLabel("付款阶段");
            View.Property(p => p.PaymentDate).ShowInList(150);
            View.Property(p => p.TotalAmount).ShowInList(130);
            View.Property(p => p.Currency).ShowInList(60);
            View.Property(p => p.AmountUnit).ShowInList(80);
            View.Property(p => p.PlanAmount).ShowInList(130);
            View.Property(p => p.Amount).ShowInList(130);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.AttachmentList).UseViewGroup("Readonly").HasLabel("附件").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<PaymentPlan>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(PaymentPlan).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.PurSeeView).HasLabel("审核记录").HasOrderNo(2);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(4);
            View.AddBehavior("SIE.Web.EMS.Purchases.PaymentPlans.PaymentPlanBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SavePaymentPlanCommand).FullName);
            View.Property(p => p.PaymentOrderNo).Readonly();
            View.Property(p => p.PurchaseOrderId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var plan = source as PaymentPlan;
                if (plan == null)
                {
                    return new EntityList<PurchaseOrder>();
                }
                return RT.Service.Resolve<PurchaseOrderController>().GetPaymentPlanOrder(pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.TotalAmount), nameof(e.PurchaseOrder.TotalAmount));
                keyValues.Add(nameof(e.Currency), nameof(e.PurchaseOrder.Currency));
                keyValues.Add(nameof(e.AmountUnit), nameof(e.PurchaseOrder.Currency));
                keyValues.Add(nameof(e.PlanAmount), nameof(e.PurchaseOrder.TotalPlanned));
                m.DicLinkField = keyValues;
            }).HasLabel("采购订单号").Cascade(p => p.PaymentTermsId, null);
            View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.DepartmentId, null).Readonly(p => p.PurchaseOrderId > 0);
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor().Readonly(p => p.PurchaseOrderId > 0);
            View.Property(p => p.SupplierId).UseSupplierEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("供应商编码").Readonly(p => p.PurchaseOrderId > 0);
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.PaymentTermsId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var plan = source as PaymentPlan;
                if (plan == null || plan.PurchaseOrderId == null)
                {
                    return new EntityList<PaymentTerms>();
                }
                return RT.Service.Resolve<PurchaseOrderController>().GetPaymentTermsByOrderId(plan.PurchaseOrderId.Value, pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.PaymentDate), nameof(e.PaymentTerms.PaymentDate));
                keyValues.Add(nameof(e.Amount), nameof(e.PaymentTerms.Amount));
                m.DicLinkField = keyValues;
            }).HasLabel("付款阶段").Readonly(p => p.PurchaseOrderId == null);
            View.Property(p => p.PaymentDate).Readonly(p => p.PurchaseOrderId > 0);
            View.Property(p => p.TotalAmount).Readonly();
            View.Property(p => p.Currency).Readonly();
            View.Property(p => p.AmountUnit).Readonly();
            View.Property(p => p.PlanAmount).Readonly();
            View.Property(p => p.Amount).UseSpinEditor(p => p.MinValue = 0).Readonly(p => p.PurchaseOrderId > 0);
            View.Property(p => p.Remark);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(1);
        }
    }
}