using SIE.WorkBenchCommon.Workbench.Concerns;

namespace SIE.Wpf.WorkBenchCommon.Workbench.Concerns
{
    /// <summary>
    /// 关注信息视图配置
    /// </summary>
    internal class ConcernsInfoViewConfig : WPFViewConfig<ConcernsInfo>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.HasDelegate(ConcernsInfo.NameProperty);
			View.UseDefaultBehaviors();
		}
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseDefaultCommands(); 
			View.Property(p => p.Name);
			View.Property(p => p.Type);
			View.Property(p => p.Arguments);
		}

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
        }
    }
}
