using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipAccounts
{
    /// <summary>
	/// 附件资料
	/// </summary>
	[ChildEntity, Serializable]    
    [Label("附件资料")]
    public partial class EquipAccountAttachment : Attachment<EquipAccount>
    {
        #region 是否设备Logo IsEquipLogo
        /// <summary>
        /// 是否设备Logo
        /// </summary>
        [Label("是否设备Logo")]
        public static readonly Property<bool?> IsEquipLogoProperty
            = P<EquipAccountAttachment>.Register(e => e.IsEquipLogo);

        /// <summary>
        /// 是否设备Logo
        /// </summary>
        public bool? IsEquipLogo
        {
            get { return this.GetProperty(IsEquipLogoProperty); }
            set { this.SetProperty(IsEquipLogoProperty, value); }
        }
        #endregion

    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EquipmentEntityDataProvider))]
    public partial class EquipAccountAttachmentRepository : AttachmentRepository<EquipAccountAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class EquipAccountAttachmentConfig : AttachmentEntityConfig<EquipAccountAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.MapTable("EMS_EQUIP_ACCOUNT_ATT").MapAllProperties();
            Meta.Property(EquipAccountAttachment.ContentProperty).DontMapColumn();
            Meta.EnableDiscriminator("EMS_EQUIP_ACCOUNT");
        }
    }
}
