using SIE.MetaModel.View;
using SIE.Core.QmsStaticConst; 

namespace SIE.Web.Core.QmsStaticConst
{
	/// <summary>
	/// 控制图参数视图配置
	/// </summary>
	internal class ControlChartConstViewConfig : WebViewConfig<ControlChartConst>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseGridSelectionModel(isCheckboxmodel: true);
			View.AddBehavior("SIE.Web.Core.QmsStaticConst.Behaviors.ControlChartConstBehavior");
			View.WithoutPaging();
			View.InlineEdit();
			View.ClearCommands();
			View.UseCommands(WebCommandNames.Add, "SIE.Web.Core.QmsStaticConst.Commands.ControlChartConst.DeleteCommand");
			View.Property(p => p.SampleQty).UseSpinEditor(c => { c.MinValue = 26; c.AllowNegative = false; c.AllowDecimals = false; }).DisableSort(); 
			View.Property(p => p.A).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
			View.Property(p => p.A2).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.A3).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.B3).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.B4).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.B5).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.B6).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.C4).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();     
			View.Property(p => p.D1).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
			View.Property(p => p.D2).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();	
			View.Property(p => p.D3).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
			View.Property(p => p.D4).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.D2Nd).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.D3Nd).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.D4Nd).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();		
			View.Property(p => p.E2).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.MeA2).UseSpinEditor(c => { c.MinValue = 0; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 4; }).DisableSort();
            View.Property(p => p.CreateByName).DisableSort();
			View.Property(p => p.CreateDate).DisableSort();
			View.Property(p => p.UpdateByName).DisableSort();
			View.Property(p => p.UpdateDate).DisableSort();
		}
	}
}