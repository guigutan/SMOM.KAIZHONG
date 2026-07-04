using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.MetaModel.View;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.EMS.Purchases.EquipmentSetups.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试工作计划视图配置
    /// </summary>
    public class EquipmentSetupPlanViewConfig : WebViewConfig<EquipmentSetupPlan>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(PlanStartCommand).FullName, typeof(PlanEndCommand).FullName);
            View.Property(p => p.TodoItem).Readonly().ShowInList(400);
            View.Property(p => p.PlanStartDateTime).Readonly().ShowInList(150);
            View.Property(p => p.PlanEndDateTime).Readonly().ShowInList(150);
            //审核状态为【待提交】、【驳回】的数据才能删除和修改
            View.Property(p => p.WorkHours).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft
                && p.ApprovalStatus != ApprovalStatus.Reject)
                .UseSpinEditor(p => p.DecimalPrecision = 1).ShowInList(80);
            View.Property(p => p.WorkStatus).Readonly().ShowInList(80);
            View.Property(p => p.ActualStartDateTime).Readonly(p => p.WorkStatus == WorkStatus.NotStarted).ShowInList(150);
            View.Property(p => p.ActualEndDateTime).Readonly(p => p.WorkStatus != WorkStatus.Finish).ShowInList(150);
            View.Property(p => p.EmployeeCount).Readonly().ShowInList(80);
            View.Property(p => p.ActualWorkHours).Readonly().ShowInList(100);

            //委外并且审核状态为【待提交】、【驳回】的数据才能删除和修改
            View.Property(p => p.SupplierId).UseSupplierEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("供应商编码").ShowInList(100).Cascade(p => p.PurchaseOrderId, null).Readonly(p => !p.OutSource
                || p.ApprovalStatus == ApprovalStatus.PendingReview
                || p.ApprovalStatus == ApprovalStatus.UnderReview
                || p.ApprovalStatus == ApprovalStatus.Audited);
            View.Property(p => p.SupplierName).Readonly().ShowInList(200);

            //委外并且审核状态为【待提交】、【驳回】的数据才能删除和修改
            View.Property(p => p.PurchaseOrderId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var plan = source as EquipmentSetupPlan;
                if (plan == null || plan.SupplierId == null)
                {
                    return new EntityList<PurchaseOrder>();
                }
                return RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersBySupplier(plan.SupplierId.Value, pagingInfo, keyword);
            }).ShowInList(130).Readonly(p => !p.OutSource ||
            p.ApprovalStatus == ApprovalStatus.PendingReview || p.ApprovalStatus == ApprovalStatus.UnderReview || p.ApprovalStatus == ApprovalStatus.Audited);
            View.Property(p => p.ContactPerson).ShowInList(80).Readonly(p => !p.OutSource ||
            p.ApprovalStatus == ApprovalStatus.PendingReview || p.ApprovalStatus == ApprovalStatus.UnderReview || p.ApprovalStatus == ApprovalStatus.Audited);
            View.Property(p => p.ContactDetail).ShowInList(130).Readonly(p => !p.OutSource ||
            p.ApprovalStatus == ApprovalStatus.PendingReview || p.ApprovalStatus == ApprovalStatus.UnderReview || p.ApprovalStatus == ApprovalStatus.Audited);

            //审核状态为【待提交】、【驳回】的数据才能删除和修改
            View.Property(p => p.Remark).Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.TodoItem).Readonly().ShowInList(400);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(EquipmentSetup));
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddSetupPlanCommand", WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.TodoItem).ShowInList(400);
                View.Property(p => p.PlanStartDateTime).ShowInList(150);
                View.Property(p => p.PlanEndDateTime).ShowInList(150);
                View.Property(p => p.WorkHours).UseSpinEditor(p => p.DecimalPrecision = 1).ShowInList(80);
                View.Property(p => p.WorkStatus).Readonly().ShowInList(80);
                View.Property(p => p.ActualStartDateTime).Readonly().ShowInList(150);
                View.Property(p => p.ActualEndDateTime).Readonly().ShowInList(150);
                View.Property(p => p.EmployeeCount).Readonly().ShowInList(80);
                View.Property(p => p.ActualWorkHours).Readonly().ShowInList(100);
                View.Property(p => p.SupplierId).UseSupplierEditor().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("供应商编码").ShowInList(100).Cascade(p => p.PurchaseOrderId, null).Readonly(p => !p.OutSource);
                View.Property(p => p.SupplierName).Readonly().ShowInList(200);
                View.Property(p => p.PurchaseOrderId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var plan = source as EquipmentSetupPlan;
                    if (plan == null || plan.SupplierId == null)
                    {
                        return new EntityList<PurchaseOrder>();
                    }
                    return RT.Service.Resolve<PurchaseOrderController>().GetPurchaseOrdersBySupplier(plan.SupplierId.Value, pagingInfo, keyword);
                }).Readonly(p => !p.OutSource).ShowInList(130);
                View.Property(p => p.ContactPerson).Readonly(p => !p.OutSource).ShowInList(80);
                View.Property(p => p.ContactDetail).Readonly(p => !p.OutSource).ShowInList(130);
                View.Property(p => p.Remark).ShowInList(200);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}