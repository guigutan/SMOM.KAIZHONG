using SIE.MetaModel.View;
using SIE.Core.QmsStaticConst; 

namespace SIE.Web.Core.QmsStaticConst
{
	/// <summary>
	/// K1视图配置
	/// </summary>
	internal class StaticConstK1ViewConfig : WebViewConfig<StaticConstK1>
	{
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseGridSelectionModel(isCheckboxmodel: true);
			View.AddBehavior("SIE.Web.Core.QmsStaticConst.Behaviors.StaticConstK1Behavior");
			View.WithoutPaging();
			View.InlineEdit();
			View.ClearCommands();
			View.UseCommands(WebCommandNames.Add, "SIE.Web.Core.QmsStaticConst.Commands.K1.DeleteCommand");
			View.Property(p => p.TestQty).UseSpinEditor(c=> { c.MinValue = 4;c.AllowNegative = false;c.AllowDecimals = false; }).DisableSort();
			View.Property(p => p.Value).UseSpinEditor(c => { c.MinValue = 0.0001; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.CreateByName).DisableSort();
            View.Property(p => p.CreateDate).DisableSort();
            View.Property(p => p.UpdateByName).DisableSort();
            View.Property(p => p.UpdateDate).DisableSort();
        }
	}
}