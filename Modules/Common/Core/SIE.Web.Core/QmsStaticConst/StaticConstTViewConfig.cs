using SIE.Core.QmsStaticConst; 

namespace SIE.Web.Core.QmsStaticConst
{
	/// <summary>
	/// t值视图配置
	/// </summary>
	internal class StaticConstTViewConfig : WebViewConfig<StaticConstT>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseGridSelectionModel(isCheckboxmodel: true, checkOnly: false, injectCheckbox: 0);
			View.WithoutPaging();
			View.UseClientOrder();
			View.ClearCommands();
			View.AddBehavior("SIE.Web.Core.QmsStaticConst.Behaviors.StaticConstTBehavior");
			View.UseCommands("SIE.Web.Core.QmsStaticConst.Commands.ConstT.AddRow", "SIE.Web.Core.QmsStaticConst.Commands.ConstT.DeleteRow", "SIE.Web.Core.QmsStaticConst.Commands.ConstT.AddColumn", "SIE.Web.Core.QmsStaticConst.Commands.ConstT.DeleteColumn");
			View.Property(p => p.Alpha).DisableSort();
			View.Property(p => p.SampleQty).DisableSort();
			View.Property(p => p.Value).DisableSort();
            View.Property(p => p.CreateByName).DisableSort();
            View.Property(p => p.CreateDate).DisableSort();
            View.Property(p => p.UpdateByName).DisableSort();
            View.Property(p => p.UpdateDate).DisableSort();
        }
	}
}