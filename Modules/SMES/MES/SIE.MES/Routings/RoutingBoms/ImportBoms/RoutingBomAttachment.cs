using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Routings.RoutingBoms.ImportBoms
{
    /// <summary>
    /// 导入日志附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("导入日志附件")]
    public partial class RoutingBomAttachment : Attachment<RoutingBomDetail>
    {
    }

    ///<summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(MesCoreEntityDataProvider))]
    public partial class RoutingBomAttachmentRepository : AttachmentRepository<RoutingBomAttachment>
    {
    }

    /// <summary>
    /// 导入日志附件 实体配置
    /// </summary>
    internal class RoutingBomAttachmentConfig : AttachmentEntityConfig<RoutingBomAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("TECH_ROUTING_BOM_ATT");
        }
    }
}
