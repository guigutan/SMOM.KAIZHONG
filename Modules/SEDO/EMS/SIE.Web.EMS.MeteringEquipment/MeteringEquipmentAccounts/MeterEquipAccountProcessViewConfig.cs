using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
	/// 设备台账工序视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class MeterEquipAccountProcessViewConfig : WebViewConfig<MeterEquipAccountProcess>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelMeterAccountProcessCommand).FullName, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.ProcessId);
        }
    }
}
