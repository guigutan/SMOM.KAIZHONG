using SIE.Domain;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands;
using System;
using System.Linq;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养计划查询实体视图配置
    /// </summary>
    internal class MaintainPlanCriteriaViewConfig : WebViewConfig<MaintainPlanCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            
            View.Property(p => p.EquipAccount);
            View.Property(p => p.MachineNo);
            View.Property(p => p.EquipModel);
            View.Property(p => p.EquipTypeCategory).UseCatalogEditor(c => { c.CatalogType = MaintainPlanCriteria.EquipTypeCatalogType; c.CatalogReloadData = true; });
            View.Property(p => p.Workshop).UseDataSource((e, c, r) =>
            {
                var criteria = e as MaintainPlanCriteria;
                if (criteria == null)
                    return new EntityList<Enterprise>();
                var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                return workShopList;
            }).Show(ShowInWhere.All).HasLabel("车间").Cascade(p=>p.WipResourceId,null);
            
            View.Property(p => p.WipResource).UseDataSource((e, c, r) =>
            {
                var criteria = e as MaintainPlanCriteria;
                var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, criteria.WorkshopId);
                resourcesList.ForEach(p => p.TreePId = null);
                return resourcesList;
            }).HasLabel("产线").Show(ShowInWhere.All);

            View.Property(p => p.IsShowChildEquip).Show(ShowInWhere.Hide);
            View.Property(p => p.UseState).Show(ShowInWhere.All);
            View.Property(p => p.ProjectCycle).Show(ShowInWhere.Hide);
            View.Property(p => p.Year).UseDateEditor(d => d.Format = "Y").DefaultValue(DateTime.Now.Year).Show(ShowInWhere.Hide);
        }
    }
}
