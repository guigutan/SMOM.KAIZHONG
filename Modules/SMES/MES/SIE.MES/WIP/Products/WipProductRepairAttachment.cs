using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品维修记录附件")]
    public partial class WipProductRepairAttachment : Attachment<WipProductRepair>
    {
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(MesCoreEntityDataProvider))]
    public partial class RepairAttachmentRepository : AttachmentRepository<WipProductRepairAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class RepairAttachmentConfig : AttachmentEntityConfig<WipProductRepairAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("WIP_PROD_REP");
        }
    }
}
