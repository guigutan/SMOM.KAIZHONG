using SIE.Warehouses;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 库区基本资料视图配置
    /// </summary>
    internal class StorageAreaInfoViewConfig : WPFViewConfig<StorageAreaInfo>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.Area);
            View.Property(p => p.Volume);
            View.Property(p => p.StartPointX);
            View.Property(p => p.StartPointY);
        }

       
    }
}
