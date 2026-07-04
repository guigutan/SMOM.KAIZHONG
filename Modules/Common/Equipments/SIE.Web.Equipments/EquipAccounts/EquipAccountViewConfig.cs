using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.Equipments.DeviceIOTParas.Details;
using SIE.Web.Equipments.EquipAccounts.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipAccountViewConfig : WebViewConfig<EquipAccount>
    {
        /// <summary>
        /// 修改视图
        /// </summary>
        public const string EditViewGroup = "EditViewGroup";

        /// <summary>
        /// 字体显示宽度
        /// </summary>
        private const int charDisplayWidth = 20;

        /// <summary>
        /// 电子行业基础数据
        /// </summary>
        public const string EISBaseDataViewGroup = "EISBaseDataViewGroup";

        /// <summary>
        /// 电子行业基础数据ADD
        /// </summary>
        public const string EISBaseDataAddViewGroup = "EISBaseDataAddViewGroup";

        /// <summary>
        /// 图片
        /// </summary>
        public const string PhotoViewGroup = "PhotoView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditViewGroup
                , EISBaseDataViewGroup, EISBaseDataAddViewGroup, PhotoViewGroup);

            if (ViewGroup == EditViewGroup)
            {
                this.ConfigDetailsView();
                View.ReplaceCommands(WebCommandNames.FormSave, typeof(EditSaveAccountCommand).FullName);
            }

            if (ViewGroup == EISBaseDataViewGroup)
            {
                View.UseCommand(WebCommandNames.Save);
                EISBaseDataView();
            }

            if (ViewGroup == EISBaseDataAddViewGroup)
            {
                EISBaseDataView();
            }

            if (ViewGroup == PhotoViewGroup)
            {
                PhotoView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DraggableForTree();
            View.AddBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountListViewBehavior");

            View.AssignAuthorize(typeof(EquipAccount));
            View.FormEdit();

            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.AddAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.AddChildAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.EditAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.CopyAccountCommand");

            View.UseCommand(WebCommandNames.Delete);
            View.UseCommands(typeof(UpgradeAccountCommand).FullName, typeof(DowngradeAccountCommand).FullName);

            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.EquipAccountImportCommand");
            
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("设备编码").Readonly().ShowInList(width: charDisplayWidth * 8);
                View.Property(p => p.Name).HasLabel("设备名称").Readonly().ShowInList(width: charDisplayWidth * 8);

                View.Property(p => p.Alias).Readonly().ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.ModelCode).HasLabel("设备型号编码".L10N()+"*").Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.ModelName).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.State).Readonly().ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.UseState).HasLabel("管理状态".L10N() + "*").Readonly().ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.Frozen).Readonly().ShowInList(width: charDisplayWidth * 3);

                View.Property(p => p.EquipTypeCode).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.EquipTypeName).HasLabel("设备类型").Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.EquipTypeCategory)
                    .UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.IndustryCategory).ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.IsVirtual).ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.IsCustomsSupervision).HasLabel("海关监管").ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.EquipmentGrading).ShowInList(width: charDisplayWidth * 4);

                View.Property(p => p.UseLevel)
                    .UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.CatalogReloadData = true; }).HasLabel("ABC分类").ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.FactoryId).UseFactoryEditor().ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.UseDepartmentId).ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.WorkShopId).HasLabel("车间").ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.ResourceId).HasLabel("产线").ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.WarehouseId).HasLabel("仓库").ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.StorageLocationId).HasLabel("库位").ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.AdministratorId).HasLabel("保管人").ShowInList(width: charDisplayWidth * 4);

                View.Property(p => p.ProcessId).HasLabel("工序").ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.ManageDepartmentId).HasLabel("管理部门").ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.Proprietorship).ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.ResPersonId).ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.UserId).ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.PurchaseUnit).ShowInList(width: charDisplayWidth * 10);
                View.Property(p => p.Manufacturer).ShowInList(width: charDisplayWidth * 10);

                View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("供应商编码").ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.SupplierName).Readonly().ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.PurchaseOrderNo).ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.InstallationLocation).ShowInList(width: charDisplayWidth * 10);
                View.Property(p => p.OriginalSerialNumber).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.RFID).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.EnterDate).UseDateEditor().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.CardDate).UseDateEditor().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.UsefulLife).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.WarrantyPeriod).ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.Drawn).Show();
                View.Property(p => p.PurchaseDate).Show();
                View.Property(p => p.CostCenterCode).Show();
                View.Property(p => p.WorkCenterId).Show();
                View.Property(p => p.SerialNumber).Show();
                View.Property(p => p.Acupoint).Show();
                View.Property(p => p.FunctionalLocation).Show();
            }
            ConfigListViewChildren();

        }

        /// <summary>
        /// 配置子属性
        /// </summary>
        private void ConfigListViewChildren()
        {
            View.ChildrenProperty(p => p.ResumeList).HasLabel("设备履历").HasOrderNo(3);
            View.ChildrenProperty(p => p.ProcessList).HasLabel("工序列表").HasOrderNo(195);
            View.AttachChildrenProperty(typeof(EquipAccountProduct), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccount>();
                if (parent == null)
                    return new EntityList<EquipAccountProduct>();

                var physicalUnion = RT.Service.Resolve<EquipAccountController>()
                    .GetEquipAccountProducts(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return physicalUnion;
            }).HasLabel("产品列表").HasOrderNo(196).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.PcbSlotList).HasLabel("缸槽管理").Show(ChildShowInWhere.Hide).HasOrderNo(198);
            View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(PhysicalUnion), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccount>();
                if (parent == null)
                    return new EntityList<PhysicalUnion>();

                var physicalUnion = RT.Service.Resolve<DeviceIOTParaController>()
                    .GetPhysicalUnions(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return physicalUnion;
            }, PhysicalUnionViewConfig.AccountPhysicalUnionViewGroup).HasLabel("设备参数").HasOrderNo(3);
            View.AttachDetailChildrenProperty(typeof(EquipAccount), (c) =>
            {
                var account = c.Parent as EquipAccount;
                account = RF.GetById<EquipAccount>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, EISBaseDataViewGroup).HasLabel("电子行业").HasOrderNo(196);
            View.ChildrenProperty(p => p.EquipAccountLocationList).HasLabel("位置列表").HasOrderNo(197);

            View.AttachChildrenProperty(typeof(EquipAccountSlot), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccount>();
                if (parent == null)
                    return new EntityList<EquipAccountSlot>();

                var accountSlos = RT.Service.Resolve<EquipAccountSloController>()
                    .GetEquipAccountSlots(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return accountSlos;
            }, EquipAccountSloViewConfig.EquipAccountSloListViewGroup).HasLabel("缸槽管理").HasOrderNo(198);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件资料").HasOrderNo(200).Show(ChildShowInWhere.All);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.AddBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.AddEquipAccountBehavior");            
            View.UseCommands(typeof(EditSaveAccountCommand).FullName);
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipModelId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipAccountController>().GetEquipModelLoadAll(pagingInfo, keyword);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ModelName), nameof(e.EquipModel.Name));
                    keyValues.Add(nameof(e.EquipTypeCode), nameof(e.EquipModel.TypeCode));
                    keyValues.Add(nameof(e.EquipTypeName), nameof(e.EquipModel.TypeName));
                    keyValues.Add(nameof(e.EquipTypeCategory), nameof(e.EquipModel.TypeCategory));
                    keyValues.Add(nameof(e.IndustryCategory), nameof(e.EquipModel.IndustryCategory));
                    m.DicLinkField = keyValues;
                }).HasLabel("设备型号编码").Show();
                View.Property(p => p.ModelName).Readonly().Show();
                View.Property(p => p.EquipTypeCode).Readonly().Show();
                View.Property(p => p.EquipTypeName).Readonly().Show();
                View.Property(p => p.Code).Show().Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
                View.Property(p => p.Name).UseTextEditor(p => p.AllowBlank = false).HasLabel("设备名称").Show();
                View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).Readonly().Show();

                View.Property(p => p.Alias).Show();
                View.Property(p => p.OriginalSerialNumber).Show();
                View.Property(p => p.RFID).Show();
                View.Property(p => p.State).Readonly().Show();
                View.Property(p => p.IndustryCategory).Readonly().Show();
                View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.CatalogReloadData = true; }).Show();
                View.Property(p => p.FactoryId).UseFactoryEditor().Show();
                View.Property(p => p.ManageDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as SIE.Equipments.EquipAccounts.EquipAccount;
                    return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, pagingInfo, keyword);
                }).UsePagingLookUpEditor().HasLabel("管理部门".L10N() + "*").Show();

                View.Property(p => p.UseDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as SIE.Equipments.EquipAccounts.EquipAccount;
                    return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, pagingInfo, keyword);
                }).UsePagingLookUpEditor().Cascade(p => p.WorkShopId, null).HasLabel("使用部门".L10N() + "*").Show();
                View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var equipAccount = source as EquipAccount;
                    return RT.Service.Resolve<EnterpriseController>().GetDepartmentWorkShops(equipAccount.FactoryId, pagingInfo, keyword);
                }).UsePagingLookUpEditor().Show().Cascade(p => p.ResourceId, null);
                View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
                {
                    var equipAccount = e as EquipAccount;
                    if (equipAccount == null || equipAccount.WorkShop == null)
                    {
                        return new EntityList<Enterprise>();
                    }
                    var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, equipAccount.WorkShopId.Value);
                    resourcesList.ForEach(p => p.TreePId = null);
                    return resourcesList;
                }).UsePagingLookUpEditor().Show().Readonly(p => p.WorkShopId == null);
                View.Property(p => p.Process).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetProcessList(p, k);
                }).Show();
                
                View.Property(p => p.UserId).UseDataSource((s, p, k) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(p, k);
                }).Show();
                View.Property(p => p.Proprietorship).Show().HasLabel("资产来源");
                View.Property(p => p.AssetCode).Show().HasLabel("固定资产编码").Readonly();
                View.Property(p => p.ResPersonId).UseDataSource((s, p, k) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(p, k);
                }).Show();

                View.Property(p => p.PurchaseUnit).Show();
                View.Property(p => p.EnterDate).UseDateEditor().Show();
                View.Property(p => p.Manufacturer).Show();
                View.Property(p => p.SupplierId).UseDataSource((s, p, k) =>
                {
                    return RT.Service.Resolve<SupplierController>().GetSuppliers(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("供应商编码").Show();
                View.Property(p => p.SupplierName).Readonly().Show();
                View.Property(p => p.UsefulLife).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).Show();
                View.Property(p => p.WarrantyPeriod).UseDateEditor().Show();
                View.Property(p => p.PurchaseOrderNo).Readonly().Show();
                View.Property(p => p.EquipmentGrading).Readonly().Show();
                View.Property(p => p.IsVirtual).Show();
                View.Property(p => p.IsCustomsSupervision).Show();
                View.Property(p => p.InstallationLocation).Show();
                View.Property(p => p.UseState).Show();
                View.Property(p => p.Drawn).Show();
                View.Property(p => p.PurchaseDate).Show();
                View.Property(p => p.CostCenterCode).Show();
                View.Property(p => p.WorkCenterId).Show();
                View.Property(p => p.SerialNumber).Show();
                View.Property(p => p.Acupoint).Show();
                View.Property(p => p.FunctionalLocation).Show();

                //多的字段
                ConfigDetailsProperty();
            }
            ConfigDetailsViewChildren();
        }

        /// <summary>
        /// 配置字段属性
        /// </summary>
        private void ConfigDetailsProperty()
        {
            View.Property(p => p.WarehouseId).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetAvailableWarehouses(p, k);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.WarehouseName), nameof(e.Warehouse.Name));
                m.DicLinkField = keyValues;
            }).Cascade(p => p.StorageLocationId, null).Cascade(p => p.StorageLocationName, null).HasLabel("仓库编码").Show();
            View.Property(p => p.WarehouseName).Readonly().HasLabel("仓库名称").Show();
            View.Property(p => p.StorageLocationId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.StorageLocationName), nameof(e.StorageLocation.Name));
                m.DicLinkField = keyValues;
            }).UseDataSource((o, e, r) =>
            {
                var model = o as EquipAccount;
                if (model == null || model.WarehouseId == null)
                {
                    return new EntityList<StorageLocation>();
                }
                return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas(model.WarehouseId.Value, r, e);
            }).HasLabel("库位编码").Show();
            View.Property(p => p.StorageLocationName).Readonly().HasLabel("库位名称").Show();
            View.Property(p => p.Downgrade).Show().Visibility(p => p.IsCalibration);

            View.Property(p => p.PrecisionClass).UseCatalogEditor(e => { e.CatalogType = EquipAccount.PrecisionClassType; e.CatalogReloadData = true; }).UseListSetting(e => { e.HelpInfo = "精度级别类型(PRECISION_CLASS_TYPE)"; }).Show().Visibility(p => p.IsCalibration);

            View.Property(p => p.InspectionDate).UseDateEditor().Show().Visibility(p => p.IsCalibration);

        }

        /// <summary>
        /// 配置子视图属性
        /// </summary>
        private void ConfigDetailsViewChildren()
        {
            View.ChildrenProperty(p => p.ResumeList).HasLabel("设备履历").Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件资料").HasOrderNo(89).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.EquipAccountLocationList).HasLabel("位置列表").HasOrderNo(90).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.ProcessList).HasLabel("工序列表").HasOrderNo(91).Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(EquipAccountProduct), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccount>();
                if (parent == null)
                    return new EntityList<EquipAccountProduct>();

                var physicalUnion = RT.Service.Resolve<EquipAccountController>()
                    .GetEquipAccountProducts(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return physicalUnion;
            }).HasLabel("产品列表").HasOrderNo(92).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);

            View.AttachDetailChildrenProperty(typeof(EquipAccount), (c) =>
            {
                var account = c.Parent as EquipAccount;
                account = RF.GetById<EquipAccount>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, EISBaseDataAddViewGroup).HasLabel("电子行业").HasOrderNo(100).Show(ChildShowInWhere.All);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.DraggableForTree();
            View.Property(p => p.Code).ShowInList(width: 100).Readonly();
            View.Property(p => p.Name).ShowInList(width: 100).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.UseState).Readonly();
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.SupplierId).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.EquipTypeCode).Readonly();
            View.Property(p => p.EquipTypeName).Readonly();
            View.Property(p => p.EquipTypeCategory).Readonly();
            View.Property(p => p.WorkShopId).HasLabel("车间");
            View.Property(p => p.ProcessId).HasLabel("工序");
            View.Property(p => p.InstallationLocation);
        }

        /// <summary>
        /// 电子行业基础数据
        /// </summary>
        protected void EISBaseDataView()
        {
            View.RemoveCommands(ConfigCommands.ModuleConfigEditCommand);
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.RailType).Show();
                View.Property(p => p.VirtualDevice).Show();
                View.Property(p => p.FeederBinding).Show();
                View.Property(p => p.FeederLocFailSafe).Show();
                View.Property(p => p.FeederBarcodeFailSafe).Show();
                View.Property(p => p.IsDisabled).Show();
                View.Property(p => p.AgingType).Show();
                View.Property(p => p.ProductionType).Show();
            }
        }

        /// <summary>
        /// 图片
        /// </summary>
        protected void PhotoView()
        {
            View.RemoveCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Photo).UseCustomEditor(p => p.XType = "equipAccountPictureEditor").ShowInDetail();
            }
        }
    }
}
