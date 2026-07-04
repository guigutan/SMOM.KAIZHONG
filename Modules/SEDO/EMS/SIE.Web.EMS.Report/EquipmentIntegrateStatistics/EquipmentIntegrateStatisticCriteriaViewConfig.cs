using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Report.EquipmentIntegrateStatistics;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// ESD分析查询视图配置
    /// </summary>
    public class EquipmentIntegrateStatisticCriteriaViewConfig : WebViewConfig<EquipmentIntegrateStatisticCriteria>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.ClearQuery);
            View.ReplaceCommands(WebCommandNames.ExecuteQuery,
                "SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Commands.EquipIntStatisticsQuery");
           
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipType).Cascade(p => p.EquipCodeId, null).Cascade(p => p.EquipModelId, null).Show();
                View.Property(p => p.EquipModel).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipmentIntegrateStatisticCriteria;
                    return RT.Service.Resolve<EquipController>().GetEquipModels(c, criteria.EquipTypeId, r);
                }).Cascade(p => p.EquipCodeId, null);
                View.Property(p => p.EquipCode).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipmentIntegrateStatisticCriteria;
                    return RT.Service.Resolve<EquipController>().GetEquipAccounts(c, criteria.EquipModelId, criteria.EquipTypeId, r);
                }).UsePagingLookUpEditor((p, e) => {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipName), nameof(e.EquipCode.Name));
                    p.DicLinkField = keyValues;
                }).Show();
                View.Property(p => p.EquipName).Show();
                
                View.Property(p => p.UseDepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var ctl = RT.Service.Resolve<EnterpriseController>();
                    return ctl.GetDepartmentsWithParent(pagingInfo, keyword);
                }).UsePagingLookUpEditor().Cascade(p => p.WorkShopId, null).Show();

                View.Property(p => p.WorkShop).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipmentIntegrateStatisticCriteria;
                    if (criteria == null)
                        return new EntityList<Enterprise>();
                    var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                    workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                    return workShopList;
                }).Show(ShowInWhere.All).HasLabel("车间").Cascade(p => p.WipResourceId, null);
                View.Property(p => p.WipResource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipmentIntegrateStatisticCriteria;
                    var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, criteria.WorkShopId);
                    resourcesList.ForEach(p => p.TreePId = null);
                    return resourcesList;
                }).HasLabel("产线").Show(ShowInWhere.All);
                View.Property(p => p.Year).HasLabel("年份").UseYearEditor().DefaultValue(DateTime.Now.Date).Show();
                View.Property(p => p.Month).DefaultValue(DateTime.Now.Month).Show();
                View.Property(p => p.UtilizationRate).UseSpinEditor(p => { p.MinValue = 0; }).DefaultValue(100).Show();
            }
        }
    }
}
