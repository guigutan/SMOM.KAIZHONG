using SIE.Domain;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.Purchases.FixtureAcceptances.Commands;
using SIE.Web.EMS.Purchases.WorkFlows;
using System;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 工治具验收视图配置
	/// </summary>
	internal class FixtureAcceptanceViewConfig : WebViewConfig<FixtureAcceptance>
	{	
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
            View.DisableEditing();
            View.AddBehavior("SIE.Web.EMS.Purchases.Common.ApprovalBehavior");
            View.AddBehavior("SIE.Web.EMS.Purchases.FixtureAcceptances.FixtureAcceptanceBehavior");
            View.UseCommands(typeof(SaveFixtureAcceptanceCommand).FullName, 
            typeof(SubmitFixtureAcceptanceCommand).FullName, typeof(CancelFixtureAcceptanceCommand).FullName,
            typeof(ExamineFixtureAcceptanceCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.AcceptanceNo).ShowInList(130);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.FixtureReceiveId).HasLabel("接收单号").ShowInList(120);
            View.Property(p => p.ReceiveType).ShowInList(80);
            View.Property(p => p.SupplierId).HasLabel("供应商编码").ShowInList(120);
            View.Property(p => p.SupplierName).ShowInList(200);
            View.Property(p => p.FixtureEncodeId).HasLabel("工治具编码").ShowInList(130);
            View.Property(p => p.ModelCode).ShowInList(200);
            View.Property(p => p.ModelName).ShowInList(80);
            View.Property(p => p.ReceiveQty).ShowInList(80);
            View.Property(p => p.PassQty).ShowInList(80);
            View.Property(p => p.UnqualifiedQty).ShowInList(100);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.FixtureAcceptanceDetailList).HasLabel("验收明细").HasOrderNo(1);

            View.AttachChildrenProperty(typeof(FixtureAcceptanceSn), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<FixtureAcceptance>();
                if (parent == null)
                {
                    return new EntityList<FixtureAcceptanceSn>();
                }
                return RT.Service.Resolve<FixtureAcceptancesController>().GetAcceptanceSnInfo(parent.Id, args.PagingInfo);
            }).HasLabel("序列号明细").HasOrderNo(3);
            View.ChildrenProperty(p => p.FixtureAcceptanceItemList).HasLabel("验收项目").HasOrderNo(4);
            View.ChildrenProperty(p => p.FixtureAcceptanceAttachmentList).HasLabel("附件").HasOrderNo(5);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<FixtureAcceptance>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(FixtureAcceptance).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.PurSeeView).HasLabel("审核记录").HasOrderNo(6);
        }
	}
}