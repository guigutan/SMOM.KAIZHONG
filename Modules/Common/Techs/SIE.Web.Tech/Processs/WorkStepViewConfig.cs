using SIE.MetaModel.View;
using SIE.Tech.Processs;

namespace SIE.Web.Tech.Processs
{
	/// <summary>
	/// 工步视图配置
	/// </summary>
	public class WorkStepViewConfig : WebViewConfig<WorkStep>
	{
		/// <summary>
		/// 工艺路线维护工序的工步视图配置
		/// </summary>
		public const string WorkStepView = "WorkStepView";

		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.HasDelegate(WorkStep.NameProperty);
			View.UseDefaultCommands();
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.InlineEdit();
			View.ClearCommands();
			View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
			using (View.OrderProperties())
			{
				View.Property(p => p.Code);
				View.Property(p => p.Name);
				View.Property(p => p.SeqNumber);
			}
		}
		///<summary>
		/// 配置下拉视图
		/// </summary>
		protected override void ConfigSelectionView()
		{
			View.Property(p => p.Code);
			View.Property(p => p.Name);
			View.Property(p => p.SeqNumber);
		}
	}
}