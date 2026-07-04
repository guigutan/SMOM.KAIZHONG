using SIE.Common.Catalogs;
using SIE.Common.Domain;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.EquipTypes;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.Equipments.EquipmentCards;
using SIE.Web.Equipments.EquipmentCards.Commands;
using SIE.Web.Equipments.Extensions;
using SIE.Web.Equipments.WorkFlows;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using SIE.Web.Equipments;
using SIE.Equipments.EquipModels;
using SIE.CSM.Suppliers;
using SIE.Resources.Enterprises;

namespace SIE.Web.Equipments.EuipmentCards
{
    /// <summary>
    /// 设备卡片视图配置
    /// </summary>
    public class EquipmentCardViewConfig : WebViewConfig<EquipmentCard>
    {
        /// <summary>
        /// 查看
        /// </summary>
        public const string SeeView = "SeeView";

        /// <summary>
        /// 修改
        /// </summary>
        public const string EditView = "EditView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SeeView, EditView);
            switch (ViewGroup)
            {
                case SeeView:
                    ConfigSeeView();
                    break;
                case EditView:
                    this.ConfigDetailsView();
                    View.RemoveCommands(typeof(SaveEquipCardCommand).FullName);
                    View.UseCommands(typeof(EditSaveEquipCardCommand).FullName);
                    break;
                default:
                    break;
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Equipments.EquipmentCards.Behaviors.EquipmentCardsListBehavior");
            View.AddBehavior("SIE.Web.Equipments.Common.Scripts.ApprovalBehavior");
            View.FormEdit();
            View.UseCommands(WebCommandNames.Add, EquipmentCardCommands.Edit, EquipmentCardCommands.Delete, EquipmentCardCommands.SeeView, typeof(SubmitEquipCardCommand).FullName, typeof(CancelEquipCardCommand).FullName, typeof(EquipmentCardImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand", typeof(AuditEquipCardCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);

            using (View.OrderProperties())
            {
                View.Property(p => p.ApprovalStatus);
                View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("工厂");
                View.Property(p => p.UseDepartmentId).HasLabel("使用部门");
                View.Property(p => p.UserId);
                View.Property(p => p.AccountState).HasLabel("设备状态");
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.Alias);
                View.Property(p => p.EquipModelId).UsePagingLookUpEditor((m, e) =>
                {
                    m.ReloadDataOnPopping = true;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
                    dic.Add(nameof(e.EquipModelType), nameof(e.EquipModel.EquipType.TypeName));
                    dic.Add(nameof(e.EquipModelSpecifications), nameof(e.EquipModel.Specifications));
                    dic.Add(nameof(e.EquipTypeCategory), nameof(e.EquipModel.TypeCategory));
                    dic.Add(nameof(e.IndustryCategory), nameof(e.EquipModel.IndustryCategory));
                    m.DicLinkField = dic;
                }).HasLabel("型号编码");
                View.Property(p => p.EquipModelName).HasLabel("型号名称").Readonly();
                View.Property(p => p.EquipModelType).HasLabel("设备类型").Readonly();
                View.Property(p => p.EquipModelSpecifications).HasLabel("技术规格").Readonly();
                View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).HasLabel("设备类别").Readonly();
                View.Property(p => p.IndustryCategory).HasLabel("行业属性").Readonly();
                View.Property(p => p.OriginalSerialNumber);
                View.Property(p => p.Rfid).HasLabel("RFID");
                View.Property(p => p.UseLevel);
                View.Property(p => p.ManagementId).HasLabel("管理部门");
                View.Property(p => p.AccountUseState);
                View.Property(p => p.WorkShopId).HasLabel("车间");
                View.Property(p => p.ResourceId).HasLabel("产线");
                View.Property(p => p.WarehouseId).HasLabel("仓库");
                View.Property(p => p.StorageLocationId).HasLabel("库位");
                View.Property(p => p.AdministratorId).HasLabel("保管人");
                View.Property(p => p.Proprietorship);
                View.Property(p => p.PurchaseUnit);
                View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                {
                    m.ReloadDataOnPopping = true;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    m.DicLinkField = dic;
                });
                View.Property(p => p.SupplierName).HasLabel("供应商名称").Readonly();
                View.Property(p => p.Manufacturer);
                View.Property(p => p.PurchaseOrderNo);
                View.Property(p => p.EnterDate).UseDateEditor();
                View.Property(p => p.CreateCardDateTime).UseDateEditor();
                View.Property(p => p.IsCustomsSupervision).UseCheckDropDownEditor().Readonly();
                View.Property(p => p.IssAsset).UseCheckDropDownEditor().Readonly().HasLabel("是否固定资产");
                View.Property(p => p.FixedAssetsAccountCode);
                View.Property(p => p.FixedAssetsAccountName);
                View.Property(p => p.AssetUserId).HasLabel("资产责任人");
                View.Property(p => p.OriginalValue).HasLabel("原值");
                View.Property(p => p.NetAssetValue);
                View.Property(p => p.UsefulLife).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.AllowNegative = false;
                    p.DecimalPrecision = 2;
                });
                View.Property(p => p.WarrantyPeriod).UseDateEditor();
                View.Property(p => p.InstallationLocation);
                View.Property(p => p.EquipmentCardSource);
                //修改记录
                View.AttachChildrenProperty(typeof(EntityLog), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<EquipmentCard>();
                    if (parent == null)
                    {
                        return new EntityList<EntityLog>();
                    }

                    EntityList<EntityLog> list = RT.Service.Resolve<EquipmentCardController>().GetList(typeof(EquipmentCard), parent.Id, args.SortInfo, args.PagingInfo);
                    return list;
                }, EntityLogViewConfig.RegSeeView).HasLabel("修改记录").HasOrderNo(1);

                //审核记录
                View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<EquipmentCard>();
                    if (parent == null)
                    {
                        return new EntityList<WorkFlowRecord>();
                    }
                    return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(EquipmentCard).FullName, args.SortInfo, args.PagingInfo);
                }, WorkFlowRecordViewConfig.RegSeeView).HasLabel("审核记录").HasOrderNo(2);
            }
        }


        /// <summary>
        /// 明细页面
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.AddBehavior("SIE.Web.Equipments.EquipmentCards.Behaviors.AddEquipmentCardBehavior");
            View.UseCommands(typeof(SaveEquipCardCommand).FullName);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("基本信息", 4, false))
                {
                    View.Property(p => p.Code).Readonly();
                    View.Property(p => p.Name).HasLabel("设备名称".L10N() + "*");
                    View.Property(p => p.Alias);
                    View.Property(p => p.AccountState).HasLabel("设备状态");

                    View.Property(p => p.ManagementId).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as EquipmentCard;
                        return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, pagingInfo, keyword);
                    }).HasLabel("管理部门".L10N()+"*");
                    View.Property(p => p.AccountUseState);
                    View.Property(p => p.OriginalSerialNumber).HasLabel("原厂序列号");
                    View.Property(p => p.Rfid).HasLabel("RFID");

                    View.Property(p => p.EquipModelId).UsePagingLookUpEditor((m, e) =>
                    {
                        m.ReloadDataOnPopping = true;
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
                        dic.Add(nameof(e.EquipModelType), nameof(e.EquipModel.EquipType.TypeName));
                        dic.Add(nameof(e.EquipModelSpecifications), nameof(e.EquipModel.Specifications));
                        dic.Add(nameof(e.EquipTypeCategory), nameof(e.EquipModel.TypeCategory));
                        dic.Add(nameof(e.IndustryCategory), nameof(e.EquipModel.IndustryCategory));
                        m.DicLinkField = dic;
                    }).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
                    }).HasLabel("型号编码");
                    View.Property(p => p.EquipModelName).HasLabel("型号名称").Readonly();
                    View.Property(p => p.EquipModelType).HasLabel("设备类型").Readonly();
                    View.Property(p => p.EquipModelSpecifications).HasLabel("技术规格").Readonly();
                    View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).Readonly();
                    View.Property(p => p.IndustryCategory).HasLabel("行业属性").Readonly();
                    View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.CatalogReloadData = true; }).HasLabel("ABC分类".L10N() + "*");
                    View.Property(p => p.EquipModelGrade).HasLabel("设备评级");
                }
            }
            ConfigDetailsViewT();
        }

        /// <summary>
        /// 扩展ConfigDetailsView 位置信息,资产信息,来源信息
        /// </summary>
        public void ConfigDetailsViewT()
        {
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("位置信息", 4, false))
                {
                    View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("工厂").Cascade(p => p.ManagementId, null).Cascade(p => p.UseDepartmentId, null).Cascade(p => p.WorkShopId, null).Cascade(p => p.ResourceId, null);
                    View.Property(p => p.UseDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as EquipmentCard;
                        return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, pagingInfo, keyword);
                    }).HasLabel("使用部门".L10N() + "*");
                    View.Property(p => p.UserId).UseDataSource((o, e, r) =>
                    {
                        return RT.Service.Resolve<EmployeeController>().GetEmployeeListOnJob(e, r);
                    }).HasLabel("使用责任人");
                    View.Property(p => p.WorkShopId).UseFactoryWorkshopEditor().HasLabel("车间").Cascade(p => p.ResourceId, null);

                    View.Property(p => p.ResourceId).UseWorkShopResourceEditor().HasLabel("产线");
                    View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
                    }).Cascade(p => p.StorageLocationId, null).HasLabel("仓库").Readonly(p => p.EquipmentCardSource == EquipmentCardSource.EquipmentReceive);
                    View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
                    {
                        var model = o as EquipmentCard;
                        if (model == null)
                        {
                            return new EntityList<StorageLocation>();
                        }
                        return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(null, model.WarehouseId, r, e);
                    }).HasLabel("库位");
                    View.Property(p => p.AdministratorId).UseDataSource((o, e, r) =>
                    {
                        return RT.Service.Resolve<EmployeeController>().GetEmployeeListOnJob(e, r);
                    }).HasLabel("保管人");

                    View.Property(p => p.InstallationLocation);
                }
                using (View.DeclareGroup("资产信息", 4, false))
                {
#pragma warning disable S1125 // Boolean literals should not be redundant
                    View.Property(p => p.IssAsset).HasLabel("是否固定资产").UseCheckDropDownEditor().Cascade(p => p.AssetCode, null).Cascade(p => p.AssetName, null).Cascade(p => p.OriginalValue, null).Readonly(p => p.IsEnableAsset == true);
#pragma warning restore S1125 // Boolean literals should not be redundant
#pragma warning disable S1125 // Boolean literals should not be redundant
                    View.Property(p => p.AssetCode).HasLabel("固定资产编码").Readonly(p => p.IsEnableAsset == true || p.IssAsset != true);
#pragma warning restore S1125 // Boolean literals should not be redundant
#pragma warning disable S1125 // Boolean literals should not be redundant
                    View.Property(p => p.AssetName).HasLabel("资产名称").Readonly(p => p.IsEnableAsset == true || p.IssAsset != true);
#pragma warning restore S1125 // Boolean literals should not be redundant
#pragma warning disable S1125 // Boolean literals should not be redundant
                    View.Property(p => p.AssetUserId).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                    }).HasLabel("资产责任人").Readonly(p => p.IsEnableAsset == true || p.IssAsset != true);
