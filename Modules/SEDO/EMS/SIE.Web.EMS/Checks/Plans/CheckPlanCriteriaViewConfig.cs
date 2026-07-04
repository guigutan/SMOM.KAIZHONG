using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using SIE.Web.Core;
using SIE.Web.EMS.Checks.Plans.Commands;
using System;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划查询实体视图配置
    /// </summary>
    internal class CheckPlanCriteriaViewConfig : WebViewConfig<CheckPlanCriteria>
    {

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.AddBehavior("SIE.Web.EMS.Checks.Plans.Scripts.CheckPlanCriteriaBahavior");
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.EMS.Checks.Plans.Commands.QueryCheckPlanCommand");
            View.RemoveCommands(WebCommandNames.ClearQuery);
            View.Property(p => p.EquipAccount).Cascade(p => p.EquipModelId, null);
            View.Property(p => p.EquipAccountName);
            View.Property(p => p.EquipModel).UseDataSource((e, p, k) =>
            {
                var equipmentAccount = e as CheckPlanCriteria;
                if (equipmentAccount == null)
                {
                    return new EntityList<EquipModel>();
                }

                return RT.Service.Resolve<SIE.EMS.Equipments.EquipController>()
                      .GetEquipModelsOfUserHasPermission(p, k);
            });
            View.Property(p => p.TypeCategory).UseCatalogEditor(c => { c.CatalogType = CheckPlanCriteria.EquipTypeCatalogType; c.CatalogReloadData = true; });
            View.Property(p => p.Month).UseYearMonthEditor().DefaultValue(DateTime.Now);
            View.Property(p => p.Workshop).UseDataSource((e, c, r) =>
            {
                var criteria = e as CheckPlanCriteria;
                if (criteria == null)
                    return new EntityList<Enterprise>();
                var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                return workShopList;
            }).Show(ShowInWhere.All).HasLabel("车间").Cascade(p => p.WipResourceId, null);
            View.Property(p => p.WipResource).UseDataSource((e, c, r) =>
            {
                var criteria = e as CheckPlanCriteria;
                var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, criteria.WorkshopId);
                resourcesList.ForEach(p => p.TreePId = null);
                return resourcesList;
            }).HasLabel("产线").Show(ShowInWhere.All);
            View.Property(p => p.State);
            View.Property(p => p.UseState);
            View.Property(p => p.ManageDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword);
                if (departments == null || departments.Count <= 0)
                    return new EntityList<Enterprise>();
                departments.ForEach(p => p.TreePId = null);
                return departments;
            }).UsePagingLookUpEditor().Show();

            View.Property(p => p.EquipCode).Show(ShowInWhere.Hide);
            View.Property(p => p.MachineNo).Show(ShowInWhere.Hide);

            View.Property(p => p.Process).Show(ShowInWhere.Hide);
            View.Property(p => p.CheckCycleType).Show(ShowInWhere.Hide);

        }
    }
}
