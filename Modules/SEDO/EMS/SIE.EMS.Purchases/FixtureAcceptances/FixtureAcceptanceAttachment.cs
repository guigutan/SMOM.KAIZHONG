using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
    /// <summary>
    /// 工治具验收附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工治具验收附件")]
    public partial class FixtureAcceptanceAttachment : Attachment<FixtureAcceptance>
    {
        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<FixtureAcceptanceAttachment>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 序列号下拉 SnViewModel
        /// <summary>
        /// 序列号下拉Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty SnViewModelIdProperty =
            P<FixtureAcceptanceAttachment>.RegisterRefId(e => e.SnViewModelId, ReferenceType.Normal);

        /// <summary>
        /// 序列号下拉Id
        /// </summary>
        public double? SnViewModelId
        {
            get { return (double?)this.GetRefNullableId(SnViewModelIdProperty); }
            set { this.SetRefNullableId(SnViewModelIdProperty, value); }
        }

        /// <summary>
        /// 序列号下拉
        /// </summary>
        public static readonly RefEntityProperty<FixtureAcceptanceSn> SnViewModelProperty =
            P<FixtureAcceptanceAttachment>.RegisterRef(e => e.SnViewModel, SnViewModelIdProperty);

        /// <summary>
        /// 序列号下拉
        /// </summary>
        public FixtureAcceptanceSn SnViewModel
        {
            get { return this.GetRefEntity(SnViewModelProperty); }
            set { this.SetRefEntity(SnViewModelProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  数据仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class FixtureAcceptanceAttachmentRepository : AttachmentRepository<FixtureAcceptanceAttachment>
    {
    }

    /// <summary>
    /// 验收附件 实体配置
    /// </summary>
    internal class FixtureAcceptanceAttachmentConfig : AttachmentEntityConfig<FixtureAcceptanceAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXT_ACPT_ATCH").MapAllProperties();
            Meta.Property(FixtureAcceptanceAttachment.SnViewModelIdProperty).DontMapColumn();
            Meta.Property(Attachment.FilePathProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Attachment.FileNameProperty).ColumnMeta.HasLength(240);
            Meta.Property("OwnerId").ColumnMeta.IgnoreFK();
            Meta.Property(FixtureAcceptanceAttachment.ContentProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}