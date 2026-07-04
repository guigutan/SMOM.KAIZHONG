using SIE.Kit.MES.CallMaterials;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
	/// 排序优先级设置视图配置
	/// </summary>
	internal class PrioritySettingViewConfig : WebViewConfig<PrioritySetting>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Condition);
            View.Property(p => p.Priority);
            View.Property(p => p.SortMode);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            //方法重写
        }
    }
}