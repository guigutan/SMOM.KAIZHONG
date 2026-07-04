using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts
{
    /// <summary>
	/// 附件资料
	/// </summary>
	[ChildEntity, Serializable]
    [Label("图片")]
    public partial class SparePartPictureAttachment : Attachment<SparePart>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class SparePartPictureAttachmentRepository : AttachmentRepository<SparePartPictureAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class SparePartPictureAttachmentConfig : AttachmentEntityConfig<SparePartPictureAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_SPARE_PART_PIC");
        }
    }
}
