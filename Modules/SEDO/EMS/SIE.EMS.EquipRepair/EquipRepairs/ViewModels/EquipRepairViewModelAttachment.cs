using SIE.Common.Attachments;
using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
	/// 附件资料
	/// </summary>
	[ChildEntity, Serializable]
    [Label("附件")]
    public partial class EquipRepairViewModelAttachment : Attachment<EquipRepairViewModel>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EquipRepairEntityDataProvider))]
    public partial class EquipRepairViewModelAttachmentRepository : AttachmentRepository<EquipRepairViewModelAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class EquipRepairViewModelAttachmentConfig : AttachmentEntityConfig<EquipRepairViewModelAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EMS_EQUIP_REPAIR");
        }
    }
}
