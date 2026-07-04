using SIE.Common.Attachments;
using SIE.Domain;
using SIE.EMS.Purchases.EquipmentAcceptances.ViewModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentAcceptances
{
    /// <summary>
    /// 设备开箱验收附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备开箱验收附件")]
    public partial class EquipmentAcceptanceAttachment : Attachment<EquipmentAcceptance>
    {
        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<EquipmentAcceptanceAttachment>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return GetProperty(EquipmentCodeProperty); }
            set { SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 设备验收设备明细视图 EquipmentAcceptanceDetailViewModel
        /// <summary>
        /// 设备验收设备明细视图Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipmentAcceptanceDetailViewModelIdProperty =
            P<EquipmentAcceptanceAttachment>.RegisterRefId(e => e.EquipmentAcceptanceDetailViewModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备验收设备明细视图Id
        /// </summary>
        public double? EquipmentAcceptanceDetailViewModelId
        {
            get { return (double?)this.GetRefNullableId(EquipmentAcceptanceDetailViewModelIdProperty); }
            set { this.SetRefNullableId(EquipmentAcceptanceDetailViewModelIdProperty, value); }
        }

        /// <summary>
        /// 设备验收设备明细视图
        /// </summary>
        public static readonly RefEntityProperty<EquipmentAcceptanceDetailViewModel> EquipmentAcceptanceDetailViewModelProperty =
            P<EquipmentAcceptanceAttachment>.RegisterRef(e => e.EquipmentAcceptanceDetailViewModel, EquipmentAcceptanceDetailViewModelIdProperty);

        /// <summary>
        /// 设备验收设备明细视图
        /// </summary>
        public EquipmentAcceptanceDetailViewModel EquipmentAcceptanceDetailViewModel
        {
            get { return this.GetRefEntity(EquipmentAcceptanceDetailViewModelProperty); }
            set { this.SetRefEntity(EquipmentAcceptanceDetailViewModelProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  数据仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class EquipmentAcceptanceAttachmentRepository : AttachmentRepository<EquipmentAcceptanceAttachment>
    {
    }

    /// <summary>
	/// 设备开箱验收附件 实体配置
	/// </summary>
	internal class EquipmentAcceptanceAttachmentConfig : AttachmentEntityConfig<EquipmentAcceptanceAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_ACPT_ATCH").MapAllProperties();
            Meta.Property(Attachment.FilePathProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Attachment.FileNameProperty).ColumnMeta.HasLength(240);
            Meta.Property("OwnerId").ColumnMeta.IgnoreFK();
            Meta.Property(EquipmentAcceptanceAttachment.ContentProperty).DontMapColumn();
            Meta.Property(EquipmentAcceptanceAttachment.EquipmentAcceptanceDetailViewModelIdProperty).DontMapColumn();            
            Meta.EnablePhantoms();
        }
    }
}