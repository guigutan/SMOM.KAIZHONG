using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.Equipments.Models;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.EMS.Equipments.Accounts;
using SIE.Web.EMS.EquipRepair.EquipRepairs;
using SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands;
using SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.ExtensionViewConfig;
using SIE.Web.Equipments.DeviceIOTParas.Details;
using SIE.Web.Equipments.EquipAccounts;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量设备台账视图配置
    /// </summary>
    public class MeteringEquipmentAccountViewConfig : WebViewConfig<MeteringEquipmentAccount>
    {

        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private readonly static int CoulmnWidth = 20;

        /// <summary>
        /// 修改视图
        /// </summary>
        public const string EditViewGroup = "EditViewGroup";

        /// <summary>
        /// 特种设备台账视图
        /// </summary>
        public readonly static string MeteringEquipmentAccountGroup = "MeteringEquipmentAccountView";


        /// <summary>
        /// 电子行业基础数据
        /// </summary>
        public readonly static string EISBaseDataViewGroup = "EISBaseDataViewGroup";

        /// <summary>
        /// 电子行业基础数据ADD
        /// </summary>
        public const string EISBaseDataAddViewGroup = "EISBaseDataAddViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {

            View.DeclareExtendViewGroup(EditViewGroup, MeteringEquipmentAccountGroup, EISBaseDataViewGroup, EISBaseDataAddViewGroup);
            if (ViewGroup == EditViewGroup)
            {
                this.ConfigDetailsView();
            }

            if (ViewGroup == MeteringEquipmentAccountGroup)
            {
                MeteringEquipmentAccountView();
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

        }


        /// <summary>
        /// 计量设备台账
        /// </summary>
        protected void MeteringEquipmentAccountView()
        {
            View.AssignAuthorize(typeof(MeteringEquipmentAccount));
            View.FormEdit();

            View.AddBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountListViewBehavior");

            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.AddAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.AddChildAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.EditAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.CopyAccountCommand");
            View.UseCommand(WebCommandNames.Delete);
            View.UseCommands(typeof(SynMeteringModelCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("设备编码").Readonly().ShowInList(width: CoulmnWidth * 8);
                View.Property(p => p.Name).HasLabel("设备名称").Readonly().ShowInList(width: CoulmnWidth * 8);
                View.Property(p => p.Alias).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ModelCode).HasLabel("设备型号编码".L10N() + "*").Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ModelName).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.State).Readonly().ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.UseState).HasLabel("管理状态".L10N() + "*").Readonly().ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.Frozen).Readonly().ShowInList(width: CoulmnWidth * 3);
                View.Property(p => p.EquipTypeCode).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.EquipTypeName).HasLabel("设备类型").Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.IndustryCategory).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.IsVirtual).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.IsCustomsSupervision).Readonly().HasLabel("海关监管").ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.EquipmentGrading).ShowInList(width: CoulmnWidth * 4);

                View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.CatalogReloadData = true; }).HasLabel("ABC分类").ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.Factory).UseFactoryEditor().ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.UseDepartmentId).ShowInList(width: CoulmnWidth * 6);

                View.Property(p => p.WorkShopId).HasLabel("车间").ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ResourceId).HasLabel("产线").ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ProcessId).HasLabel("工序").ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.ManageDepartmentId).HasLabel("管理部门").ShowInList(width: CoulmnWidth * 6);

                View.Property(p => p.Proprietorship).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.AssetCode).ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ResPersonId).ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.PurchaseUnit).ShowInList(width: CoulmnWidth * 10);
                View.Property(p => p.Manufacturer).ShowInList(width: CoulmnWidth * 10);

                View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("供应商编码").ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.SupplierName).Readonly().ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.PurchaseOrderNo).ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.InstallationLocation).ShowInList(width: CoulmnWidth * 10);
                View.Property(p => p.OriginalSerialNumber).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.RFID).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.EnterDate).UseDateEditor().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.CardDate).UseDateEditor().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.UsefulLife).UseSpinEditor(p => { p.AllowNegative = false; }).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.WarrantyPeriod).ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.NextInspectionDate).ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.RegularInspectionStatus).ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.Downgrade).Readonly().ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.PrecisionClass).UseCatalogEditor(e => { e.CatalogType = CalibrationEquipment.PrecisionClassType; e.CatalogReloadData = true; }).UseListSetting(e => { e.HelpInfo = "精度级别类型(PRECISION_CLASS_TYPE)"; }).ShowInList(width: CoulmnWidth * 5);
            }
            ConfigListViewChildrenOne();
            ConfigListViewChildrenTwo();
        }

        /// <summary>
        /// 计量设备台账增加子页签
        /// </summary>
        protected void ConfigListViewChildrenOne()
        {
            View.ChildrenProperty(p => p.PcbSlotList).HasLabel("缸槽管理").Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipAccountCalibrationList).HasLabel("计量校验规程").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(EquipBomDetail), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<EquipAccountBase>();
                if (parent == null)
                {
                    return new EntityList<EquipBomDetail>();
                }
                var bomDtls = RT.Service.Resolve<EquipBomController>()
                    .GetEquipBomDetailsByModelId(parent.EquipModelId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return bomDtls;
            }, EquipBomDetailExtensionViewConfig.AccountEquipBomDetailViewGroup).HasLabel("设备BOM").HasOrderNo(2);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringTechParameterListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as MeteringEquipmentAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                           .GetEquipModelTechParametersByEquipAccount(model.Id, arg.PagingInfo, null);
                }
                return new EntityList<EquipModelTechParameter>();
            }, EquipModelTechParameterExtensionViewConfig.ReadOnlyView).Show(ChildShowInWhere.All).HasLabel("技术参数").HasOrderNo(3).LazyLoad(false);

            View.AttachChildrenProperty(typeof(PhysicalUnion), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as MeteringEquipmentAccount;
                if (parent == null)
                {
                    return new EntityList<PhysicalUnion>();
                }
                var physicalUnion = RT.Service.Resolve<DeviceIOTParaController>()
                    .GetPhysicalUnions(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return physicalUnion;
            }, PhysicalUnionViewConfig.AccountPhysicalUnionViewGroup).HasLabel("设备参数").HasOrderNo(4);

            View.ChildrenProperty(p => p.ResumeList).Show(ChildShowInWhere.Hide);//隐藏标准产品的
            View.AttachChildrenProperty(typeof(MeterEquipAccountResume), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MeteringEquipmentAccount>();
                if (parent == null)
                {
                    return new EntityList<EquipAccountResume>();
                }
                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var state = (ResumeType?)parent.GetMeteringResumeStateDontMap();
                var sesumes = RT.Service.Resolve<EquipAccountController>().GetEquipAccountResumes(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo, state);
                return sesumes;
            }).HasLabel("设备履历").HasOrderNo(4);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringCheckMgtProperty, e =>
            {
                var account = e.Parent as MeteringEquipmentAccount;
                var checkMgt = new EquipAccountCheckMgt();
                if (account == null)
                {
                    return checkMgt;
                }
                checkMgt.Id = account.Id;
                checkMgt.MarkSaved();
                return checkMgt;
            }, ViewConfig.DetailsView).HasLabel("点检管理").HasOrderNo(6);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringMaintainProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as MeteringEquipmentAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                         .GetEquipAccountMaintainProjects(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipAccountMaintainProject>();
            }, ViewConfig.ListView).Show(ChildShowInWhere.All).HasLabel("保养项目").HasOrderNo(7);
        }

        /// <summary>
        /// 计量设备台账增加子页签
        /// </summary>
        protected void ConfigListViewChildrenTwo()
        {
            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringLubricationProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as MeteringEquipmentAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>().GetEquipAccountLubricationProject(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipAccountMaintainProject>();
            }).Show(ChildShowInWhere.All).HasLabel("润滑项目").HasOrderNo(8).LazyLoad(false);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringTpmMgtProperty, e =>
            {
                var account = e.Parent as MeteringEquipmentAccount;
                var tpmMgt = new EquipAccountTpmMgt();
                if (account == null)
                {
                    return tpmMgt;
                }
                tpmMgt.Id = account.Id;
                tpmMgt.MarkSaved();
                return tpmMgt;
            }, ViewConfig.DetailsView).HasLabel("TPM管理").HasOrderNo(9);

            View.AttachChildrenProperty(typeof(EquipRepairBill), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MeteringEquipmentAccount>();
                if (parent == null)
                {
                    return new EntityList<EquipRepairBill>();
                }
                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var state = (EquipRepairState?)parent.GetMeteringRepairStateDontMap();
                var repairs = RT.Service.Resolve<RepairController>().GetNotCompletedEquipRepairBills(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo, state);
                return repairs;
            }, MeterEquipRepairBillViewConfig.MeterEquipAccountRepairView).HasLabel("维修记录").HasOrderNo(10);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringSpPartRecordProperty, e =>
            {
                var account = e.Parent as MeteringEquipmentAccount;
                var record = new EquipAccountSpPartRecord();
                if (account == null)
                {
                    return record;
                }
                record.Id = account.Id;
                record.MarkSaved();
                return record;
            }, ViewConfig.DetailsView).HasLabel("备件记录").HasOrderNo(11);

            View.ChildrenProperty(p => p.ProcessList).HasLabel("工序列表").HasOrderNo(12);

            View.AttachChildrenProperty(typeof(MeterEquipAccountSlot), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccountBase>();
                if (parent == null)
                {
                    return new EntityList<EquipAccountSlot>();
                }
                var accountSlos = RT.Service.Resolve<EquipAccountSloController>().GetEquipAccountSlots(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return accountSlos;
            }, EquipAccountSloViewConfig.MeteringEquipmentSloListViewGroup).HasLabel("计量设备缸槽管理").HasOrderNo(13);



            View.AttachDetailChildrenProperty(typeof(MeteringEquipmentAccount), (c) =>
            {
                var account = c.Parent as MeteringEquipmentAccount;
                account = RF.GetById<MeteringEquipmentAccount>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, EISBaseDataViewGroup).HasLabel("电子行业").HasOrderNo(196);

            View.ChildrenProperty(p => p.EquipAccountLocationList).HasLabel("位置列表").HasOrderNo(197);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件资料").Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(EquipAccountAttachment), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var account = w.Parent as MeteringEquipmentAccount;
                if (account == null)
                {
                    return new EntityList<EquipAccountAttachment>();
                }
                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var attachment = RT.Service.Resolve<EquipAccountController>().GetEquipAccountAttachment(account.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return attachment;
            }).HasLabel("附件资料").HasOrderNo(200);
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

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.AddBehavior("SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Behaviors.AddMeteringEquipAccountBehavior");
            View.UseCommands(typeof(MeteringEquipSaveCommand).FullName);
            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.EquipModelId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipAccountController>().GetMeteringEquipModelLoadAll(pagingInfo, keyword);
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
                    var entity = source as SIE.Equipments.EquipAccounts.EquipAccountBase;
                    return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, pagingInfo, keyword);
                }).UsePagingLookUpEditor().HasLabel("管理部门".L10N() + "*").Show();

                View.Property(p => p.UseDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as SIE.Equipments.EquipAccounts.EquipAccountBase;
                    return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, pagingInfo, keyword);
                }).UsePagingLookUpEditor().Cascade(p => p.WorkShopId, null).HasLabel("使用部门".L10N() + "*").Show();
                View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var equipAccount = source as MeteringEquipmentAccount;
                    return RT.Service.Resolve<EnterpriseController>().GetDepartmentWorkShops(equipAccount.FactoryId, pagingInfo, keyword);
                }).UsePagingLookUpEditor().Show().Cascade(p => p.ResourceId, null);
                View.Property(p => p.ResourceId).UseDataSource((e, c, r) =>
                {
                    var equipAccount = e as MeteringEquipmentAccount;
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
                    var model = o as EquipAccountBase;
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
                ConfigDetailsViewChildren();
            }
            
        }

        /// <summary>
        /// 配置子视图属性
        /// </summary>
        private void ConfigDetailsViewChildren()
        {
            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringCheckProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as MeteringEquipmentAccount;

                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                         .GetEquipAccountCheckProjects(model.Id, arg.PagingInfo, arg.SortInfo);
                }

                return new EntityList<EquipAccountCheckProject>();
            }, MeteringEquipAccountCheckProjectViewConfig.EquipAccountDetailViewGroup)
            .HasLabel("点检项目")
            .HasOrderNo(8)
            .Show(ChildShowInWhere.All)
            .LazyLoad(false);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringMaintainProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as MeteringEquipmentAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                         .GetEquipAccountMaintainProjects(model.Id, arg.PagingInfo, arg.SortInfo);
                }

                return new EntityList<EquipAccountMaintainProject>();
            }, MeteringEquipAccountMaintainProjectViewConfig.EquipAccountDetailViewGroup)
                .Show(ChildShowInWhere.All).HasLabel("保养项目").HasOrderNo(20).LazyLoad(false);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringLubricationProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as MeteringEquipmentAccount;

                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>()
                        .GetEquipAccountLubricationProject(model.Id, arg.PagingInfo, arg.SortInfo);
                }

                return new EntityList<EquipAccountLubricationProject>();
            })
            .Show(ChildShowInWhere.All)
            .HasLabel("润滑项目")
            .HasOrderNo(21)
            .LazyLoad(false);

            View.AssociateChildrenProperty(MeteringEquipmentAccountExtension.MeteringEquipAccountRepairStandardListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as MeteringEquipmentAccount;

                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>()
                        .GetEquipAccountRepairStandard(model.Id, arg.PagingInfo, arg.SortInfo);
                }

                return new EntityList<EquipAccountRepairStandard>();
            })
           .Show(ChildShowInWhere.All)
           .HasLabel("维修定标")
           .HasOrderNo(22)
           .LazyLoad(false);

            View.ChildrenProperty(p => p.ResumeList).HasLabel("设备履历").Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件资料").HasOrderNo(89).Show(ChildShowInWhere.All);

            View.ChildrenProperty(p => p.EquipAccountLocationList).HasLabel("位置列表").HasOrderNo(90).Show(ChildShowInWhere.All);

            View.ChildrenProperty(p => p.ProcessList).HasLabel("工序列表").HasOrderNo(91).Show(ChildShowInWhere.All);

            View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);

            View.AttachDetailChildrenProperty(typeof(MeteringEquipmentAccount), (c) =>
            {
                var account = c.Parent as MeteringEquipmentAccount;
                account = RF.GetById<MeteringEquipmentAccount>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, EISBaseDataAddViewGroup).HasLabel("电子行业").HasOrderNo(100).Show(ChildShowInWhere.All);
        }


        /// <summary>
        /// 下拉选择
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("设备编码").Readonly().ShowInList(width: CoulmnWidth * 8);
                View.Property(p => p.Name).HasLabel("设备名称").Readonly().ShowInList(width: CoulmnWidth * 8);
            }
        }

    }
}