using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Equipments;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
	/// 附件资料
	/// </summary>
	[ChildEntity, Serializable]    
    [Label("附件资料")]
    public partial class MeteringEquipAccountAttachment : Attachment<MeteringEquipmentAccount>
    {
        #region 是否设备Logo IsEquipLogo
        /// <summary>
        /// 是否设备Logo
        /// </summary>
        [Label("是否设备Logo")]
        public static readonly Property<bool?> IsEquipLogoProperty
            = P<MeteringEquipAccountAttachment>.Register(e => e.IsEquipLogo);

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
    public partial class EquipAccountAttachmentRepository : AttachmentRepository<MeteringEquipAccountAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class EquipAccountAttachmentConfig : AttachmentEntityConfig<MeteringEquipAccountAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.MapTable("EMS_EQUIP_ACCOUNT_ATT").MapAllProperties();
            Meta.Property(MeteringEquipAccountAttachment.ContentProperty).DontMapColumn();
            Meta.EnableDiscriminator("EMS_EQUIP_ACCOUNT");
        }
    }
}
