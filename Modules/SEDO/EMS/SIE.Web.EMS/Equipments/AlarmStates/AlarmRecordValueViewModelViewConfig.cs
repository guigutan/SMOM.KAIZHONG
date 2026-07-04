using SIE.EMS.Equipments.AlarmStates;

namespace SIE.Web.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 设备报警记录值
    /// </summary>
    public class AlarmRecordValueViewModelViewConfig : WebViewConfig<AlarmRecordValueViewModel>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Equipments.AlarmStates.Scripts.AlarmRecordValueBehavior");
            View.UseCommands("SIE.Web.EMS.Equipments.AlarmStates.Commands.ViewChartCommand");
            View.Property(p => p.FullTagName).Show(ShowInWhere.Hide); 
            View.Property(p => p.MDCVariableName); 
            View.Property(p => p.PararCode);
            View.Property(p => p.ParaName);
            View.Property(p => p.AlarmValue);
            View.Property(p => p.RecoveryValue);
            View.Property(p => p.IsShowInChart);
        }
    }
}
