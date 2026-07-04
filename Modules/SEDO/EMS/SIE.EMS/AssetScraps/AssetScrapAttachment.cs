using SIE.Common.Attachments;
using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AssetScraps
{
	/// <summary>
	/// 资产报废附件
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("附件")]
	public partial class AssetScrapAttachment : Attachment<AssetScrap>
	{
		
	}

	/// <summary>
	///  仓库
	/// </summary>
	[DataProvider(typeof(EmsEntityDataProvider))]
	public partial class AssetScrapAttachmentRepository : AttachmentRepository<AssetScrapAttachment>
	{
	}

	/// <summary>
	/// 资产报废附件 实体配置
	/// </summary>
	internal class AssetScrapAttachmentConfig : AttachmentEntityConfig<AssetScrapAttachment>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			base.ConfigMeta();			
			Meta.EnableDiscriminator("EMS_ASET_SCRP");
		}
	}
}