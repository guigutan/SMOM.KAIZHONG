using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.Equipments.Models;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.Equipments.DeviceIOTParas.Controllers;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Common.Configs.Commands;
using SIE.Web.EMS.EquipRepair.EquipRepairs;
using SIE.Web.EMS.SpecialEquipment.Commands;
using SIE.Web.EMS.SpecialEquipment.SpecialEquipmentAcounts.ExtensionViewConfig;
using SIE.Web.Equipments.DeviceIOTParas.Details;
using SIE.Web.Equipments.EquipAccounts;
using SIE.Web.Equipments.EquipAccounts.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
namespace SIE.Web.EMS.SpecialEquipment.SpecialEquipmentAcounts
{
    /// <summary>
    /// 特种设备台账视图配置
    /// </summary>
    public class SpecialEquipmentAccountViewConfig : WebViewConfig<SpecialEquipmentAccount>
    {
        /// <summary>
        /// 修改视图
        /// </summary>
        public readonly static string EditViewGroup = "EditViewGroup";

        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private readonly static int CoulmnWidth = 20;

        /// <summary>
        /// 电子行业基础数据
        /// </summary>
        public readonly static string EISBaseDataViewGroup = "EISBaseDataViewGroup";

        /// <summary>
        /// 电子行业基础数据ADD
        /// </summary>
        public readonly static string EISBaseDataAddViewGroup = "EISBaseDataAddViewGroup";

        /// <summary>
        /// 图片
        /// </summary>
        public readonly static string PhotoViewGroup = "PhotoView";


        /// <summary>
        /// 特种设备台账视图
        /// </summary>
        public readonly static string SpecialEquipmentAccountGroup = "SpecialEquipmentAccountView";



        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditViewGroup, EISBaseDataViewGroup, EISBaseDataAddViewGroup, PhotoViewGroup);

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

