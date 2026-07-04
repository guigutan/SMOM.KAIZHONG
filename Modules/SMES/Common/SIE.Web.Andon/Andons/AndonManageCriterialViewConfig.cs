using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Resources.Employees;
using SIE.Web.Equipments.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯管理查询视图配置
    /// </summary>
    public class AndonManageCriterialViewConfig : WebViewConfig<AndonManageCriterial>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AndonManage));
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonManageCode);
                View.Property(p => p.AndonManageClass);
                View.Property(p => p.AndonType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<AndonController>().GetAndonTypes(pagingInfo, keyword);
                });
                View.Property(p => p.Andon).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<AndonController>().GetAndonList(pagingInfo, keyword);
                });
                View.Property(p => p.MulitState).UseEnumMutilEditor(x => x.EnumType = typeof(AndonManageState)).Show(ShowInWhere.Detail);
                View.Property(p => p.Department).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<AndonManageController>().GetOrganizations(p, k);
                });
                View.Property(p => p.Trigger).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                });
                View.Property(p => p.Handler).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                });
                View.Property(p => p.Factory).UseFactoryEditor().Cascade(p => p.WorkShop, null);
                View.Property(p => p.WorkShop).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManageCriterial;
                    return RT.Service.Resolve<AndonManageController>().GetWorkShops(source, p, k);
                }).Cascade(p => p.WipResource, null);
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManageCriterial;
                    return RT.Service.Resolve<AndonManageController>().GetWipResources(source, p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.WipResourceName), nameof(e.WipResource.Name));
                    m.DicLinkField = keyValues;
                    m.BindDisplayField = AndonManageCriterial.WipResourceNameProperty.Name;
                });
                View.Property(p => p.LineStop);
                View.Property(p => p.AskMaterial);
                View.Property(p => p.CreateTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            }

        }
    }
}
