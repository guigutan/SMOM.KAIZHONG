using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Web.AbnormalInfo.AbnormalMonitors.Commands;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常监控任务视图配置
	/// </summary>
	internal class AbnormalMonitorInventoryViewConfig : WebViewConfig<AbnormalMonitorInventory>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand(typeof(GeneralTask).FullName);
            View.DisableEditing();
            View.Property(p => p.Code);
            View.Property(p => p.AbnormalName);
            View.Property(p => p.AbnormalDefineId);
            View.Property(p => p.ProblemDescription);
        }


        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.AbnormalName);
            View.Property(p => p.AbnormalDefineId);
            View.Property(p => p.UpdateDate).HasLabel("更新时间").UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Week; });
        }
    }
}