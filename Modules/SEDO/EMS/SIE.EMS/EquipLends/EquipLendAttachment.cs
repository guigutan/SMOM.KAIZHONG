using SIE.Common.Attachments;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SIE.EMS.EquipLends
{
    /// <summary>
    /// 设备借还管理附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("附件")]
    public class EquipLendAttachment : Attachment<EquipLendManage>
    {
    }

    /// <summary>
    /// 数据仓库
    /// </summary>
    public class EquipLendAttachmentRepository : AttachmentRepository<EquipLendAttachment>
    {

    }

    /// <summary>
    /// 实体元配置
    /// </summary>
    public class EquipLendAttachmentConfig : AttachmentEntityConfig<EquipLendAttachment>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("EQUIP_LEND_MANAGE");
        }
    }
}
