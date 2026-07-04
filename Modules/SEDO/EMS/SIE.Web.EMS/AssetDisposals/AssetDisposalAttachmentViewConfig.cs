using SIE.EMS.AssetDisposals;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetDisposals
{
	/// <summary>
	/// 资产处置附件视图配置
	/// </summary>
	public class AssetDisposalAttachmentViewConfig : WebViewConfig<AssetDisposalAttachment>
	{
		/// <summary>
		/// 附件清单编辑视图
		/// </summary>
		public const string EditAssetDisposalAttachmentViewGroup = "EditAssetDisposalAttachmentViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetDisposalAttachmentViewGroup });

			if (ViewGroup == EditAssetDisposalAttachmentViewGroup)
			{
				ConfigEditAssetDisposalAttachmentView();
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
		protected void ConfigEditAssetDisposalAttachmentView()
		{
			View.UseDefaultCommands();
			View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, "SIE.Web.Core.Common.Commands.ImmediateDeleteCommand");
			View.RemoveCommands(typeof(ViewImageCommand).FullName);
		}
	}
}
