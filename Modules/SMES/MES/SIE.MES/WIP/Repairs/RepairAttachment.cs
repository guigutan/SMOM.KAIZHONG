using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MES;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修采集附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("维修采集附件")]
    public partial class RepairAttachment : Attachment<RepairMainRecord>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(MesCoreEntityDataProvider))]
    public partial class RepairAttachmentRepository : AttachmentRepository<RepairAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class RepairAttachmentConfig : AttachmentEntityConfig<RepairAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("WIP_REP_MAIN_RECORD");
        }
    }
}
