using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.EMS.InventoryPlans.Commands;
using SIE.Web.EMS.InventoryTasks.Commands;
using SIE.Web.Equipments.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务设备清单界面
    /// </summary>
    internal class InventoryTaskEquipmentViewConfig : WebViewConfig<InventoryTaskEquipment>
    {
        /// <summary>
        /// 盘点平账界面
        /// </summary>
        public const string BalanceView = "BalanceView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BalanceView);
            if (ViewGroup == BalanceView)
            {
                ConfigBalanceView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.InvTaskEquipBehavior");
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection,
                   typeof(ImportTaskEquipCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand",
                   "SIE.Web.EMS.InventoryTasks.Commands.AddProfitCommand", typeof(DeleteProfitCommand).FullName,
                  "SIE.Web.EMS.InventoryTasks.Commands.UploadPhotoCommand", typeof(SeePhotoCommand).FullName);
            View.Property(p => p.InventoryStatus).ShowInList(80).Readonly().HasLabel("状态");
            View.Property(p => p.EquipmentCode).Readonly();
            View.Property(p => p.EquipmentName).Readonly();

            //主表状态为【盘点中】才能编辑、来源为【盘盈新增】时不可编辑、用户在盘点人中有初盘权限才可编辑
            View.Property(p => p.FirstInventoryResult).Readonly(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing || p.InventoryAssetSource == InventoryAssetSource.Profit
                || p.FirstPower == false).ShowInList(80).HasLabel("初盘结果".L10N()+"*");

            //主表状态为【初盘完成、复盘中】才能编辑、来源为【盘盈新增】且初盘结果没有值时不可编辑、用户在盘点人中有复盘权限才可编辑
            View.Property(p => p.SecondInventoryResult)
                .Readonly(p => (p.InventoryTaskStatus != InventoryTaskStatus.FirstDone && p.InventoryTaskStatus != InventoryTaskStatus.ScondDoing) ||
                (p.InventoryAssetSource == InventoryAssetSource.Profit && p.FirstInventoryResult == null) || p.SecondPower == false)
                .ShowInList(80);
            View.Property(p => p.SuggestProcessMethod).Readonly(p => p.FirstInventoryResult == null && p.SecondInventoryResult == null);

            //主表状态为【盘点中、初盘完成、复盘中】才能编辑
            //盘点结果优先取复盘结果，复盘结果为空时取初盘结果：当盘点结果为【正常、盘亏】时，不可编辑、当盘点结果为【信息变动、盘盈】时，可编辑
            Expression<Func<InventoryTaskEquipment, bool>> realExp = p =>
            p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Completed ||
            p.SecondInventoryResult == InventoryResult.Normal || p.SecondInventoryResult == InventoryResult.Loss ||
            (p.SecondInventoryResult == null && (p.FirstInventoryResult == InventoryResult.Normal || p.FirstInventoryResult == InventoryResult.Loss));

            using (View.DeclareBand("实盘".L10N()))
            {
                View.Property(p => p.RealManageDeptId).Readonly(realExp).UseFactoryDepartmentsEditor();
                View.Property(p => p.RealUseDeptId).Readonly(realExp).UseFactoryDepartmentsEditor();
                View.Property(p => p.AccountUseState).Readonly(realExp);
                View.Property(p => p.AccountState).Readonly(realExp);
                View.Property(p => p.UserId).Readonly(realExp);
                View.Property(p => p.RealWorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as InventoryTaskEquipment;
                    if (entity == null)
                    {
                        return new EntityList<Enterprise>();
                    }
                    var workshop = RT.Service.Resolve<EnterpriseController>().GetWorkShops(pagingInfo, keyword, entity.FactoryId);
                    if (workshop == null || workshop.Count <= 0)
                    {
                        return new EntityList<Enterprise>();
                    }
                    workshop.ForEach(p => p.TreePId = null);
                    return workshop;
                }).Cascade(p => p.RealResourceId, null).Readonly(realExp);

                //在实盘字段只读上加上车间为空时不可编辑
                View.Property(p => p.RealResourceId).UseDataSource((e, c, r) =>
                {
                    var entity = e as InventoryTaskEquipment;
                    if (entity == null || entity.RealWorkShopId == null)
                        return new EntityList<Enterprise>();
                    var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, entity.RealWorkShopId.Value);
                    resourcesList.ForEach(p => p.TreePId = null);
                    return resourcesList;
                }).Readonly(p => p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Completed
                || p.SecondInventoryResult == InventoryResult.Normal || p.SecondInventoryResult == InventoryResult.Loss
                || p.RealWorkShopId == null ||
                (p.SecondInventoryResult == null && (p.FirstInventoryResult == InventoryResult.Normal || p.FirstInventoryResult == InventoryResult.Loss)));

                View.Property(p => p.RealWarehouseId).Cascade(p => p.StorageLocationId, null).Readonly(realExp);

                //在实盘字段只读上加上仓库为空时不能编辑
                View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
                {
                    var model = o as InventoryTaskEquipment;
                    if (model == null || model.RealWarehouseId == null)
                    {
                        return new EntityList<StorageLocation>();
                    }
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(null, model.RealWarehouseId, r, e);
                }).Readonly(p => p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Completed
                || p.SecondInventoryResult == InventoryResult.Normal || p.SecondInventoryResult == InventoryResult.Loss
                || p.RealWarehouseId == null ||
                (p.SecondInventoryResult == null && (p.FirstInventoryResult == InventoryResult.Normal || p.FirstInventoryResult == InventoryResult.Loss)));
                View.Property(p => p.RealLocation).ShowInList(150).Readonly(realExp);
            }
            using (View.DeclareBand("原始".L10N()))
            {
                View.Property(p => p.OldManageDept).Readonly();
                View.Property(p => p.OldUseDeptName).ShowInList(100).Readonly();
                View.Property(p => p.OldAccountUseState).Readonly();
                View.Property(p => p.OldAccountState).Readonly();
                View.Property(p => p.OldUserName).ShowInList(100).Readonly();
                View.Property(p => p.OldWorkShopName).ShowInList(100).Readonly();
                View.Property(p => p.OldResourceName).ShowInList(100).Readonly();
                View.Property(p => p.OldWarehouseCode).ShowInList(100).Readonly();
                View.Property(p => p.OldStorageLocationCode).ShowInList(100).Readonly();
                View.Property(p => p.OldLocation).Readonly().ShowInList(150);
            }
            View.Property(p => p.TypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType;c.ReloadDataOnPopping = true; }).Readonly();
            View.Property(p => p.EquipTypeId).Readonly();
            View.Property(p => p.EquipModelId).Readonly();
            View.Property(p => p.EquipModelName).Readonly();
            View.Property(p => p.ModelSpecifications).Readonly();
            View.Property(p => p.FixedAssetsAccountCode).Readonly();
            View.Property(p => p.FixedAssetsAccountName).Readonly();
            View.Property(p => p.PhotoFilePath).Readonly();
            View.Property(p => p.InventoryAssetSource).HasLabel("来源").Readonly();
            View.Property(p => p.FirstCounterId).Readonly();
            View.Property(p => p.InventoryDateTime).ShowInList(150).Readonly();
            View.Property(p => p.SecondCounterId).Readonly();
            View.Property(p => p.SecondDateTime).ShowInList(150).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.InventoryTask.TaskNo).HasLabel("盘点任务单号");
            View.Property(p => p.EquipmentCode);
            View.Property(p => p.EquipmentName);
            View.Property(p => p.ImportResult);
            View.Property(p => p.SuggestProcessMethod);
            View.PropertyRef(p => p.RealManageDept.Code).HasLabel("实盘管理部门");
            View.PropertyRef(p => p.RealUseDept.Code).HasLabel("实盘使用部门");
            View.Property(p => p.AccountUseState).HasLabel("实盘管理状态");
            View.Property(p => p.AccountState).HasLabel("实盘设备状态");
            View.PropertyRef(p => p.User.Code).HasLabel("实盘使用责任人");
            View.PropertyRef(p => p.RealWorkShop.Code).HasLabel("实盘车间");
            View.PropertyRef(p => p.RealResource.Code).HasLabel("实盘产线");
            View.PropertyRef(p => p.RealWarehouse.Code).HasLabel("实盘仓库");
            View.PropertyRef(p => p.StorageLocationCode).HasLabel("实盘库位");
            View.Property(p => p.RealLocation).HasLabel("实盘位置");
            View.Property(p => p.TypeCategory).ImportCatalogType(EquipType.EquipTypeCatalogType, Catalog.NameProperty.Name);
            View.PropertyRef(p => p.EquipType.TypeCode).HasLabel("设备类型");
            View.PropertyRef(p => p.EquipModel.Code).HasLabel("设备型号");
        }

        /// <summary>
        /// 盘点平账界面
        /// </summary> 
        protected void ConfigBalanceView()
        {
            View.AssignAuthorize(typeof(InventoryBalance));
            using (View.OrderProperties())
            {
                //设备台账ID为空的数据可编辑
                View.Property(p => p.EquipmentCode).ShowInList().Readonly(p => p.EquipAccountId > 0);
                View.Property(p => p.EquipmentName).ShowInList().Readonly();
                View.Property(p => p.FirstInventoryResult).Readonly().ShowInList(80);
                View.Property(p => p.SecondInventoryResult).Readonly().ShowInList(80);
                View.Property(p => p.SuggestProcessMethod).Readonly().ShowInList();
                View.Property(p => p.InventoryProcessMethod).ShowInList()
                    .Readonly(p => p.ApprovalStatus != ApprovalStatus.Reject
                        && p.ApprovalStatus != ApprovalStatus.Draft).HasLabel("平账方式".L10N()+"*");
                using (View.DeclareBand("实盘".L10N()))
                {
                    View.Property(p => p.RealManageDeptId).ShowInList().Readonly();
                    View.Property(p => p.RealUseDeptId).ShowInList().Readonly();
                    View.Property(p => p.AccountUseState).ShowInList().Readonly();
                    View.Property(p => p.AccountState).ShowInList().Readonly();
                    View.Property(p => p.UserId).ShowInList().Readonly();
                    View.Property(p => p.RealWorkShopId).ShowInList().Readonly();
                    View.Property(p => p.RealResourceId).ShowInList().Readonly();
                    View.Property(p => p.RealWarehouseId).ShowInList().Readonly();
                    View.Property(p => p.StorageLocationId).ShowInList().Readonly();
                    View.Property(p => p.RealLocation).ShowInList(150).Readonly();
                }
                using (View.DeclareBand("原始".L10N()))
                {
                    View.Property(p => p.OldManageDept).ShowInList().Readonly();
                    View.Property(p => p.OldUseDeptName).ShowInList(100).Readonly();
                    View.Property(p => p.OldAccountUseState).ShowInList().Readonly();
                    View.Property(p => p.OldAccountState).ShowInList().Readonly();
                    View.Property(p => p.OldUserName).ShowInList(100).Readonly();
                    View.Property(p => p.OldWorkShopName).ShowInList(100).Readonly();
                    View.Property(p => p.OldResourceName).ShowInList(100).Readonly();
                    View.Property(p => p.OldWarehouseCode).ShowInList(100).Readonly();
                    View.Property(p => p.OldStorageLocationCode).ShowInList(100).Readonly();
                    View.Property(p => p.OldLocation).Readonly().ShowInList(150);
                }
                View.Property(p => p.TypeCategory).ShowInList().UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.ReloadDataOnPopping = true; }).Readonly();
                View.Property(p => p.EquipTypeId).ShowInList().Readonly();
                View.Property(p => p.EquipModelId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.TypeCategory), nameof(e.EquipModel.TypeCategory)); 
                    keyValues.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
                    keyValues.Add(nameof(e.EquipTypeId) + "_Display", nameof(e.EquipModel.TypeCode));
                    keyValues.Add(nameof(e.EquipTypeId), nameof(e.EquipModel.EquipTypeId));
                    keyValues.Add(nameof(e.ModelSpecifications), nameof(e.EquipModel.Specifications));
                    m.DicLinkField = keyValues;
                }).ShowInList().Readonly(p => p.InventoryProcessMethod != InventoryProcessMethod.NewCard);
                View.Property(p => p.EquipModelName).ShowInList().Readonly();
                View.Property(p => p.ModelSpecifications).ShowInList().Readonly();
                View.Property(p => p.FixedAssetsAccountCode).ShowInList().Readonly();
                View.Property(p => p.FixedAssetsAccountName).ShowInList().Readonly();
                View.Property(p => p.PhotoFilePath).ShowInList().Readonly();
                View.Property(p => p.InventoryAssetSource).ShowInList().HasLabel("来源").Readonly();
                View.Property(p => p.FirstCounterId).ShowInList().Readonly();
                View.Property(p => p.InventoryDateTime).ShowInList(150).Readonly();
                View.Property(p => p.SecondCounterId).ShowInList().Readonly();
                View.Property(p => p.SecondDateTime).ShowInList(150).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
