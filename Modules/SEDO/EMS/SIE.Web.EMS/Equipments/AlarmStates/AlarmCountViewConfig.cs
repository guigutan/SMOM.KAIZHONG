using SIE.EMS.Equipments.AlarmStates;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 报警统计视图配置
    /// </summary>
    internal class AlarmCountViewConfig : WebViewConfig<AlarmCount>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(AlarmCount));
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.AlarmEquipAccountCode);
                View.Property(p => p.AlarmEquipAccountName);
                View.Property(p => p.AlarmEquipModelCode);
                View.Property(p => p.AlarmEquipModelName);
                View.Property(p => p.AlarmEquipTypeeCode);
                View.Property(p => p.AlarmEquipTypeeName);
                View.Property(p => p.AlarmSum);
                View.Property(p => p.Serious);
                View.Property(p => p.Major);
                View.Property(p => p.Medium);
                View.Property(p => p.Minor);
                View.Property(p => p.Info);
                View.Property(p => p.Alarm);
                View.Property(p => p.Close);
            }
        }
    }
}