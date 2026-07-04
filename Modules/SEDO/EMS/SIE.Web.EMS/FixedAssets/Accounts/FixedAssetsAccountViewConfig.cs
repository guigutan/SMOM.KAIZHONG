using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipTypes;
using SIE.EMS.FixedAssets.Accounts;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using SIE.Web.EMS.FixedAssets.Accounts.Commands;
using SIE.Web.Resources;
using System.Collections.Generic;
using SIE.Web.Common.Configs.Commands;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Resources.Employees;

namespace SIE.Web.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 固定资产台账视图配置
    /// </summary>
    internal class FixedAssetsAccountViewConfig : WebViewConfig<FixedAssetsAccount>
    {
        /// <summary>
        /// 一个中文默认显示宽度
        /// </summary>
        private const int clounmWidth = 20;

        private const string percent = "0,000.00";

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.DisableEditing();
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.Common.Script.ApprovalBehavior"); 
            View.AddBehavior("SIE.Web.EMS.FixedAssets.Accounts.Scripts.ApprovalBehavior");
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddFixedAssetAccountCommand).FullName);
            View.RemoveCommands(WebCommandNames.Copy);
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.EMS.FixedAssets.Accounts.Commands.EditFixedAssetAccountCommand");
            View.ReplaceCommands(WebCommandNames.Delete, typeof(DeleteFixedAssetAccountCommand).FullName);
            View.RemoveCommands(WebCommandNames.Save);
            View.UseCommands(typeof(SubmitCommand).FullName, typeof(WithdrawCommand).FullName, "SIE.Web.EMS.FixedAssets.Accounts.Commands.ApproveCommand");
            View.Property(p => p.Code).ShowInList(width: 8 * clounmWidth);
            View.Property(p => p.Name).ShowInList(width: 8 * clounmWidth);
            View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = SIE.Core.Equipments.EquipType.EquipTypeCatalogType;p.ReloadDataOnPopping = true; }).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.FactoryId).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.MangeDepartmentId).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.ReviewStatus).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.ManageStatus).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.AssetsSource).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.AssetOwnerId).ShowInList(width: 5 * clounmWidth);
            View.Property(p => p.OriginalAssetsValue).ShowInList(width: 5 * clounmWidth).UseSpinEditor(c => c.Format = percent);
            View.Property(p => p.DepreciationYear).ShowInList(width: 5 * clounmWidth);
            View.Property(p => p.ResidualValueRatio).ShowInList(width: 5 * clounmWidth);
            View.Property(p => p.DepreciationResidualValue).UseSpinEditor(c => c.Format = percent).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.NetAssetValue).UseSpinEditor(c => c.Format = percent).ShowInList(width: 5 * clounmWidth);
            View.Property(p => p.ContractCode).ShowInList(width: 6 * clounmWidth);
            View.Property(p => p.PurchaseUnitPrice).UseSpinEditor(c => c.Format = percent).ShowInList(width: 6 * clounmWidth);
            View.Property(p => p.CostCenter).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.FixedAssetsTransferDate).ShowInList(width: 8 * clounmWidth);
            View.Property(p => p.AssetsType).ShowInList(width: 4 * clounmWidth);
            View.Property(p => p.AssetsClass).ShowInList(width: 3 * clounmWidth);
            View.Property(p => p.FinancialCategory).ShowInList(width: 5 * clounmWidth);

            View.ChildrenProperty(p => p.DeviceBillList).HasOrderNo(1);
            View.ChildrenProperty(p => p.FixedAssetSparePartList).HasOrderNo(2);
            View.ChildrenProperty(p => p.FixedAssetFixtureBillList).HasOrderNo(3);
            View.ChildrenProperty(p => p.ResumeList).HasOrderNo(4).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(FixedAssetResume), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<FixedAssetsAccount>();
                if (parent == null)
                    return new EntityList<FixedAssetResume>();

                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var state = (ResumeType?)parent.ResumeState;
                var sesumes = RT.Service.Resolve<FixedAssetsAccountController>()
                    .GetResumes(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo, state);
                return sesumes;
            }).HasLabel("资产履历").HasOrderNo(5);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as FixedAssetsAccount;
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                    typeof(FixedAssetsAccount).FullName, args.SortInfo, args.PagingInfo);

            }).HasLabel("审核记录").HasOrderNo(6);
        }


        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(4);
            View.AddBehavior("SIE.Web.EMS.FixedAssets.Accounts.Scripts.AddFixedAssetBehavior");
            View.UseCommand("SIE.Web.EMS.FixedAssets.Accounts.Commands.SaveFixedAccountDetailsCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigCommand);
            View.Property(p => p.AssetsType);
            View.Property(p => p.AssetsCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.CatalogReloadData = true; }); ;
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.Name);
            View.Property(p => p.ManageStatus);

            View.Property(p => p.Factory).HasLabel("工厂").UseFactoryEditor().Show();
            View.Property(p => p.MangeDepartment).HasLabel("管理部门").UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as FixedAssetsAccount;
                return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, pagingInfo, keyword);
            }).UsePagingLookUpEditor();
            View.Property(p => p.AssetsSource);
            View.Property(p => p.AssetOwnerId).HasLabel("资产责任人").UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            });

            View.Property(p => p.OriginalAssetsValue).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.ResidualValueRatio).UseSpinEditor(m =>
            {
                m.MinValue = 0;
                m.DecimalPrecision = 2;
                m.MaxValue = 100;
            }).ShowInList();
            View.Property(p => p.DepreciationResidualValue).UseSpinEditor(m =>
            {
                m.MinValue = 0;
                m.DecimalPrecision = 2;
            }).Readonly();
            View.Property(p => p.NetAssetValue).UseSpinEditor(m =>
            {
                m.MinValue = 0;
                m.DecimalPrecision = 2;
            }).Readonly();
            View.Property(p => p.ContractCode);
            View.Property(p => p.PurchaseUnitPrice).UseSpinEditor(m =>
            {
                m.MinValue = 0;
                m.DecimalPrecision = 2;
            });
            View.Property(p => p.CostCenter);
            View.Property(p => p.FixedAssetsTransferDate).UseDateEditor();

            View.Property(p => p.AssetsClass).Show();

            View.Property(p => p.FinancialCategory).UsePagingLookUpEditor(((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.DepreciationYear), nameof(e.FinancialCategory.Depreciation));
                m.DicLinkField = keyValues;
            }));
            View.Property(p => p.DepreciationYear).UseSpinEditor(m =>
            {
                m.MinValue = 1;
            }).Readonly();

            View.Property(p => p.ReviewStatus).DefaultValue(ApprovalStatus.Draft).Show(ShowInWhere.Hide);

            View.ChildrenProperty(p => p.DeviceBillList).HasOrderNo(1).ViewGroup = "AddDeviceBillViewGroup";
            View.ChildrenProperty(p => p.FixedAssetSparePartList).HasOrderNo(2).ViewGroup = "AddFixedAssetSparePartViewGroup";
            View.ChildrenProperty(p => p.FixedAssetFixtureBillList).HasOrderNo(3).ViewGroup = "AddFixedAssetFixtureViewGroup";
        }
    }
}