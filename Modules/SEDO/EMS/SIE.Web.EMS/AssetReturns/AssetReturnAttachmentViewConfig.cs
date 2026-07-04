using SIE.EMS.AssetReturns;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetReturns
{
	/// <summary>
	/// 资产归还附件视图配置
	/// </summary>
	public class AssetReturnAttachmentViewConfig : WebViewConfig<AssetReturnAttachment>
	{
		/// <summary>
		/// 附件清单编辑视图
		/// </summary>
		public const string EditAssetReturnAttachmentViewGroup = "EditAssetReturnAttachmentViewGroup";

		/// <summary>
		/// 配置视图属性
		/// </summary>
		protected override void ConfigView()
		{
			View.DeclareExtendViewGroup(new string[] { EditAssetReturnAttachmentViewGroup });

			if (ViewGroup == EditAssetReturnAttachmentViewGroup)
			{
				ConfigEditAssetReturnAttachmentView();
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
			View.Property(p => p.EquipmentCodes).Readonly().Show().HasOrderNo(4);
			View.Property(p => p.FixtureCodes).Readonly().Show().HasOrderNo(4);
			View.Property(p => p.MoldCodes).Readonly().Show(ShowInWhere.Hide).HasOrderNo(4);
		}

		///<summary>
		/// 配置附件清单编辑视图
		/// </summary>
		protected void ConfigEditAssetReturnAttachmentView()
		{
			View.UseDefaultCommands();
			View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, "SIE.Web.Core.Common.Commands.ImmediateDeleteCommand");
			View.RemoveCommands(typeof(ViewImageCommand).FullName);
			View.Property(p => p.EquipmentCodes).UsePagingLookUpGridPopupEditor(p =>
			{
				p.Model = typeof(AssetReturnEquipment).FullName;
				p.XType = "mutilIssueEquipEditor";
				p.DisplayField = "EquipAccountCode";
				Dictionary<string, string> dic = new Dictionary<string, string>();
				dic.Add(AssetReturnAttachment.EquipmentCodesProperty.Name, AssetReturnEquipment.EquipAccountCodeProperty.Name);
				p.MutiLinkField = dic.ToJsonString();
			}).Show().HasOrderNo(4);
			View.Property(p => p.FixtureCodes).UsePagingLookUpGridPopupEditor(p =>
			{
				p.Model = typeof(AssetReturnFixture).FullName;
				p.XType = "mutilIssueFixtureEditor";
				p.DisplayField = "FixtureEncode";
				Dictionary<string, string> dic = new Dictionary<string, string>();
				dic.Add(AssetReturnAttachment.FixtureCodesProperty.Name, AssetReturnFixture.FixtureEncodeProperty.Name);
				p.MutiLinkField = dic.ToJsonString();
			}).Show().HasOrderNo(4);
			View.Property(p => p.MoldCodes).Readonly().Show(ShowInWhere.Hide).HasOrderNo(4);
		}
	}
}
