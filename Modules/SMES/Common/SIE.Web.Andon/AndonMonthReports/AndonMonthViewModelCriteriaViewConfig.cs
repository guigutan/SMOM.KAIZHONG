using SIE.Andon.AndonMonthReports;
using SIE.Andon.Andons;
using SIE.Andon.AndonStatisticsReports;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Core;
using SIE.Web.Resources;
using System;
using System.Linq;

namespace SIE.Web.Andon.AndonMonthReports
{
    /// <summary>
    ///安灯统计查询视图配置
    /// </summary>
    public class AndonMonthViewModelCriteriaViewConfig : WebViewConfig<AndonMonthViewModelCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.ClearQuery);
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.Andon.AndonMonthReports.Commands.AndonMonthReportQuery");
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonClass).Show();
                View.Property(p => p.AndonType).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<AndonController>().GetAndonTypes(pagingInfo, keyword);
                });
                View.Property(p => p.AndonNameId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<AndonController>().GetAndonList(pagingInfo, keyword);
                });

                View.Property(p => p.FactoryId).UseFactoryEditor(p => p.DisplayField = nameof(Enterprise.Name)).Show();
                View.Property(p => p.WorkShop).UseDataSource((e, c, r) =>
                {
                    var criteria = e as AndonStatisticsViewModelCriteria;
                    if (criteria == null)
                        return new EntityList<Enterprise>();
                    var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                    workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                    return workShopList;
                }).Show(ShowInWhere.All).HasLabel("车间").Cascade(p => p.WipResourceId, null);
                View.Property(p => p.WipResourceId).UseDataSource((e, c, r) =>
                {
                    var resourcesList = RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(c, r);
                    resourcesList.ForEach(p => p.TreePId = null);
                    return resourcesList;
                }).HasLabel("产线").Show(ShowInWhere.All);

                View.Property(p => p.EquipAccount).Show();
                View.Property(p => p.Department).UseDataSource((e, p, k) =>
                 {
                     return RT.Service.Resolve<AndonController>().GetOrganizations(p, k);
                 }).HasLabel("责任部门");

                View.Property(p => p.GroupLevel).DefaultValue(GroupLevel.Workshop).Show();
                View.Property(p => p.SummaryDimension).DefaultValue(SummaryDimension.AndonClass).Show();
                 View.Property(p => p.CreateTime).UseYearEditor().DefaultValue(DateTime.Now).Show();

            }
        }
    }
}
