using SIE.Resources.Holidays;

namespace SIE.WPF.Resources.Holidays
{
    /// <summary>
    /// 法定假期视图配置
    /// </summary>
    internal class HolidayViewConfig : WPFViewConfig<Holiday>
	{
		/// <summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.HasDelegate(Holiday.RemarkProperty);
			View.UseDefaultBehaviors();
		}
		
		/// <summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.ListSave);
            View.Property(p => p.BeginDate).UseDateEditor();
			View.Property(p => p.EndDate).UseDateEditor();
            View.Property(p => p.Remark);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Remark).HasLabel("名称");
        }
    }
}
