using SIE.MetaModel.View;
using SIE.RedCardManagment.RedCards;
using SIE.Web.RedCardManagment.RedCards.Commands;

namespace SIE.Web.RedCardManagment.RedCards
{
	/// <summary>
	/// 物料SN追溯清单视图配置
	/// </summary>
	internal class ItemSnRetroactiveViewConfig : WebViewConfig<ItemSnRetroactive>
	{
		///<summary>
		/// 配置视图
		/// </summary>
		protected override void ConfigView()
		{
			View.AssignAuthorize(typeof(ItemSnRetroactive));
		}
		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.UseCommands(
                "SIE.Web.RedCardManagment.RedCards.Commands.ItemSnRelatedProductsCommand",
                "SIE.Web.RedCardManagment.RedCards.Commands.ItemSnEnableRedCardCommand",
                "SIE.Web.RedCardManagment.RedCards.Commands.ItemSnDisableRedCardCommand", 
				WebCommandNames.ExportXls,
				typeof(ImportRedCardItemSnCommand).FullName
			);
			View.UseGridSelectionModel();
			View.Property(p => p.SN).Readonly();
			View.Property(p => p.Quannity).Readonly();
			View.Property(p => p.Status).Readonly();
			View.Property(p => p.ApplicantName).Readonly();
			View.Property(p => p.ApplyTime).Readonly();
		}

		protected override void ConfigImportView()
		{
			View.Property(p => p.SN);
			View.Property(p => p.Quannity);

        }

	}
}