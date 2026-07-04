using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Report.EquipCostAnalyses;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 设备成本分析-查询界面
    /// </summary>
    internal class EquipCostAnalyseCriteriaViewConfig : WebViewConfig<EquipCostAnalyseCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.ClearQuery);
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.EMS.Report.EquipmentMixReport.Commands.EmsMixReportQuery");
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipType).Cascade(p => p.EquipAccountId, null).Cascade(p => p.EquipModelId, null).Show();
                View.Property(p => p.EquipModel).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipCostAnalyseCriteria;
                    return RT.Service.Resolve<EquipController>().GetEquipModels(c, criteria.EquipTypeId, r);
                }).Cascade(p => p.EquipAccountId, null);
                View.Property(p => p.EquipAccount).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipCostAnalyseCriteria;
                    return RT.Service.Resolve<EquipController>().GetEquipAccounts(c, criteria.EquipModelId, criteria.EquipTypeId, r);
                }).UsePagingLookUpEditor((p, e) => {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipName), nameof(e.EquipAccount.Name));
                    p.DicLinkField = keyValues;
                }).Show();
                View.Property(p => p.EquipName).Show();
                View.Property(p => p.DepartmentId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var ctl = RT.Service.Resolve<EnterpriseController>();
                    return ctl.GetDepartmentsWithParent(pagingInfo, keyword);
                }).UsePagingLookUpEditor().Cascade(p => p.WorkShopId, null).Show();

                View.Property(p => p.WorkShop).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipCostAnalyseCriteria;
                    if (criteria == null)
                        return new EntityList<Enterprise>();
                    var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(c, r);
                    workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                    return workShopList;
                }).Show(ShowInWhere.All).HasLabel("车间").Cascade(p => p.ResourceId, null);
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as EquipCostAnalyseCriteria;
                    var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, criteria.WorkShopId);
                    resourcesList.ForEach(p => p.TreePId = null);
                    return resourcesList;
                }).HasLabel("产线").Show(ShowInWhere.All);

                View.Property(p => p.Year).UseYearEditor().DefaultValue(DateTime.Now).Show();
                View.Property(p => p.BeginMonth).DefaultValue(DateTime.Now.Month).Show();
                View.Property(p => p.EndMonth).DefaultValue(12).Show();
            }
        }
    }
}
