using SIE.Resources.ShiftTypes;

namespace SIE.Wpf.Resources.ShiftTypes
{
    /// <summary>
    /// 班制时间明细视图配置
    /// </summary>
    internal class ShiftDetailViewConfig : WPFViewConfig<ShiftDetail>
	{
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
		{
			View.UseDefaultCommands().RemoveCommands(WPFCommandNames.ListSave);

			using (View.OrderProperties())
			{
				View.Property(p => p.BeginTime).Show(ShowInWhere.All);
				View.Property(p => p.EndTime).Show(ShowInWhere.All);
			}
		}
	}
}
