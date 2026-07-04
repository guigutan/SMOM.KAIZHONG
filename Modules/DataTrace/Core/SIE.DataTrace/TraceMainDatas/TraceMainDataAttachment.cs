using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯主数据附件
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(FileName))]
    [Label("追溯主数据附件")]
    public partial class TraceMainDataAttachment : Attachment<TraceMainData>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(DataTraceEntityDataProvider))]
    public partial class TraceMainDataAttachmentRepository : AttachmentRepository<TraceMainDataAttachment>
    {
    }

    /// <summary>
    /// 附件 实体配置
    /// </summary>
    internal class TraceMainDataAttachmentConfig : AttachmentEntityConfig<TraceMainDataAttachment>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("TraceMainData");
        }
    }
}