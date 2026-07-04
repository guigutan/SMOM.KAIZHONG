using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.SpecialEquipment.Commands;

namespace SIE.Web.EMS.SpecialEquipment.SpecialEquipmentAcounts
{
    /// <summary>
    /// 设备定检规程视图配置
    /// </summary>
    internal class EquipAccountRegularInspectionViewConfig : WebViewConfig<EquipAccountRegularInspection>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelEquipAccountInspectionCommand).FullName, WebCommandNames.Save, typeof(ImmediateDeleteCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name).Readonly();
                View.Property(p => p.InspectionRuleType).Readonly();
                View.Property(p => p.CheckCategory).Readonly();
                View.Property(p => p.PeriodDays);
                View.Property(p => p.WarningPeriod);
                View.Property(p => p.FirstInspectionDate).UseDateEditor().Readonly(p => p.NextInspectionDate != null);
                View.Property(p => p.PrevInspectionDate).UseDateEditor().Readonly();
                View.Property(p => p.NextInspectionDate).UseDateEditor().Readonly();
            }
        }
    }
}