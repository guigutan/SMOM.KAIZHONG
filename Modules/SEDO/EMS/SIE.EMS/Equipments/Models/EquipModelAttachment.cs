using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using System;

namespace SIE.EMS.Equipments.Models
{
    /// <summary>
    /// 设备型号对应附件资料
    /// </summary>
    [ChildEntity, Serializable]
    public class EquipModelAttachment : Attachment<EquipModel>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class EquipModelAttachmentRepository : AttachmentRepository<EquipModelAttachment>
    {
    }

    /// <summary>
    /// 附件 实体配置
    /// </summary>
    internal class EquipModelAttachmentConfig : AttachmentEntityConfig<EquipModelAttachment>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_EQUIP_MODEL");
        }
    }
}
