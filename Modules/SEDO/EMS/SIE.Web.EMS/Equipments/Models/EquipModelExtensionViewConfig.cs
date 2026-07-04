using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipModels;
using SIE.Web.EMS.Equipments.Boms;
using SIE.Web.EMS.Equipments.Models.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Models
{
    /// <summary>
    /// 设备型号维护视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipModelExtensionViewConfig : WebViewConfig<EquipModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.EMS.Equipments.Models.Commands.OpenEquipBOMCommand", typeof(EquipModelProjectImportCommand).FullName);

            #region 标准产品页签

            View.AssociateChildrenProperty(EquipModelExtension.CheckProjectListProperty,
                c =>
                {
                    var arg = c as ChildPagingDataArgs;
                    var model = c.Parent as EquipModel;

                    if (model != null)
                    {
                        return RT.Service.Resolve<EquipController>()
                             .GetCheckProjectsOfModels(model.Id, arg.PagingInfo, arg.SortInfo);
                    }

                    return new EntityList<EquipModelCheckProject>();
                }, ViewConfig.ListView).HasLabel("点检项目").Show(ChildShowInWhere.All).HasOrderNo(110);

            View.AssociateChildrenProperty(EquipModelExtension.MaintainProjectListProperty,
                c =>
                {
                    var arg = c as ChildPagingDataArgs;
                    var model = c.Parent as EquipModel;

                    if (model != null)
                    {
                        return RT.Service.Resolve<EquipController>()
                             .GetMaintainProjectsOfModels(model.Id, arg.PagingInfo, arg.SortInfo);
                    }

                    return new EntityList<EquipModelMaintainProject>();
                }, ViewConfig.ListView).HasLabel("保养项目").Show(ChildShowInWhere.All).HasOrderNo(120);
            //润滑项目
            View.AssociateChildrenProperty(EquipModelExtension.LubricationProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipModel;

                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>()
                          .GetLubricationProjectList(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipModelLubricationProject>();
            }, ViewConfig.ListView).HasLabel("润滑项目").Show(ChildShowInWhere.All).HasOrderNo(131);

            View.AttachChildrenProperty(typeof(EquipBomDetail),
                (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as EquipModel;
                if (parent == null)
                    return new EntityList<EquipBomDetail>();

                return RT.Service.Resolve<EquipBomController>().GetEquipBomDetailsByModelId(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
            }, EquipBomDetailViewConfig.AccountEquipBomDetailViewGroup).HasLabel("设备BOM").Show(ChildShowInWhere.All).HasOrderNo(140);

            View.AssociateChildrenProperty(EquipModelExtension.EquipModelRepairProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipModel;
                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>().GetEquipModelRepairProjectList(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipModelRepairProject>();
            }, ViewConfig.ListView).HasLabel("维修项目").Show(ChildShowInWhere.All).HasOrderNo(141);

            View.AssociateChildrenProperty(EquipModelExtension.EquipModelTechParameterListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipModel;
                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>().GetEquipModelTechParameterList(model.Id, arg.PagingInfo,null);
                }

                return new EntityList<EquipModelTechParameter>();
            }, ViewConfig.ListView).HasLabel("技术参数").Show(ChildShowInWhere.All).HasOrderNo(150);
            #endregion
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.RemoveBehavior("SIE.Web.Equipments.EquipModels.Scripts.EquipModelBehavior");
            View.AddBehavior("SIE.Web.EMS.Equipments.Scripts.EquipModelBehavior");
            #region 标准产品页签

            View.AssociateChildrenProperty(EquipModelExtension.CheckProjectListProperty, c =>
                 {
                     var arg = c as ChildPagingDataArgs;
                     var model = c.Parent as EquipModel;

                     if (model != null)
                     {
                         return RT.Service.Resolve<EquipController>()
                              .GetCheckProjectsOfModels(model.Id, arg.PagingInfo, arg.SortInfo);
                     }
                     return new EntityList<EquipModelCheckProject>();
                 }, ViewConfig.ListView).HasLabel("点检项目").Show(ChildShowInWhere.All).HasOrderNo(110);

            View.AssociateChildrenProperty(EquipModelExtension.MaintainProjectListProperty, c =>
                 {
                     var arg = c as ChildPagingDataArgs;
                     var model = c.Parent as EquipModel;
                     if (model != null)
                     {
                         return RT.Service.Resolve<EquipController>().GetMaintainProjectsOfModels(model.Id, arg.PagingInfo, arg.SortInfo);
                     }
                     return new EntityList<EquipModelMaintainProject>();
                 }, ViewConfig.ListView).HasLabel("保养项目").Show(ChildShowInWhere.All).HasOrderNo(120);

            //润滑项目
            View.AssociateChildrenProperty(EquipModelExtension.LubricationProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipModel;

                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>().GetLubricationProjectList(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipModelLubricationProject>();
            }, ViewConfig.ListView).HasLabel("润滑项目").Show(ChildShowInWhere.All).HasOrderNo(131);


            View.AttachChildrenProperty(typeof(EquipBomDetail),
                (w) =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent as EquipModel;
                    if (parent == null)
                        return new EntityList<EquipBomDetail>();

                    return RT.Service.Resolve<EquipBomController>().GetEquipBomDetailsByModelId(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                }, EquipBomDetailViewConfig.AccountEquipBomDetailViewGroup).HasLabel("设备BOM").Show(ChildShowInWhere.All).HasOrderNo(140);

            View.AssociateChildrenProperty(EquipModelExtension.EquipModelRepairProjectListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipModel;
                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>().GetEquipModelRepairProjectList(model.Id, arg.PagingInfo, arg.SortInfo);
                }
                return new EntityList<EquipModelRepairProject>();
            }, ViewConfig.ListView).HasLabel("维修项目").Show(ChildShowInWhere.All).HasOrderNo(141);


            View.AssociateChildrenProperty(EquipModelExtension.EquipModelTechParameterListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var model = c.Parent as EquipModel;

                if (model != null)
                {
                    return RT.Service.Resolve<EquipController>().GetEquipModelTechParameterList(model.Id, arg.PagingInfo, null);
                }

                return new EntityList<EquipModelTechParameter>();
            }, ViewConfig.ListView).HasLabel("技术参数").Show(ChildShowInWhere.All).HasOrderNo(150);
            #endregion
        }

    }
}
