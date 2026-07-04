using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量校验规程视图配置
    /// </summary>
    public class EquipAccountCalibrationViewConfig : WebViewConfig<EquipAccountCalibration>
	{
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {

            View.AddBehavior("SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Behaviors.EquipModelCalibrationChangedBehavior");
            View.UseCommands(typeof(SelEquipAccountCalibrationCommand).FullName, typeof(EquipAccountCalibrationSaveCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name).Readonly();
                View.Property(p => p.InspectionRuleType).Readonly();
                View.Property(p => p.CheckCategory).Readonly();
                View.Property(p => p.PeriodDays);
                View.Property(p => p.WarningPeriod);
                View.Property(p => p.PrevInspectionDate).UseDateEditor();
                View.Property(p => p.NextInspectionDate).UseDateEditor().Readonly();
            }
        }
    }
}