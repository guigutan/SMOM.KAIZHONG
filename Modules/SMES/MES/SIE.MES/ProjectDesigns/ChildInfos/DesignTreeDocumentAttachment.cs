using SIE.Common.Attachments;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-文件上传附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺资料-文件上传附件")]
    public class DesignTreeDocumentAttachment : Attachment<DesignTreeDocument>
    {
    }

    /// <summary>
    /// 附件数据仓库
    /// </summary>
    [DataProvider(typeof(MesCoreEntityDataProvider))]
    public partial class DesignTreeDocumentAttachmentRepository : AttachmentRepository<DesignTreeDocumentAttachment>
    {

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeDocumentAttachmentConfig : AttachmentEntityConfig<DesignTreeDocumentAttachment>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.EnableDiscriminator("MES_DESIGN_TREE");
        }
    }
}
