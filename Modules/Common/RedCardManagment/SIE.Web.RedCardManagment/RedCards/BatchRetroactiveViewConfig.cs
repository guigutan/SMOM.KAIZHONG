using SIE.MetaModel.View;
using SIE.RedCardManagment.RedCards; 

namespace SIE.Web.RedCardManagment.RedCards
{
	/// <summary>
	/// 物料批次追溯清单视图配置
	/// </summary>
	internal class BatchRetroactiveViewConfig : WebViewConfig<BatchRetroactive>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
            View.AssignAuthorize(typeof(BatchRetroactive));
        }
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseCommands(
				"SIE.Web.RedCardManagment.RedCards.Commands.BatchRelatedProductsCommand",
                "SIE.Web.RedCardManagment.RedCards.Commands.BatchEnableRedCardCommand",
                "SIE.Web.RedCardManagment.RedCards.Commands.BatchDisableRedCardCommand",
                WebCommandNames.ExportXls
			);
			View.UseGridSelectionModel();
			View.Property(p => p.ItemBatch).Readonly();
			View.Property(p => p.Batch).Readonly();
			View.Property(p => p.Quannity).Readonly();
			View.Property(p => p.Status).Readonly();
			View.Property(p => p.ApplicantName).Readonly();
			View.Property(p => p.ApplyTime).Readonly();
		}
	}
}