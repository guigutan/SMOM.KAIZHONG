using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC
{
    /// <summary>
    /// 装箱QC附件
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(FileName))]
    [Label("装箱QC附件")]
    public class PackingQcAttachment :Attachment<PackingQc>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(MesCoreEntityDataProvider))]
    public partial class PackingQcAttachmentRepository : AttachmentRepository<PackingQcAttachment>
    {
    }

    /// <summary>
    /// 附件 实体配置
    /// </summary>
    public class PackingQcAttachmentConfig : AttachmentEntityConfig<PackingQcAttachment>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("PACKINGQC_ATTACHMENT");
        }
    }
}
