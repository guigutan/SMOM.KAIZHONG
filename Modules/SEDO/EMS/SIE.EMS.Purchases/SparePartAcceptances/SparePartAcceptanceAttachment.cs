using SIE.Common.Attachments;
using SIE.Domain;
using SIE.EMS.Purchases.SparePartAcceptances.ViewModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 验收附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("验收附件")]
    public partial class SparePartAcceptanceAttachment : Attachment<SparePartAcceptance>
    {
        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<SparePartAcceptanceAttachment>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<SparePartAcceptanceAttachment>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 视图属性（不映射数据库）
        #region 批次号下拉 LotViewModel
        /// <summary>
        /// 批次号下拉Id
        /// </summary>
        [Label("批次号")]
        public static readonly IRefIdProperty LotViewModelIdProperty =
            P<SparePartAcceptanceAttachment>.RegisterRefId(e => e.LotViewModelId, ReferenceType.Normal);

        /// <summary>
        /// 批次号下拉Id
        /// </summary>
        public double? LotViewModelId
        {
            get { return (double?)this.GetRefNullableId(LotViewModelIdProperty); }
            set { this.SetRefNullableId(LotViewModelIdProperty, value); }
        }

        /// <summary>
        /// 批次号下拉
        /// </summary>
        public static readonly RefEntityProperty<SparePartAcceptanceLotViewModel> LotViewModelProperty =
            P<SparePartAcceptanceAttachment>.RegisterRef(e => e.LotViewModel, LotViewModelIdProperty);

        /// <summary>
        /// 批次号下拉
        /// </summary>
        public SparePartAcceptanceLotViewModel LotViewModel
        {
            get { return this.GetRefEntity(LotViewModelProperty); }
            set { this.SetRefEntity(LotViewModelProperty, value); }
        }
        #endregion

        #region 序列号下拉 SnViewModel
        /// <summary>
        /// 序列号下拉Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty SnViewModelIdProperty =
            P<SparePartAcceptanceAttachment>.RegisterRefId(e => e.SnViewModelId, ReferenceType.Normal);

        /// <summary>
        /// 序列号下拉Id
        /// </summary>
        public string SnViewModelId
        {
            get { return (string)this.GetRefNullableId(SnViewModelIdProperty); }
            set { this.SetRefNullableId(SnViewModelIdProperty, value); }
        }

        /// <summary>
        /// 序列号下拉
        /// </summary>
        public static readonly RefEntityProperty<SparePartAcceptanceSnViewModel> SnViewModelProperty =
            P<SparePartAcceptanceAttachment>.RegisterRef(e => e.SnViewModel, SnViewModelIdProperty);

        /// <summary>
        /// 序列号下拉
        /// </summary>
        public SparePartAcceptanceSnViewModel SnViewModel
        {
            get { return this.GetRefEntity(SnViewModelProperty); }
            set { this.SetRefEntity(SnViewModelProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class SparePartAcceptanceAttachmentRepository : AttachmentRepository<SparePartAcceptanceAttachment>
    {
    }

    /// <summary>
    /// 验收附件 实体配置
    /// </summary>
    internal class SparePartAcceptanceAttachmentConfig : AttachmentEntityConfig<SparePartAcceptanceAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_ACPT_ATCH").MapAllProperties();
            Meta.Property(Attachment.FilePathProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Attachment.FileNameProperty).ColumnMeta.HasLength(240);
            Meta.Property("OwnerId").ColumnMeta.IgnoreFK();
            Meta.Property(SparePartAcceptanceAttachment.ContentProperty).DontMapColumn();
            Meta.Property(SparePartAcceptanceAttachment.LotViewModelIdProperty).DontMapColumn();
            Meta.Property(SparePartAcceptanceAttachment.SnViewModelIdProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}