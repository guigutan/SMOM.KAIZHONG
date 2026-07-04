using SIE.Domain;
using SIE.EMS.ViceTransfers;
using SIE.Equipments.WorkFlows;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.EMS.Common.Commands;
using SIE.Web.EMS.ViceTransfers.Commands;
using SIE.Web.Resources;
using System.Collections.Generic;

namespace SIE.Web.EMS.ViceTransfers
{
    /// <summary>
    /// 副资产调拨视图
    /// </summary>
    public class ViceTransferViewConfig : WebViewConfig<ViceTransfer>
    {
        /// <summary>
        /// 执行工治具调拨视图
        /// </summary>
        public const string ExecuteFixtureView = "ExecuteFixtureView";

        /// <summary>
        /// 执行备件调拨视图
        /// </summary>
        public const string ExecuteSparePartView = "ExecuteSparePartView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ExecuteFixtureView);
            if (ViewGroup == ExecuteFixtureView)
            {
                GetExecuteFixtureView();
            }
            if (ViewGroup == ExecuteSparePartView)
            {
                GetExecuteSparePartView();
            }
        }

        /// <summary>
        /// 获取工治具执行视图
        /// </summary>
        protected void GetExecuteFixtureView()
        {
            CommonExecuteTitelCloumn();

            View.AssociateChildrenProperty(ViceTransfer.ViceTransferFixtureDetailListProperty, c =>
            {
                var viceTransfer = c.Parent as ViceTransfer;
                if (viceTransfer != null)
                {
                    return RT.Service.Resolve<ViceTransferController>().GetViceTransferFixtureDetails(viceTransfer.Id);
                }
                return new EntityList<ViceTransferFixtureDetail>();
            }).HasLabel("调拨明细").HasOrderNo(1).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 获取备件执行视图
        /// </summary>
        protected void GetExecuteSparePartView()
        {
            CommonExecuteTitelCloumn();

            View.AssociateChildrenProperty(ViceTransfer.ViceTransferSparePartDetailListProperty, c =>
            {
                var viceTransfer = c.Parent as ViceTransfer;
                if (viceTransfer != null)
                {
                    return RT.Service.Resolve<ViceTransferController>().GetViceTransferSparePartDetails(viceTransfer.Id);
                }
                return new EntityList<ViceTransferSparePartDetail>();
            }).HasLabel("调拨明细").HasOrderNo(1).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 获取通用表头
        /// </summary>
        protected void CommonExecuteTitelCloumn()
        {
            View.ClearCommands();
            View.UseDetail(4);
            View.UseCommand(typeof(ExecutSaveComamns).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.TransferNo).Readonly().Show();
                View.Property(p => p.FactoryId).Readonly().Show();
                View.Property(p => p.ViceAssetObject).Readonly().Show();
                View.Property(p => p.WarehouseId).Readonly().Show().HasLabel("来源仓库");
                View.Property(p => p.TargetWarehouseId).Show().Readonly();
                View.Property(p => p.ApplyDate).Show().Readonly();
                View.Property(p => p.ApplicantId).Show().Readonly();
                View.Property(p => p.Remark).Show().Readonly();
            }
        }



        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.Common.Script.ApprovalBehavior");
            View.AddBehavior("SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferListBehavior");
            View.UseCommands("SIE.Web.EMS.ViceTransfers.Commands.AddTransferCommand", "SIE.Web.EMS.ViceTransfers.Commands.EditTransferCommand",
                 typeof(PromptlyDeleteCommand).FullName,
                "SIE.Web.EMS.ViceTransfers.Commands.ExecuteTransferCommand",
                  typeof(SubmitCommand).FullName, typeof(ApprovalCommand).FullName, typeof(CancelCommand).FullName, typeof(ShutdownCommand).FullName
                );

            View.DisableEditing();
            View.Property(p => p.TransferNo).ShowInList(120).Readonly();
            View.Property(p => p.FactoryId).ShowInList(120).Readonly();
            View.Property(p => p.ViceAssetObject).ShowInList(120).Readonly();
            View.Property(p => p.ApprovalStatus).ShowInList(120).Readonly();
            View.Property(p => p.TransferStatus).ShowInList(140).Readonly();
            View.Property(p => p.WarehouseId).HasLabel("来源仓库").ShowInList(100).Readonly();
            View.Property(p => p.TargetWarehouseId).HasLabel("目标仓库").ShowInList(100).Readonly();
            View.Property(p => p.ApplyDate).HasLabel("申请日期").ShowInList(100).Readonly();

            View.Property(p => p.ApplicantId).ShowInList(100).HasLabel("申请人").Readonly();
            View.Property(p => p.Remark).ShowInList(200).Readonly();
            View.Property(p => p.CloseRemark).ShowInList(200).Readonly();
            View.ChildrenProperty(p => p.ViceTransferFixtureList).HasLabel("需求清单").HasOrderNo(1);
            View.ChildrenProperty(p => p.ViceTransferFixtureDetailList).UseViewGroup(ViceTransferFixtureDetailViewConfig.ReadOnlyListView).HasLabel("调拨明细").HasOrderNo(2);

            View.ChildrenProperty(p => p.ViceTransferSparePartList).HasLabel("需求清单").HasOrderNo(3);
            View.ChildrenProperty(p => p.ViceTransferSparePartDetailList).UseViewGroup(ViceTransferSparePartDetailViewConfig.ReadOnlyListView).HasLabel("调拨明细").HasOrderNo(4);
            View.ChildrenProperty(p => p.ViceTransferAttachmentList).UseViewGroup("Readonly").HasLabel("附件").HasOrderNo(6);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as ViceTransfer;
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                    typeof(ViceTransfer).FullName, args.SortInfo, args.PagingInfo);

            }, ListView).HasLabel("审核记录").HasOrderNo(5);
        }


        /// <summary>
        /// 明细编辑
        /// </summary>

        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseCommand(typeof(SaveTransferCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.ViceTransfers.TransferBehavior");
            View.UseDetail(4);
            View.Property(p => p.TransferNo).Readonly();

            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.ViceAssetObject);
            View.Property(p => p.WarehouseId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add(nameof(e.WarehouseCode), nameof(e.Warehouse.Code));
                m.DicLinkField = keyValuePairs;
            }).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).UseListSetting(p => p.HelpInfo = "切换来源仓库清空需求清单明细").Show().HasLabel("来源仓库");

            View.Property(p => p.TargetWarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.ApplyDate).Show();
            View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 1);

            View.AssociateChildrenProperty(ViceTransfer.ViceTransferSparePartListProperty, e =>
            {
                var args = e as ChildPagingDataArgs;
                var viceTransfer = args.Parent as ViceTransfer;
                if (viceTransfer != null)
                {
                    return RT.Service.Resolve<ViceTransferController>().GetViceTransferSparePartList(viceTransfer.Id);
                }
                return new EntityList<ViceTransferSparePart>();
            }, "EditView").HasLabel("需求清单").Show(ChildShowInWhere.All).HasOrderNo(0);


            View.ChildrenProperty(p => p.ViceTransferSparePartList).HasLabel("需求清单").Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ViceTransferFixtureList).HasLabel("需求清单").Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(ViceTransfer.ViceTransferFixtureListProperty, e =>
                 {
                     var args = e as ChildPagingDataArgs;
                     var viceTransfer = args.Parent as ViceTransfer;
                     if (viceTransfer != null)
                     {
                         return RT.Service.Resolve<ViceTransferController>().GetViceTransferFixtureList(viceTransfer.Id);
                     }
                     return new EntityList<ViceTransferFixture>();
                 }, "EditView").HasLabel("需求清单").Show(ChildShowInWhere.All).HasOrderNo(1);
            View.ChildrenProperty(p => p.ViceTransferAttachmentList).HasLabel("附件");

        }
    }
}