            if (ViewGroup == SpecialEquipmentAccountGroup)
            {
                SpecialEquipmentAccountView();
            }

        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountListViewBehavior");
            View.AssignAuthorize(typeof(SpecialEquipmentAccount));
            View.FormEdit();
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.AddAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.AddChildAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.EditAccountCommand");
            View.UseCommand("SIE.Web.Equipments.EquipAccounts.Commands.CopyAccountCommand");
            View.UseCommand(WebCommandNames.Delete);
            View.UseCommands(typeof(UpgradeAccountCommand).FullName, typeof(DowngradeAccountCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("设备编码").Readonly().ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.Name).HasLabel("设备名称").Readonly().ShowInList(width: CoulmnWidth * 15);
                View.Property(p => p.Alias).Readonly().ShowInList(width: CoulmnWidth * 10);
                View.Property(p => p.OriginalSerialNumber).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.RFID).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ModelCode).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ModelName).Readonly().ShowInList(width: CoulmnWidth * 10);
                View.Property(p => p.RegularInspectionStatus).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.State).Readonly().ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.UseState).Readonly().ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.Frozen).Readonly();
                View.Property(p => p.EquipTypeCode).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.EquipTypeName).HasLabel("设备类型").Readonly().ShowInList(width: CoulmnWidth * 8);
                View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.NextInspectionDate).ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.IndustryCategory).ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.IsVirtual);
                View.Property(p => p.IsCustomsSupervision).HasLabel("海关监管");
                View.Property(p => p.EquipmentGrading).ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.UseLevel).UseCatalogEditor(p => { p.CatalogType = EquipAccount.EquipAccountUseLevel; p.CatalogReloadData = true; }).HasLabel("ABC分类");
                View.Property(p => p.Factory).UseFactoryEditor().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.UseDepartmentId).ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.WorkShopId).HasLabel("车间").ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ResourceId).HasLabel("产线").ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ProcessId).HasLabel("工序").ShowInList(width: CoulmnWidth * 6);
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
                }).HasLabel("供应商编码").ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.SupplierName).Readonly().ShowInList(width: CoulmnWidth * 10);

                View.Property(p => p.PurchaseOrderNo).ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.InstallationLocation).ShowInList(width: CoulmnWidth * 15);
                View.Property(p => p.EnterDate).UseDateEditor().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.CardDate).UseDateEditor().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.UsefulLife).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.WarrantyPeriod).ShowInList(width: CoulmnWidth * 6);
            }
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList(width: 100).Readonly();
            View.Property(p => p.Name).ShowInList(width: 100).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.UseState).Readonly();
            View.Property(p => p.EquipModelId).Readonly().HasLabel("设备型号编码");
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


        /// <summary>
        /// 特种设备台账
        /// </summary>
        protected void SpecialEquipmentAccountView()
        {
            View.AddBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountListViewBehavior");
            View.AssignAuthorize(typeof(SpecialEquipmentAccount));
            View.UseCommands(typeof(SynSpecialModelCommand).FullName);
            View.FormEdit();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("设备编码").Readonly().ShowInList(width: CoulmnWidth * 8);
                View.Property(p => p.Name).HasLabel("设备名称").Readonly().ShowInList(width: CoulmnWidth * 8);
                View.Property(p => p.Alias).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ModelCode).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.ModelName).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.State).Readonly().ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.UseState).Readonly().ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.Frozen).Readonly().ShowInList(width: CoulmnWidth * 3);
                View.Property(p => p.EquipTypeCode).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.EquipTypeName).HasLabel("设备类型").Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType; c.CatalogReloadData = true; }).Readonly().ShowInList(width: CoulmnWidth * 6);
                View.Property(p => p.IndustryCategory).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.IsVirtual).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.IsCustomsSupervision).HasLabel("海关监管").ShowInList(width: CoulmnWidth * 4);
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
                View.Property(p => p.UsefulLife).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).ShowInList(width: CoulmnWidth * 4);
                View.Property(p => p.WarrantyPeriod).ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.NextInspectionDate).ShowInList(width: CoulmnWidth * 5);
                View.Property(p => p.RegularInspectionStatus).ShowInList(width: CoulmnWidth * 5);
            }
            ConfigListViewChildrenOne();
            ConfigListViewChildrenTwo();
        }

        /// <summary>
        /// 特种设备台账增加子页签
        /// </summary>
        protected void ConfigListViewChildrenOne()
        {
            View.ChildrenProperty(p => p.PcbSlotList).HasLabel("缸槽管理").Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipAccountRegularInspectionList).HasLabel("设备定检规程").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(EquipBomDetail), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<EquipAccount>();
                if (parent == null)
                {
                    return new EntityList<EquipBomDetail>();
                }
                var bomDtls = RT.Service.Resolve<EquipBomController>()
                    .GetEquipBomDetailsByModelId(parent.EquipModelId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return bomDtls;
            }, EquipBomDetailExtensionViewConfig.AccountEquipBomDetailViewGroup).HasLabel("设备BOM").HasOrderNo(2);

            View.AssociateChildrenProperty(EquipAccountExtension.TechParameterListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipAccount;
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
                var parent = args.Parent as EquipAccount;
                if (parent == null)
                {
                    return new EntityList<PhysicalUnion>();
                }
                var physicalUnion = RT.Service.Resolve<DeviceIOTParaController>()
                    .GetPhysicalUnions(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return physicalUnion;
            }, PhysicalUnionViewConfig.AccountPhysicalUnionViewGroup).HasLabel("设备参数").HasOrderNo(4);

            View.ChildrenProperty(p => p.ResumeList).Show(ChildShowInWhere.Hide);//隐藏标准产品的
            View.AttachChildrenProperty(typeof(EquipAccountResume), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<EquipAccount>();
                if (parent == null)
                {
                    return new EntityList<EquipAccountResume>();
                }
                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var state = (ResumeType?)parent.GetResumeStateDontMap();
                var sesumes = RT.Service.Resolve<EquipAccountController>().GetEquipAccountResumes(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo, state);
                return sesumes;
            }).HasLabel("设备履历").HasOrderNo(4);

            View.AssociateChildrenProperty(EquipAccountExtension.CheckMgtProperty, e =>
            {
                var account = e.Parent as EquipAccount;
                var checkMgt = new EquipAccountCheckMgt();
                if (account == null)
                {
                    return checkMgt;
                }
                checkMgt.Id = account.Id;
                checkMgt.MarkSaved();
                return checkMgt;
            }, ViewConfig.DetailsView).HasLabel("点检管理").HasOrderNo(6);

            View.AssociateChildrenProperty(EquipAccountExtension.MaintainProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                         .GetEquipAccountMaintainProjects(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipAccountMaintainProject>();
            }, ViewConfig.ListView).Show(ChildShowInWhere.All).HasLabel("保养项目").HasOrderNo(7);
        }

        /// <summary>
        /// 特种设备台账增加子页签
        /// </summary>
        protected void ConfigListViewChildrenTwo()
        {
            View.AssociateChildrenProperty(EquipAccountExtension.LubricationProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>().GetEquipAccountLubricationProject(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipAccountMaintainProject>();
            }).Show(ChildShowInWhere.All).HasLabel("润滑项目").HasOrderNo(8).LazyLoad(false);

            View.AssociateChildrenProperty(EquipAccountExtension.TpmMgtProperty, e =>
            {
                var account = e.Parent as EquipAccount;
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
                var parent = args.ParentEntity.ToJsonObject<EquipAccount>();
                if (parent == null)
                {
                    return new EntityList<EquipRepairBill>();
                }
                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var state = (EquipRepairState?)parent.GetRepairStateDontMap();
                var repairs = RT.Service.Resolve<RepairController>().GetNotCompletedEquipRepairBills(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo, state);
                return repairs;
            }, EquipRepairViewConfig.EquipAccountRepairView).HasLabel("维修记录").HasOrderNo(10);

            View.AssociateChildrenProperty(EquipAccountExtension.SpPartRecordProperty, e =>
            {
                var account = e.Parent as EquipAccount;
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

            View.AttachChildrenProperty(typeof(EquipAccountSlot), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipAccount>();
                if (parent == null)
                {
                    return new EntityList<EquipAccountSlot>();
                }
                var accountSlos = RT.Service.Resolve<EquipAccountSloController>().GetEquipAccountSlots(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return accountSlos;
            }, EquipAccountSloViewConfig.SpecialEquipmentAccountViewGroup).HasLabel("特种设备缸槽管理").HasOrderNo(13);

            View.AttachDetailChildrenProperty(typeof(EquipAccount), (c) =>
            {
                var account = c.Parent as EquipAccount;
                account = RF.GetById<EquipAccount>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, EISBaseDataViewGroup).HasLabel("电子行业").HasOrderNo(196);

            View.ChildrenProperty(p => p.EquipAccountLocationList).HasLabel("位置列表").HasOrderNo(197);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件资料").Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(EquipAccountAttachment), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var account = w.Parent as SpecialEquipmentAccount;
                if (account == null)
                {
                    return new EntityList<EquipAccountAttachment>();
                }
                //界面加载数据源、页签工具栏查询按钮触发的数据源
                var attachment = RT.Service.Resolve<EquipAccountController>().GetEquipAccountAttachment(account.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return attachment;
            }).HasLabel("附件资料").HasOrderNo(200);
        }
    }
}