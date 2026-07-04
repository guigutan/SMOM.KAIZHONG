using DevExpress.XtraRichEdit.API.Native;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.RedCardManagment.RedCards; 

namespace SIE.Web.RedCardManagment.RedCards
{
	/// <summary>
	/// 红牌操作日志视图配置
	/// </summary>
	internal class RedCardLogViewConfig : WebViewConfig<RedCardLog>
	{		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.AssignAuthorize(typeof(RedCard));
			View.UseCommand(WebCommandNames.ExportXls); 
			View.Property(p => p.ItemCode).Readonly();
			View.Property(p => p.ItemName).Readonly();
			View.Property(p => p.SupplierCode).Readonly();
			View.Property(p => p.SupplierName).Readonly();
			View.Property(p => p.ItemBatch).Readonly();
			View.Property(p => p.ProductSn).Readonly();
			View.Property(p => p.ProductDateStart).Readonly();
			View.Property(p => p.ProductDateEnd).Readonly();
			View.Property(p => p.RedCardNo).Readonly();
			View.Property(p => p.SN).Readonly();
			View.Property(p => p.Status).Readonly();
			View.Property(p => p.ApplicantName).Readonly();
			View.Property(p => p.ApplyTime).Readonly();
		}

		protected override void ConfigQueryView()
		{
			base.ConfigQueryView();
            View.Property(p => p.RedCardNo);
            View.Property(p => p.Item).HasLabel("物料");
            View.Property(p => p.Supplier).HasLabel("供应商");
            View.Property(p => p.ItemBatch);
            View.Property(p => p.SN);
            View.Property(p => p.ApplyTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
        }


	}
}