using SIE.EMS.AssetScraps;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetScraps
{
	/// <summary>
	/// 资产报废附件视图配置
	/// </summary>
	public class AssetScrapAttachmentViewConfig : WebViewConfig<AssetScrapAttachment>
	{
		/// <summary>
		/// 附件清单编辑视图
		/// </summary>
		public const string EditAssetScrapAttachmentViewGroup = "EditAssetScrapAttachmentViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetScrapAttachmentViewGroup });

			if (ViewGroup == EditAssetScrapAttachmentViewGroup)
			{
				ConfigEditAssetScrapAttachmentView();
			}
		}

		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{
			View.RemoveCommands(typeof(UploadAttachmentCommand).FullName);
			View.RemoveCommands(typeof(DeleteAttachmentCommand).FullName);
			View.RemoveCommands(typeof(ViewImageCommand).FullName);
		}

		///<summary>
		/// 配置附件清单编辑视图
		/// </summary>
		protected void ConfigEditAssetScrapAttachmentView()
		{
			View.UseDefaultCommands();
			View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, "SIE.Web.Core.Common.Commands.ImmediateDeleteCommand");
			View.RemoveCommands(typeof(ViewImageCommand).FullName);
		}
	}
}
