using SIE.MES.LoadItems;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 下料缺陷视图配置
    /// </summary>
    internal class UnloadItemDefectViewConfig : WPFViewConfig<UnloadItemDefect>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Qty).Show(ShowInWhere.All);
            }
        }
    }
}
