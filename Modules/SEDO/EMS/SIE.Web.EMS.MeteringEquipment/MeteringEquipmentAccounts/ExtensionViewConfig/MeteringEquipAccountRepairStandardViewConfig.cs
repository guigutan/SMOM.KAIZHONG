using SIE.EMS.Enums;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel.View;
using SIE.Web.EMS.Equipments.Accounts.Commands;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.ExtensionViewConfig
{
    /// <summary>
    /// 维修定标视图
    /// </summary>
    public class MeteringEquipAccountRepairStandardViewConfig : WebViewConfig<MeteringEquipAccountRepairStandard>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Equipments.Accounts.Scripts.RunStandardsValueListBehavior");
            View.UseCommands(typeof(SelRunStandardValueCommand).FullName, WebCommandNames.Delete,typeof(SaveRunStandardValueCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.StandardType).ShowInList(80).Readonly();
                View.Property(p => p.StandardUnit).ShowInList(80).Readonly();
                View.Property(p => p.Amount).ShowInList(80).UseSpinEditor(m => m.MinValue = 0).Readonly();
                View.Property(p => p.RoundAmount).ShowInList(80).Readonly(p => p.StandardType == StandardType.Period).UseSpinEditor(m => m.MinValue = 0);
                View.Property(p => p.TotalAmount).ShowInList(80).Readonly().UseSpinEditor(m => m.MinValue = 0);
                View.Property(p => p.LastExecuteDate).UseDateEditor().ShowInList(130).Readonly(p => p.StandardType != StandardType.Period);
                View.Property(p => p.NextExecuteDate).UseDateEditor().ShowInList(130).Readonly();
                View.Property(p => p.LeadTime).ShowInList(100);
                View.Property(p => p.RunStandardNo).ShowInList(80).Readonly();
            }
        }
    }
}
