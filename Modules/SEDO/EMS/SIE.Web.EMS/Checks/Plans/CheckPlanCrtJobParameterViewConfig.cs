using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划自动生成调度参数视图配置
    /// </summary>
    public class CheckPlanCrtJobParameterViewConfig : WebViewConfig<CheckPlanCrtJobParameter>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Factory).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(p, k);
                    foreach (var item in list)
                    {
                        item.TreePId = null;
                    }
                    return list;
                }).Cascade(p => p.UseDpt, null).Cascade(p => p.WorkShop, null).Show();
                View.Property(p => p.UseDpt).UseDataSource((e, p, k) =>
                {
                    var entity = e as CheckPlanCrtJobParameter;
                    if (entity.FactoryId != null && entity.FactoryId != 0)
                    {
                        var list = RT.Service.Resolve<EnterpriseController>().GetAllDepartmentWithFactory(entity.FactoryId, p, k);
                        return list;
                    }
                    else
                    {
                        var list = RT.Service.Resolve<EnterpriseController>().GetDepartments(p, k);
                        foreach (var item in list)
                        {
                            item.TreePId = null;
                        }
                        return list;
                    }
                }).Show();
                View.Property(p => p.WorkShop).UseDataSource((e, p, k) =>
                {
                    var entity = e as CheckPlanCrtJobParameter;
                    return RT.Service.Resolve<EnterpriseController>().GetWorkShops(p, k, entity.FactoryId);
                }).Cascade(p => p.Resource, null).Show();
                View.Property(p => p.Resource).UseDataSource((e, p, k) =>
                {
                    var entity = e as CheckPlanCrtJobParameter;
                    List<double?> workshopIds = new List<double?>();
                    if (entity != null && entity.WorkShopId != null && entity.WorkShopId != 0)
                    {
                        workshopIds.Add(entity.WorkShopId);
                    }
                    return RT.Service.Resolve<WipResourceController>().GetWipResourcesByWorkShopId(new List<ResourceState>() {
                         ResourceState.Actived,
                        }, workshopIds, new List<SyncSourceType>() {
                         SyncSourceType.Enterprise
                        }, p, k);
                }).Show();
            }
        }
    }
}
