using SIE.EMS.Report.EquipCostAnalyses;

namespace SIE.Web.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 设备成本分析-界面
    /// </summary>
    internal class EquipCostInfoViewConfig : WebViewConfig<EquipCostInfo>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            View.UseClientOrder();
            View.Property(p => p.EquipCode).ShowInList(130);
            View.Property(p => p.EquipName).ShowInList(130);
            View.Property(p => p.RepairCost).ShowInList(130);
            View.Property(p => p.MaintainCost).ShowInList(130);
            View.Property(p => p.OutsourceCost).ShowInList(130);
            View.Property(p => p.SparePartCost).ShowInList(130);
            View.Property(p => p.EnergyConsumptionCost).ShowInList(130);
            View.Property(p => p.DepreciationCost).ShowInList(130);
            View.Property(p => p.TotalWokerHourCost).ShowInList(140);
            View.Property(p => p.TotalCost).ShowInList(140);
        }
    }
}
