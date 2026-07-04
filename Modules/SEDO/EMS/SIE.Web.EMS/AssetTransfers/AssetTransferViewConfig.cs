using SIE.Domain;
using SIE.EMS.AssetTransfers;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.AssetTransfers.Commands;
using SIE.Web.EMS.Common.Commands;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.AssetTransfers
{
    /// <summary>
    /// 资产调拨视图配置
    /// </summary>
    public class AssetTransferViewConfig : WebViewConfig<AssetTransfer>
    {

        /// <summary>
        /// 上传视图
        /// </summary>
        public const string UploadView = "UploadView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(UploadView);
            if (ViewGroup == UploadView)
            {
                UploadFileView();
            }
        }





        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.AssetTransfers.AssetTransferListBehavior");
            View.AddBehavior("SIE.Web.EMS.Common.Script.ApprovalBehavior");
            View.UseCommands("SIE.Web.EMS.AssetTransfers.Commands.AddAssetTransferCommand"
                , "SIE.Web.EMS.AssetTransfers.Commands.EditAssetTransferCommand"
                , typeof(PromptlyDeleteCommand).FullName
                , typeof(SubmitCommand).FullName
                , typeof(ApprovalCommand).FullName
                 , typeof(CancelCommand).FullName
                , typeof(SendAssetTransfersCommand).FullName
                , "SIE.Web.EMS.AssetTransfers.Commands.ReceivedCommand"
                );
            View.DisableEditing();
            View.Property(p => p.TransferNo).Readonly();
            View.Property(p => p.TransferType).Readonly();
            View.Property(p => p.ApprovalStatus).Readonly();
            View.Property(p => p.TransferStatus).Readonly();
            View.Property(p => p.SourceFactoryId).HasLabel("原工厂").Readonly();
            View.Property(p => p.ManageDeptId).HasLabel("原管理部门").Readonly();
            View.Property(p => p.UseDeptId).HasLabel("原使用部门").Readonly();

            View.Property(p => p.TargetFactoryId).HasLabel("目标工厂").Readonly();
            View.Property(p => p.TargetManageDeptId).HasLabel("目标管理部门").Readonly();
            View.Property(p => p.TargetUseDepartId).HasLabel("目标使用部门").Readonly();
            View.Property(p => p.IsAsset).Readonly();
            View.Property(p => p.ApplyDate).Readonly();
            View.Property(p => p.ApplicantId).HasLabel("申请人").Readonly();
            View.Property(p => p.Remark).Readonly();
            View.ChildrenProperty(p => p.AssetTransferDetailList).HasLabel("设备清单").HasOrderNo(1);
            View.ChildrenProperty(p => p.AssetTransferAttachmentList).UseViewGroup("Readonly").HasLabel("附件").HasOrderNo(2);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as AssetTransfer;
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                    typeof(AssetTransfer).FullName, args.SortInfo, args.PagingInfo);

            }, ListView).HasLabel("审核记录").HasOrderNo(3);
        }


        /// <summary>
        /// 明细编辑
        /// </summary>

        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.AssetTransfers.AssetTransferBehavior");
            View.ClearCommands();
            View.ReplaceCommands(WebCommandNames.Save, typeof(SaveAssetTransfer).FullName);
            View.UseDetail(4);
            View.Property(p => p.TransferNo).Readonly();
            View.Property(p => p.SourceFactoryId).UseFactoryEditor().Cascade(p => p.ManageDeptId, null).Cascade(p => p.UseDeptId, null);
            View.Property(p => p.ManageDeptId).UseUserBussinessDepartmentEditor(factoryIdPropertyName: "SourceFactoryId");
            View.Property(p => p.UseDeptId).UseUserBussinessDepartmentEditor(factoryIdPropertyName: "SourceFactoryId");

            View.Property(p => p.TransferType);
            View.Property(p => p.TargetFactoryId).UseFactoryEditor().Readonly(p => p.TransferType == TransferType.InsideFactory)
                .Cascade(p => p.TargetManageDeptId, null).Cascade(p => p.TargetUseDepartId, null);
            View.Property(p => p.TargetManageDeptId).UseUserBussinessDepartmentEditor(factoryIdPropertyName: "TargetFactoryId");
            View.Property(p => p.TargetUseDepartId).UseUserBussinessDepartmentEditor(factoryIdPropertyName: "TargetFactoryId");
            View.Property(p => p.IsAsset);
            View.Property(p => p.ApplyDate);
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 2);
            View.ChildrenProperty(p => p.AssetTransferDetailList).UseViewGroup(AssetTransferDetailViewConfig.EditView).HasLabel("设备清单");
            View.ChildrenProperty(p => p.AssetTransferAttachmentList).HasLabel("附件");

        }

        /// <summary>
        /// 上传文件视图
        /// </summary>
        protected void UploadFileView()
        {
            View.UseDetail(2);
            View.ClearCommands();
            View.Property(p => p.TransferNo).Readonly().Show();
            View.Property(p => p.TransferType).Readonly().Show();
            View.ChildrenProperty(p => p.AssetTransferAttachmentList).UseViewGroup("UploadView").HasLabel("附件").Show( ChildShowInWhere.Detail);
        }


    }
}