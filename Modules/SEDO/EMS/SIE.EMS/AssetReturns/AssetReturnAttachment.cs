using SIE.Common.Attachments;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.AssetReturns
{
    /// <summary>
    /// 附件资料
    /// </summary>
    [ChildEntity, Serializable]
    [Label("附件")]
    public partial class AssetReturnAttachment : Attachment<AssetReturn>
    {
        #region 设备编码 EquipmentCodes
        /// <summary>
        /// 设备编码
        /// </summary>
        [MaxLength(1000)]
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodesProperty = P<AssetReturnAttachment>.Register(e => e.EquipmentCodes);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCodes
        {
            get { return GetProperty(EquipmentCodesProperty); }
            set { SetProperty(EquipmentCodesProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureCodes
        /// <summary>
        /// 工治具编码
        /// </summary>
        [MaxLength(1000)]
        [Label("工治具编码")]
        public static readonly Property<string> FixtureCodesProperty = P<AssetReturnAttachment>.Register(e => e.FixtureCodes);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureCodes
        {
            get { return GetProperty(FixtureCodesProperty); }
            set { SetProperty(FixtureCodesProperty, value); }
        }
        #endregion

        #region 模具编码 MoldCodes
        /// <summary>
        /// 模具编码
        /// </summary>
        [MaxLength(1000)]
        [Label("模具编码")]
        public static readonly Property<string> MoldCodesProperty = P<AssetReturnAttachment>.Register(e => e.MoldCodes);

        /// <summary>
        /// 模具编码
        /// </summary>
        public string MoldCodes
        {
            get { return GetProperty(MoldCodesProperty); }
            set { SetProperty(MoldCodesProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class AssetReturnAttachmentRepository : AttachmentRepository<AssetReturnAttachment>
    {
    }

    /// <summary>
    /// 附件资料 实体配置
    /// </summary>
    internal class AssetReturnAttachmentConfig : AttachmentEntityConfig<AssetReturnAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            base.ConfigMeta();
            Meta.MapTable("EMS_ASET_RETURN_ATT").MapAllPropertiesExcept(AssetReturnAttachment.ContentProperty);//文件内容映射到数据库会卡死
            Meta.Property(AssetReturnAttachment.EquipmentCodesProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AssetReturnAttachment.FixtureCodesProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AssetReturnAttachment.MoldCodesProperty).ColumnMeta.HasLength(4000);
            Meta.EnableDiscriminator("EMS_ASET_RETURN");
        }
    }
}