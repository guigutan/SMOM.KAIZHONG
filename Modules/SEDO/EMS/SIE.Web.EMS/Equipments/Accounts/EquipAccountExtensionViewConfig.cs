using SIE.Domain;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Web.Common;
using SIE.Web.EMS.Equipments.Accounts.Commands;
using SIE.Web.EMS.Equipments.Boms;
using SIE.Web.EMS.Equipments.Models;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipAccountExtensionViewConfig : WebViewConfig<EquipAccount>
    {
        /// <summary>
        /// 批量添加校验计划-选择设备台账视图
        /// </summary>
        public const string CalibrationPlanBatchAddList = "CalibrationPlanBatchAddList";

        /// <summary>
        /// 修改视图
        /// </summary>
        public const string EditViewGroup = "EditViewGroup";

        /// <summary>
        /// 图片
        /// </summary>
        public const string PhotoViewGroup = "PhotoView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup( EditViewGroup, PhotoViewGroup);

            if (ViewGroup == EditViewGroup)
            {
                this.ConfigDetailsView();
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
            View.RemoveBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountListViewBehavior");
            View.AddBehavior("SIE.Web.EMS.Equipments.Scripts.EquipAccountListViewBehavior");
            View.FormEdit();
            View.UseCommands(typeof(SynModelCommand).FullName, "SIE.Web.EMS.Equipments.Accounts.Commands.QRCodePrintCommand");
            View.Property(p => p.FixedAssetsAccountCode).HasOrderNo(45);
            View.Property(p => p.FixedAssetsAccountName).HasOrderNo(45);
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
               }, EquipModelTechParameterViewConfig.ReadOnlyView)
              .Show(ChildShowInWhere.All)
              .HasLabel("技术参数")
              .HasOrderNo(2)
              .LazyLoad(false);

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
            }, ViewConfig.DetailsView).HasLabel("点检管理").HasOrderNo(5);

            View.AssociateChildrenProperty(EquipAccountExtension.MaintainProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>().GetEquipAccountMaintainProjects(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipAccountMaintainProject>();
            }, ViewConfig.ListView).Show(ChildShowInWhere.All).HasLabel("保养项目").HasOrderNo(6);

            View.AssociateChildrenProperty(EquipAccountExtension.LubricationProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>().GetEquipAccountLubricationProject(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipAccountMaintainProject>();
            }).Show(ChildShowInWhere.All).HasLabel("润滑项目").HasOrderNo(7).LazyLoad(false);

            //添加子视图
            AddChildren();
        }

        /// <summary>
        /// 添加子视图
        /// </summary>
        protected void AddChildren()
        {
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
            }, ViewConfig.DetailsView).HasLabel("TPM管理").HasOrderNo(8);

            View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);

            View.AttachChildrenProperty(typeof(EquipBomDetail), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<EquipAccount>();
                if (parent == null)
                {
                    return new EntityList<EquipBomDetail>();
                }
                var bomDtls = RT.Service.Resolve<EquipBomController>().GetEquipBomDetailsByModelId(parent.EquipModelId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return bomDtls;
            }, EquipBomDetailViewConfig.AccountEquipBomDetailViewGroup).HasLabel("设备BOM").HasOrderNo(1);

            SetExtChildrenList();
        }

        private void SetExtChildrenList()
        {
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
            }, ViewConfig.DetailsView).HasLabel("备件记录").HasOrderNo(194);

            View.AssociateChildrenProperty(EquipAccountExtension.EquipAccountRepairStandardListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipAccount;
                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>().GetEquipAccountRepairStandard(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipAccountRepairStandard>();
            }).Show(ChildShowInWhere.All).HasLabel("维修定标").HasOrderNo(199).LazyLoad(false);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.RemoveBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.AddEquipAccountBehavior");
            View.AddBehavior("SIE.Web.EMS.Equipments.Scripts.AddEquipAccountBehavior");

            if (ViewGroup == EditViewGroup)
            {
                View.ReplaceCommands(typeof(Web.Equipments.EquipAccounts.Commands.EditSaveAccountCommand).FullName,
                    typeof(EquipmentsEditSaveAccountCommand).FullName);
            }

            using (View.OrderProperties())
            {
                View.Property(p => p.EquipModelId)
                    .UseDataSource((e, p, k) =>
                    {
                        var equipmentAccount = e as EquipAccount;
                        if (equipmentAccount == null)
                        {
                            return new EntityList<EquipModel>();
                        }

                        return RT.Service.Resolve<EquipController>().GetEquipModelsOfUserHasPermission(p, k);
                    })
                    .HasLabel("设备型号编码").Show();

                View.AssociateChildrenProperty(EquipAccountExtension.CheckProjectListProperty, c =>
                 {
                     var arg = c as ChildPagingDataArgs;
                     var model = c.Parent as EquipAccount;

                     if (model != null)
                     {
                         return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                              .GetEquipAccountCheckProjects(model.Id, arg.PagingInfo, arg.SortInfo);
                     }

                     return new EntityList<EquipAccountCheckProject>();
                 }, EquipAccountCheckProjectViewConfig.EquipAccountDetailViewGroup)
                .HasLabel("点检项目")
                .HasOrderNo(8)
                .Show(ChildShowInWhere.All)
                .LazyLoad(false);

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
                }, EquipAccountMaintainProjectViewConfig.EquipAccountDetailViewGroup)
                    .Show(ChildShowInWhere.All).HasLabel("保养项目").HasOrderNo(20).LazyLoad(false);

                View.AssociateChildrenProperty(EquipAccountExtension.LubricationProjectListProperty, c =>
                    {
                        var arg = c as ChildPagingDataArgs;
                        var model = c.Parent as EquipAccount;

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

                View.AssociateChildrenProperty(EquipAccountExtension.EquipAccountRepairStandardListProperty, c =>
                {
                    var arg = c as ChildPagingDataArgs;
                    var model = c.Parent as EquipAccount;

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
            View.Property(p => p.ModelCode).Readonly();
            View.Property(p => p.ModelName).Readonly();
            View.Property(p => p.SupplierId).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.EquipTypeCode).Readonly();
            View.Property(p => p.EquipTypeName).Readonly();
            View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType;c.ReloadDataOnPopping = true; }).Readonly();
            View.Property(p => p.WorkShopId).HasLabel("车间");
            View.Property(p => p.ProcessId).HasLabel("工序");
            View.Property(p => p.InstallationLocation);
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
