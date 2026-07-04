using SIE.Inventory.Piles; 

namespace SIE.WPF.Inventory.Piles
{
	/// <summary>
	/// 垛明细视图配置
	/// </summary>
	internal class PileDetailViewConfig : WebViewConfig<PileDetail>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.DisableEditing();
			View.Property(p => p.Sn);
			View.Property(p => p.ItemExtProp);
			View.Property(p => p.ItemExtPropName);
			View.Property(p => p.StorerCode);
			View.Property(p => p.ProjectNo);
			View.Property(p => p.TaskNo);
			View.Property(p => p.Qty);
			View.Property(p => p.Item);
			View.Property(p => p.OnhandState);
			View.Property(p => p.Lot);
			View.Property(p => p.ItemState);
		} 
	}
}