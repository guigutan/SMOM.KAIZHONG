using SIE.Domain;
using SIE.EMS.AssetDisposals;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetDisposals
{
    /// <summary>
    /// 资产处置视图配置
    /// </summary>
    public class AssetDisposalViewConfig : WebViewConfig<AssetDisposal> 
    {
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AddBehavior("SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalBehavior");
			View.UseCommand(WebCommandNames.Add);
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.EditAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetRequisitions.Commands.DeleteAssetRequisitionCommand");
			View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.SubmitAssetDisposalCommand");
            View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.CancelAssetDisposalCommand");
            View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.ApprovalAssetDisposalCommand");
            View.FormEdit();
			View.Property(p => p.No).ShowInList(width: 120);
			View.Property(p => p.FactoryId).UseFactoryEditor();
			View.Property(p => p.ApprovalStatus);
			View.Property(p => p.AssetObject);
			View.Property(p => p.ManageDeptId);
			View.Property(p => p.UseDeptId);
			View.Property(p => p.WarehouseId);
			View.Property(p => p.IsFixAsset).Readonly();
			View.Property(p => p.DsiposalAmount);
			View.Property(p => p.Amount);
			View.Property(p => p.ApplicantId);
			View.Property(p => p.ApplyDate).UseDateEditor();
			View.Property(p => p.Remark);

			View.ChildrenProperty(p => p.AssetDisposalEquipmentList).HasOrderNo(1);
			View.ChildrenProperty(p => p.AssetDisposalFixtureList).HasOrderNo(2);
			View.ChildrenProperty(p => p.AssetDisposalSparePartList).HasOrderNo(3);
			View.ChildrenProperty(p => p.AssetDisposalAttachmentList).HasOrderNo(4);

            View.AttachChildrenProperty(typeof(WorkFlowRecord), e =>
            {
                var args = e as ChildPagingDataArgs;
                var parent = args.Parent as AssetDisposal;
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                    typeof(AssetDisposal).FullName, args.SortInfo, args.PagingInfo);

            }).HasLabel("审核记录").HasOrderNo(5);
        }

		///<summary>
		/// 配置表单视图
		/// </summary>
		protected override void ConfigDetailsView()
		{
            View.AddBehavior("SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalDetailsBehavior");
            View.UseCommand("SIE.Web.EMS.AssetDisposals.Commands.SaveAssetDisposalCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
            View.UseDetail(4);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.AssetObject);
            View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.ManageDeptId, null).Cascade(p => p.UseDeptId, null);
            View.Property(p => p.ManageDeptId).UseUserBussinessDepartmentEditor();
            View.Property(p => p.UseDeptId).UseUserBussinessDepartmentEditor();
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            });
            View.Property(p => p.IsFixAsset);
            View.Property(p => p.DsiposalAmount).UseSpinEditor(m => { m.DecimalPrecision = 2; m.MinValue = 0; });
			View.Property(p => p.Amount).Readonly();
            View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            });
            View.Property(p => p.ApplyDate).UseDateEditor();
            View.Property(p => p.Remark);
            View.ChildrenProperty(p => p.AssetDisposalEquipmentList).HasOrderNo(1).ViewGroup = "EditAssetDisposalEquipmentViewGroup";
            View.ChildrenProperty(p => p.AssetDisposalFixtureList).HasOrderNo(2).ViewGroup = "EditAssetDisposalFixtureViewGroup";
			View.ChildrenProperty(p => p.AssetDisposalSparePartList).HasOrderNo(3).ViewGroup = "EditAssetDisposalSparePartViewGroup"; 
			View.ChildrenProperty(p => p.AssetDisposalAttachmentList).HasOrderNo(4).ViewGroup = "EditAssetDisposalAttachmentViewGroup";
        }
	}

}
