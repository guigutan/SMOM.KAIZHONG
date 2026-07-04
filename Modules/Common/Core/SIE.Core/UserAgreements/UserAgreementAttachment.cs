using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.UserAgreements
{

    /// <summary>
    /// 用户协议附件
    /// </summary>   
    [ChildEntity, Serializable]
    [Label("附件")]
    public class UserAgreementAttachment : Attachment<UserAgreement>
    {
        #region 协议类型 AgreementType
        /// <summary>
        /// 协议类型
        /// </summary>
        [Label("协议类型")]
        public static readonly Property<AgreementType> AgreementTypeProperty = P<UserAgreementAttachment>.Register(e => e.AgreementType);

        /// <summary>
        /// 协议类型
        /// </summary>
        public AgreementType AgreementType
        {
            get { return this.GetProperty(AgreementTypeProperty); }
            set { this.SetProperty(AgreementTypeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 仓库
    /// </summary>
    public partial class UserAgreementAttachmentRepository : AttachmentRepository<UserAgreementAttachment>
    {
    }

    /// <summary>
    /// 附件 实体配置
    /// </summary>
    internal class InventoryLendAttachmentConfig : AttachmentEntityConfig<UserAgreementAttachment>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            // 鉴别器，对应bd_attachment附件管理表中的DISCRIMINATOR
            Meta.EnableDiscriminator("UserAgreementAttachment");
            Meta.DisableInvOrg();
            Meta.Property(UserAgreementAttachment.AgreementTypeProperty).DontMapColumn();
        }
    }
}
