using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
	/// 设备台账工序视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipAccountProcessViewConfig : WebViewConfig<EquipAccountProcess>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(SelAccountProcessCommand).FullName, WebCommandNames.Delete);
            View.Property(p => p.ProcessId).Readonly();
        }
    }
}
