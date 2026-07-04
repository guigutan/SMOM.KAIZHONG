using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders;
using SIE.Fixtures;
using SIE.Fixtures.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Repairs;
using SIE.Warehouses;
using SIE.Web.Fixtures._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Repairs
{
    /// <summary>
    /// 治具异常详情视图配置
    /// </summary>
    public class FixtureRepairDetailViewConfig : WebViewConfig<FixtureRepairDetail>
    {
        /// <summary>
        /// 自定义添加视图
        /// </summary>
        public const string AddFixtureRepairDetail = "AddFixtureRepairDetail";

        /// <summary>
        /// 自定义异常/维修详情视图
        /// </summary>
        public const string FixtureRepairDetail = "FixtureRepairDetail";

        /// <summary>
        /// 自定义工治具台帐维修履历视图
        /// </summary>
        public const string AccountRepairView = "AccountRepairView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddFixtureRepairDetail, FixtureRepairDetail, AccountRepairView);
            if (ViewGroup == AddFixtureRepairDetail)
            {
                AddFixtureRepairDetailView();
            }

            if (ViewGroup == FixtureRepairDetail)
            {
                FixtureRepairDetailView();
            }

            if (ViewGroup == AccountRepairView)
            {
                ConfigAccountRepairView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.FixtureAccountCode).Readonly();
            View.Property(p => p.FixtureEncodeCode).Readonly();
            View.Property(p => p.FixtureModelCode).Readonly();
            View.Property(p => p.FixtureModelName).Readonly();
            View.Property(p => p.FixtureModelType).Readonly();
            View.Property(p => p.AbnormalCode).Readonly();
            View.Property(p => p.AbnormalDescription).Readonly();
            View.Property(p => p.RepairBeforeState).UseEnumEditor().Readonly();
            View.Property(p => p.FixtureWarehouseId);
            View.Property(p => p.FixtureStorageLocationId).UsePagingLookUpEditor().Readonly();
            View.Property(p => p.LocationName).Readonly();
            View.Property(p => p.WorkOrderId);
            View.Property(p => p.RepairBeforeQualityStatus).UseEnumEditor().Readonly();
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.Content).Readonly();
            View.Property(p => p.Part).Readonly();
            View.Property(p => p.InspectionResult).HasLabel("维修结果").Readonly();
            View.Property(p => p.RepairWhereaboutStatus).UseEnumEditor().Readonly();
            View.Property(p => p.InWarehouseId).Readonly();
            View.Property(p => p.Remark).Readonly();
            View.Property(p => p.FaultCode).Readonly();
            View.Property(p => p.FaultDescription).Readonly();
            View.ChildrenProperty(p => p.Attachments).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.SpareList).Show(ChildShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 工治具报修-添加页面-【异常/维修详情】视图配置
        /// </summary>
        protected virtual void AddFixtureRepairDetailView()
        {
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.UseChildrenAsHorizontal();
                View.UseLayoutSize(0.7, 0.3);

                View.UseCommands("SIE.Web.Fixtures.Repairs.Commands.AddRepairDetailCommand", "SIE.Web.Fixtures.Repairs.Commands.EditRepairDetailCommand", "SIE.Web.Fixtures.Repairs.Commands.DeleteRepairDetailCommand");
                View.Property(p => p.FixtureAccountId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureAccounts(c, r);
                }).HasLabel("工治具").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.FixtureAccountCode), nameof(e.FixtureAccount.Code));
                    keyValues.Add(nameof(e.FixtureEncodeCode), nameof(e.FixtureAccount.EncodeCode));
                    keyValues.Add(nameof(e.FixtureModelCode), nameof(e.FixtureAccount.ModelCode));
                    keyValues.Add(nameof(e.FixtureModelName), nameof(e.FixtureAccount.ModelName));
                    keyValues.Add(nameof(e.FixtureModelType), nameof(e.FixtureAccount.FixtureTypeCode));
                    keyValues.Add(nameof(e.RepairBeforeQualityStatus), nameof(e.FixtureAccount.QualityState));
                    keyValues.Add(nameof(e.ManageMode), nameof(e.FixtureAccount.ManageMode));
                    m.DicLinkField = keyValues;
                }).Show(ShowInWhere.All).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
                View.Property(p => p.ManageMode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureEncodeCode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureModelCode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureModelName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureModelType).Readonly().Show(ShowInWhere.All);


                View.Property(p => p.RepairBeforeState).UseEnumEditor().Readonly(p => p.PersistenceStatus != PersistenceStatus.New || p.ManageMode == ManageMode.Number).Cascade(p => p.FixtureWarehouseId, null).Cascade(p => p.FixtureStorageLocationId, null).Cascade(p => p.LocationName, null).Cascade(p => p.WorkOrderId, null).Show(ShowInWhere.All);

                View.Property(p => p.FixtureWarehouseId).UseDataSource((e, c, r) =>
                {
                    var repairDetail = e as FixtureRepairDetail;
                    if (repairDetail == null || repairDetail.FixtureAccount == null || repairDetail.RepairBeforeState == RepairBeforeState.Online)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<CoreFixtureController>().GetWarehouses(repairDetail.FixtureAccountId, c, r);
                }).Cascade(p => p.FixtureStorageLocationId, null).Cascade(p=>p.LocationName,null).Readonly(p => p.ManageMode == ManageMode.Number || p.RepairBeforeState != RepairBeforeState.InStock).Show(ShowInWhere.All);

                View.Property(p => p.FixtureStorageLocationId).UseDataSource((e, c, r) =>
                {
                    var repairDetail = e as FixtureRepairDetail;
                    if (repairDetail == null || repairDetail.FixtureAccount == null || repairDetail.FixtureWarehouseId == null || repairDetail.RepairBeforeState == RepairBeforeState.Online)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<CoreFixtureController>().GetStorageLocations(repairDetail.FixtureAccountId, repairDetail.FixtureWarehouseId.Value, c, r);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    m.ReloadDataOnPopping = true;
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.LocationName), nameof(e.StorageLocation.Name));
                    m.DicLinkField = keyValues;
                }).Show(ShowInWhere.All).HasLabel("库位编码").Readonly(p => p.FixtureAccountCode == "" || p.RepairBeforeState == RepairBeforeState.Online || p.PersistenceStatus != PersistenceStatus.New || p.ManageMode == ManageMode.Number);

                View.Property(p => p.LocationName).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.WorkOrderId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(pagingInfo, keyword);
                }).Readonly(p => p.ManageMode == ManageMode.Number || p.RepairBeforeState == RepairBeforeState.InStock).Show(ShowInWhere.All);
                View.Property(p => p.RepairBeforeQualityStatus).UseEnumEditor().Readonly(p => p.ManageMode == ManageMode.Number || (p.ManageMode == ManageMode.Code && p.RepairBeforeState == RepairBeforeState.Online)).Show(ShowInWhere.All);

                View.Property(p => p.Qty).UseSpinEditor(p =>
                {
                    p.AllowDecimals = false;
                    p.MinValue = 0;
                }).Show(ShowInWhere.All).Readonly(p => p.ManageMode == ManageMode.Number || p.PersistenceStatus != PersistenceStatus.New);


                View.Property(p => p.AbnormalId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetFixtureAbnormalList(pagingInfo, keyword);
                }).Show(ShowInWhere.All).UseAbnormalTypeUnusualEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AbnormalCode), nameof(e.Abnormal.Code));
                    keyValues.Add(nameof(e.AbnormalDescription), nameof(e.Abnormal.Description));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.PersistenceStatus != PersistenceStatus.New).HasLabel("异常现象编码");
                View.Property(p => p.AbnormalDescription).Readonly().Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.Attachments).UseViewGroup(FixtureRepairAttachmentViewConfig.AddFixtureRepairDetail).Show(ChildShowInWhere.All).HasLabel("图片");
                View.ChildrenProperty(p => p.SpareList).Show(ChildShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 工治具报修-维修页面-【异常/维修详情】视图配置
        /// </summary>
        protected virtual void FixtureRepairDetailView()
        {
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.InlineEdit();
                View.UseChildrenAsHorizontal();
                View.UseLayoutSize(0.7, 0.3);
                View.AddBehavior("SIE.Web.Fixtures.Repairs.Script.FixtureRepairDetailBehavior");
                View.UseCommands("SIE.Web.Fixtures.Repairs.Commands.EditDetailCommand");
                View.Property(p => p.FixtureAccountId).UsePagingLookUpEditor().HasLabel("工治具ID").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureEncodeCode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureModelCode).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureModelName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureModelType).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.RepairBeforeState).UseEnumEditor().Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureWarehouseId).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.FixtureStorageLocationId).Readonly().UsePagingLookUpEditor().HasLabel("库位编码").Show(ShowInWhere.All);
                View.Property(p => p.LocationName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderId).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.RepairBeforeQualityStatus).UseEnumEditor().Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Qty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.AbnormalId).UsePagingLookUpEditor().HasLabel("异常现象编码").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.AbnormalDescription).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Content).Show(ShowInWhere.All).Readonly(p => p.Result != null);
                View.Property(p => p.Part).Show(ShowInWhere.All).Readonly(p => p.Result != null);
                View.Property(p => p.InspectionResult).HasLabel("维修结果").Show(ShowInWhere.All).Readonly(p => p.Result != null);
                View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly(p => p.Result != null);
                View.Property(p => p.FaultId).UseAbnormalTypeFaultEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.FaultCode), nameof(e.Abnormal.Code));
                    keyValues.Add(nameof(e.FaultDescription), nameof(e.Abnormal.Description));
                    m.DicLinkField = keyValues;
                }).HasLabel("故障类型编码").Show(ShowInWhere.All).Readonly(p => p.Result != null);
                View.Property(p => p.RepairWhereaboutStatus).Cascade(p => p.InWarehouseId, null).UseEnumEditor().Readonly(p => p.RepairBeforeState != RepairBeforeState.Online || p.InspectionResult != SIE.Common.InspectionResult.Pass).Show(ShowInWhere.All);
                View.Property(p => p.InWarehouseId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<CoreFixtureController>().GetConfigWarehouses(c, r);
                }).Readonly(p => p.RepairWhereaboutStatus == RepairWhereabout.Use).Show(ShowInWhere.All);
                View.Property(p => p.FaultDescription).Readonly().Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.Attachments).UseViewGroup(FixtureRepairAttachmentViewConfig.FixtureRepairDetail).Show(ChildShowInWhere.All).HasLabel("图片");
                View.ChildrenProperty(p => p.SpareList).UseViewGroup(FixtureRepairRecordViewConfig.FixtureRepairDetail).HasLabel("维修记录").Show(ChildShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置工治具台帐维修履历视图
        /// </summary>
        void ConfigAccountRepairView()
        {
            View.DomainName("维修履历");

            View.AssignAuthorize(typeof(FixtureAccountModel));

            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.Property(p => p.RepairNo).Readonly().ShowInList(150);
                View.Property(p => p.RepairApplyBy).Readonly().ShowInList();
                View.Property(p => p.RepairApplyDate).Readonly().ShowInList(150);
                View.Property(p => p.RepairByName).Readonly().ShowInList();
                View.Property(p => p.RepairDate).Readonly().ShowInList(150);
                View.Property(p => p.AbnormalDescription).Readonly().ShowInList();
                View.Property(p => p.Content).Readonly().ShowInList();
                View.Property(p => p.Part).Readonly().ShowInList();
                View.Property(p => p.InspectionResult).UseEnumEditor().Readonly().ShowInList();
                View.Property(p => p.Remark).Readonly().ShowInList();
                View.Property(p => p.FaultDescription).Readonly().ShowInList();
                View.ChildrenProperty(p => p.Attachments).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.SpareList).Show(ChildShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}