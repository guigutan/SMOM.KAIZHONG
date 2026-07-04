using SIE.Domain;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands;
using SIE.Web.EMS.Purchases.WorkFlows;
using System;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收视图配置
    /// </summary>
    internal class EquipmentAcceptanceViewConfig : WebViewConfig<EquipmentAcceptance>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.Purchases.Common.ApprovalBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.AcceptanceCommand",
                typeof(SubmitEquipAcceptCommand).FullName, typeof(CancelEquipAcceptCommand).FullName, typeof(ExamineEquipAcceptCommand).FullName,
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.AcceptanceNo).ShowInList(130);
            View.Property(p => p.ApprovalStatus).HasLabel("状态").ShowInList(60);
            View.Property(p => p.ReceiveType).ShowInList(80);
            View.Property(p => p.SupplierId).HasLabel("供应商编码").ShowInList(120);
            View.Property(p => p.SupplierName).HasLabel("供应商名称").ShowInList(200);
            View.Property(p => p.CustomerId).HasLabel("客户编码").ShowInList(120);
            View.Property(p => p.CustomerName).HasLabel("客户名称").ShowInList(200);
            View.Property(p => p.EquipModelId).HasLabel("设备型号编码").ShowInList(120);
            View.Property(p => p.EquipModelName).HasLabel("设备型号名称").ShowInList(200);
            View.Property(p => p.ReceiveQty).ShowInList(80);
            View.Property(p => p.PassQty).ShowInList(80);
            View.Property(p => p.UnqualifiedQty);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).HasLabel("设备明细").HasOrderNo(1);
            View.ChildrenProperty(p => p.EquipmentAcceptanceItemList).HasLabel("验收项目").HasOrderNo(2);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(3);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;

                var parent = args.Parent.CastTo<EquipmentAcceptance>();

                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(EquipmentAcceptance).FullName, args.SortInfo, args.PagingInfo);

            }, WorkFlowRecordViewConfig.PurSeeView).HasLabel("审核记录").HasOrderNo(4);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(8);
            View.ClearCommands();
            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentAcceptances.EquipAcceptBehavior");
            View.UseCommands(typeof(SaveEquipAcceptCommand).FullName, typeof(DetermineEquipAcceptCommand).FullName);
            View.Property(p => p.AcceptanceNo).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.FactoryId).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.DepartmentId).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.ReceiveType).Readonly().ShowInDetail(columnSpan: 2);
            View.Property(p => p.EquipModelId).HasLabel("型号编码").ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.EquipModelName).HasLabel("型号名称").ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ReceiveQty).ShowInDetail(columnSpan: 2).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.ApprovalStatus).ShowInDetail(columnSpan: 2).ShowInDetail(columnSpan: 2).HasLabel("状态").Readonly();
            View.Property(p => p.SupplierId).UseSupplierEditor().ShowInDetail(columnSpan: 2).HasLabel("供应商编码").Readonly();
            View.Property(p => p.SupplierName).HasLabel("供应商名称").ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.PassQty).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.UnqualifiedQty).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.Message).Readonly().ShowInDetail(columnSpan: 8);
            View.Property(p => p.Sn).UseDisplayEditor(p => p.XType = "EquipmentAcceptanceSnEditor").ShowInDetail(columnSpan: 4);
            View.Property(p => p.AcceptanceStatus).ShowInDetail(columnSpan: 2);
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 2);
            View.ChildrenProperty(p => p.DetailList).UseViewGroup(EquipmentAcceptanceDetailViewConfig.EditView).HasLabel("设备明细").HasOrderNo(1);
            View.ChildrenProperty(p => p.EquipmentAcceptanceItemList).UseViewGroup(EquipmentAcceptanceItemViewConfig.EditView).HasLabel("验收项目").HasOrderNo(2);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(3);
        }
    }
}