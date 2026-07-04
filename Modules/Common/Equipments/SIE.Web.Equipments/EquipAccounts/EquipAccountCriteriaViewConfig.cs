using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipTypes;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using SIE.Web.Common;
using SIE.Web.Resources;
using System.Linq;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备台账查询实体视图配置
    /// </summary>
    internal class EquipAccountCriteriaViewConfig : WebViewConfig<EquipAccountCriteria>
    {
        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.EquipOther).Show(ShowInWhere.All);
                View.Property(p => p.ModelCode).Show(ShowInWhere.All);
                View.Property(p => p.ModelName).Show(ShowInWhere.All);
                View.Property(p => p.EquipType).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<EquipTypeController>().GetEquipTypes(c, r);
                }).Show(ShowInWhere.All);
                View.Property(p => p.Factory).UseFactoryEditor()
                    .Cascade(p => p.WorkShopId, null).Cascade(p => p.ManageDeptId, null).Cascade(p => p.UseDeptId, null).Show(ShowInWhere.All);
                View.Property(p => p.UseDept).UseDataSource((e, c, r) =>
                {
                    var entity = e as EquipAccountCriteria;
                    return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentsWithFactoryOrAll(entity.FactoryId, c, r);

                }).Show(ShowInWhere.All);

                View.Property(p => p.ManageDept).UseDataSource((e, c, r) =>
                {
                    var entity = e as EquipAccountCriteria;
                    return RT.Service.Resolve<EnterpriseController>().GetAllDepartmentsWithFactoryOrAll(entity.FactoryId, c, r);
                }).Show(ShowInWhere.All);
                View.Property(p => p.WorkShop).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipAccountCriteria;
                    if (criteria == null)
                        return new EntityList<Enterprise>();
                    var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r, criteria.FactoryId);
                    workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                    return workShopList;
                }).Cascade(p => p.ResourceId, null).Show(ShowInWhere.All).HasLabel("车间");

                View.Property(p => p.Resource)
                    .UseDataSource((e, c, r) =>
                    {
                        var equipAccountCriteria = e as EquipAccountCriteria;
                        
                        if (equipAccountCriteria == null)
                        {
                            return new EntityList<Enterprise>();
                        }

                        var resourcesList = RT.Service.Resolve<EnterpriseController>()
                            .GetLines(c, r, equipAccountCriteria.ResourceId);

                        resourcesList.ForEach(p => p.TreePId = null);

                        return resourcesList;
                    }).UsePagingLookUpEditor()
                    .Show(ShowInWhere.All).HasLabel("产线");

                View.Property(p => p.Process).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ProcessController>().GetEmployeeProcessList(pagingInfo, keyword);
                })
                .Show(ShowInWhere.All).HasLabel("工序");

                View.Property(p => p.TypeCategory).UseCatalogEditor(p => { p.CatalogType = EquipType.EquipTypeCatalogType; p.CatalogReloadData = true; })
                    .Show(ShowInWhere.All).HasLabel("设备类别");

                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.AccountUseState).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
