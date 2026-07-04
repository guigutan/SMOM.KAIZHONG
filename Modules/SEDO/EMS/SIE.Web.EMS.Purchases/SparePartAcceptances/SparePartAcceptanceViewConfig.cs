using SIE.Domain;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.Purchases.SparePartAcceptances.Commands;
using SIE.Web.EMS.Purchases.WorkFlows;
using System;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收视图配置
    /// </summary>
    internal class SparePartAcceptanceViewConfig : WebViewConfig<SparePartAcceptance>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.AddBehavior("SIE.Web.EMS.Purchases.Common.ApprovalBehavior");
            View.AddBehavior("SIE.Web.EMS.Purchases.SparePartAcceptances.SparePartAcceptBehavior");
            View.UseCommands(typeof(SaveSparePartAcceptanceCommand).FullName, typeof(SubmitSparePartAcceptanceCommand).FullName, typeof(CancelSparePartAcceptanceCommand).FullName,
                typeof(ExamineSparePartAcceptCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.AcceptanceNo).ShowInList(130);
            View.Property(p => p.ApprovalStatus).ShowInList(60);
            View.Property(p => p.SparePartReceiveId).HasLabel("接收单号").ShowInList(120);
            View.Property(p => p.ReceiveType).ShowInList(80);
            View.Property(p => p.SupplierId).HasLabel("供应商编码").ShowInList(120);
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.SparePartId).HasLabel("备件编码").ShowInList(130);
            View.Property(p => p.SparePartName).ShowInList(200);
            View.Property(p => p.ControlMethod).ShowInList(80);
            View.Property(p => p.ReceiveQty).ShowInList(80);
            View.Property(p => p.PassQty).ShowInList(80);
            View.Property(p => p.UnqualifiedQty).ShowInList(100);
            View.Property(p => p.UnitName).ShowInList(60);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate);
            View.ChildrenProperty(p => p.DetailList).HasLabel("备件明细").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(SparePartAcceptanceLot), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<SparePartAcceptance>();
                if (parent == null)
                {
                    return new EntityList<SparePartAcceptanceLot>();
                }
                return RT.Service.Resolve<SparePartAcceptanceController>().GetAcceptanceLotInfo(parent.Id, args.PagingInfo);
            }).HasLabel("批次明细").HasOrderNo(2);
            View.AttachChildrenProperty(typeof(SparePartAcceptanceSn), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<SparePartAcceptance>();
                if (parent == null)
                {
                    return new EntityList<SparePartAcceptanceSn>();
                }
                return RT.Service.Resolve<SparePartAcceptanceController>().GetAcceptanceSnInfo(parent.Id, args.PagingInfo);
            }).HasLabel("序列号明细").HasOrderNo(3);
            View.ChildrenProperty(p => p.AcceptanceItemList).HasLabel("验收项目").HasOrderNo(4);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(5);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<SparePartAcceptance>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(SparePartAcceptance).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.PurSeeView).HasLabel("审核记录").HasOrderNo(6);
        }
    }
}