using SIE.EMS.AssetRequisitions;
using SIE.MetaModel.View;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.EMS.AssetRequisitions.Commands;

namespace SIE.Web.EMS.AssetRequisitions
{
	/// <summary>
	/// 资产领用附件视图配置
	/// </summary>
	internal class AssetRequisitionAttachmentViewConfig : WebViewConfig<AssetRequisitionAttachment>
	{
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, typeof(AssetRequisitionAttachmentDeleteCommand).FullName);
			View.RemoveCommands(typeof(ViewImageCommand).FullName);
		}
	}
}