#pragma warning restore S1125 // Boolean literals should not be redundant

                    View.Property(p => p.IsCustomsSupervision).UseCheckDropDownEditor();//.UseCheckDropDownEditor(p => p.AllowBlank = false)
                    View.Property(p => p.OriginalValue).HasLabel("资产原值").UseSpinEditor(p =>
                    {
                        p.AllowDecimals = true;
                        p.MinValue = 0;
                        p.DecimalPrecision = 2;
#pragma warning disable S1125 // Boolean literals should not be redundant
                    }).Readonly(p => p.IsEnableAsset == true || p.IssAsset == false);
#pragma warning restore S1125 // Boolean literals should not be redundant
                    View.Property(p => p.NetAssetValue).HasLabel("资产净值").Readonly();
                    View.Property(p => p.UsefulLife).UseSpinEditor(p =>
                    {
                        p.AllowDecimals = true;
                        p.MinValue = 0;
                        p.DecimalPrecision = 2;
                    }).HasLabel("使用年限".L10N() + "*");
                    View.Property(p => p.WarrantyPeriod).UseDateEditor();
                }
                using (View.DeclareGroup("来源信息", 4, false))
                {
                    View.Property(p => p.Proprietorship);
                    View.Property(p => p.PurchaseUnit);
                    View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                    {
                        m.ReloadDataOnPopping = true;
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                        m.DicLinkField = dic;
                    }).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
                    }).HasLabel("供应商编码");
                    View.Property(p => p.SupplierName).HasLabel("供应商名称").Readonly();
                    View.Property(p => p.PurchaseOrderNo).Readonly();
                    View.Property(p => p.Manufacturer);
                    View.Property(p => p.EnterDate).UseDateEditor();
                    View.Property(p => p.CreateCardDateTime).UseDateEditor().Readonly();
                    View.Property(p => p.EquipmentCardSource).Readonly();
                }
            }
        }


        /// <summary>
        /// 查看视图
        /// </summary>
        public void ConfigSeeView()
        {
            View.AddBehavior("SIE.Web.Equipments.EquipmentCards.Behaviors.SetEquipmentCardDirtyBehavior");
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("基本信息", 4, false))
                {
                    View.Property(p => p.Code).Readonly();
                    View.Property(p => p.Name).Readonly();
                    View.Property(p => p.Alias).Readonly();
                    View.Property(p => p.AccountState).HasLabel("设备状态").Readonly();

                    View.Property(p => p.ManagementId).HasLabel("管理部门").Readonly();
                    View.Property(p => p.AccountUseState).Readonly();
                    View.Property(p => p.OriginalSerialNumber).HasLabel("原厂序列号").Readonly();
                    View.Property(p => p.Rfid).HasLabel("RFID").Readonly();

                    View.Property(p => p.EquipModelId).UsePagingLookUpEditor((m, e) =>
                    {
                        m.ReloadDataOnPopping = true;
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.EquipModelName), nameof(e.EquipModel.Name));
                        dic.Add(nameof(e.EquipModelType), nameof(e.EquipModel.EquipType.TypeName));
                        dic.Add(nameof(e.EquipModelSpecifications), nameof(e.EquipModel.Specifications));
                        dic.Add(nameof(e.EquipTypeCategory), nameof(e.EquipModel.TypeCategory));
                        dic.Add(nameof(e.IndustryCategory), nameof(e.EquipModel.IndustryCategory));
                        m.DicLinkField = dic;
                    }).HasLabel("设备型号").Readonly();
                    View.Property(p => p.EquipModelName).HasLabel("设备名称").Readonly();
                    View.Property(p => p.EquipModelType).HasLabel("设备类型").Readonly();
                    View.Property(p => p.EquipModelSpecifications).HasLabel("技术规格").Readonly();

                    View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).HasLabel("设备类别").Readonly();
                    View.Property(p => p.IndustryCategory).HasLabel("行业属性").Readonly();
                    View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.CatalogReloadData = true; }).HasLabel("ABC分类").Readonly();
                    View.Property(p => p.EquipModelGrade).HasLabel("设备评级").Readonly();
                }


                using (View.DeclareGroup("位置信息", 4, false))
                {
                    View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("工厂").Readonly();
                    View.Property(p => p.UseDepartmentId).HasLabel("使用部门").Readonly();
                    View.Property(p => p.UserId).HasLabel("使用责任人").Readonly();
                    View.Property(p => p.WorkShopId).HasLabel("车间").Readonly();
                    View.Property(p => p.ResourceId).HasLabel("产线").Readonly();
                    View.Property(p => p.WarehouseId).HasLabel("仓库").Readonly();
                    View.Property(p => p.StorageLocationId).HasLabel("库位").Readonly();
                    View.Property(p => p.AdministratorId).HasLabel("保管人").Readonly();
                    View.Property(p => p.InstallationLocation).Readonly();
                }


                using (View.DeclareGroup("资产信息", 4, false))
                {
                    View.Property(p => p.IssAsset).HasLabel("是否固定资产").Readonly();
                    View.Property(p => p.AssetCode).HasLabel("固定资产编码").Readonly();
                    View.Property(p => p.AssetName).HasLabel("资产名称").Readonly();
                    View.Property(p => p.AssetUserId).HasLabel("资产责任人").Readonly();

                    View.Property(p => p.IsCustomsSupervision).UseCheckDropDownEditor(p => p.AllowBlank = false).Readonly();
                    View.Property(p => p.OriginalValue).HasLabel("原值").Readonly();
                    View.Property(p => p.NetAssetValue);
                    View.Property(p => p.UsefulLife).UseSpinEditor(p =>
                    {
                        p.AllowDecimals = true;
                        p.MinValue = 0;
                        p.DecimalPrecision = 2;
                    }).Readonly();

                    View.Property(p => p.WarrantyPeriod).UseDateEditor().Readonly();
                }

                using (View.DeclareGroup("来源信息", 4, false))
                {
                    View.Property(p => p.Proprietorship).Readonly();
                    View.Property(p => p.PurchaseUnit).Readonly();
                    View.Property(p => p.SupplierId).UsePagingLookUpEditor((m, e) =>
                    {
                        m.ReloadDataOnPopping = true;
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                        m.DicLinkField = dic;
                    }).HasLabel("供应商编码").Readonly();
                    View.Property(p => p.SupplierName).HasLabel("供应商名称").Readonly();

                    View.Property(p => p.PurchaseOrderNo).Readonly();
                    View.Property(p => p.Manufacturer).Readonly();
                    View.Property(p => p.EnterDate).UseDateEditor().Readonly();
                    View.Property(p => p.CreateCardDateTime).UseDateEditor().Readonly();
                    View.Property(p => p.EquipmentCardSource).Readonly();
                }
            }
        }

        /// <summary>
        /// 导入配置
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.FactoryCode).HasLabel("工厂编码".L10N() + "*").BeforeImportRequireFunc("工厂编码");

            View.PropertyRef(p => p.UseDepartmentCode).HasLabel("使用部门编码".L10N() + "*");
            View.PropertyRef(p => p.User.Code).HasLabel("使用责任人编码");
            View.Property(p => p.Code).ImportIndexer(true).HasLabel("设备编码".L10N() + "*").BeforeImportRequireFunc("设备编码");
            View.Property(p => p.Name).HasLabel("设备名称".L10N() + "*").BeforeImportRequireFunc("设备名称"); 
            View.Property(p => p.Alias).HasLabel("设备别名");
            View.PropertyRef(p => p.EquipModel.Code).HasLabel("设备型号编码".L10N() + "*").BeforeImportRequireFunc("设备型号编码");
            View.Property(p => p.OriginalSerialNumber).HasLabel("原厂序列号");
            View.Property(p => p.Rfid).HasLabel("RFID");
            View.Property(p => p.UseLevel).HasLabel("ABC分类".L10N() + "*").ImportCatalogType(EquipAccount.EquipAccountUseLevel, Catalog.NameProperty.Name).BeforeImportRequireFunc("ABC分类");
            View.Property(p => p.AccountState).HasLabel("设备状态".L10N() + "*").BeforeImportRequireFunc("设备状态"); 
            
            View.PropertyRef(p => p.ManagementCode).HasLabel("管理部门编码");
            View.Property(p => p.AccountUseState).HasLabel("管理状态*").BeforeImportRequireFunc("管理状态");
            View.PropertyRef(p => p.WorkShopCode).HasLabel("车间编码");
            View.PropertyRef(p => p.ResourceCode).HasLabel("产线编码");
            View.PropertyRef(p => p.Warehouse.Code).HasLabel("仓库编码");
            View.Property(p => p.StorageLocationCode).HasLabel("库位编码");
            View.Property(p => p.Proprietorship).HasLabel("资产来源".L10N() + "*").BeforeImportRequireFunc("资产来源"); 
            View.Property(p => p.PurchaseUnit).HasLabel("租赁/客供单位");
            View.PropertyRef(p => p.Supplier.Code).HasLabel("供应商编码");
            View.Property(p => p.Manufacturer).HasLabel("生产厂家");
            View.Property(p => p.PurchaseOrderNo).HasLabel("采购单号");
            View.Property(p => p.EnterDate).HasLabel("入厂日期".L10N() + "*").BeforeImportRequireFunc("入厂日期"); 
            View.Property(p => p.IsCustomsSupervision).HasLabel(" 海关监管设备");
            View.Property(p => p.IssAsset).HasLabel(" 是否固定资产");
            View.Property(p => p.AssetCode).HasLabel(" 固定资产编码");
            View.PropertyRef(p => p.AssetName).HasLabel("固定资产名称");
            View.PropertyRef(p => p.AssetUser.Code).HasLabel("资产责任人编码");
            View.PropertyRef(p => p.OriginalValue).HasLabel("原值");
            View.PropertyRef(p => p.UsefulLife).HasLabel("使用年限");
            View.PropertyRef(p => p.WarrantyPeriod).HasLabel("保修期");
            View.PropertyRef(p => p.InstallationLocation).HasLabel("位置");
        }
    }
}