using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 生产产品工序图片
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产产品工序图片")]
    public partial class WipProductProcessPhoto : DataEntity
    {
        #region 图片地址 Url
        /// <summary>
        /// 图片地址
        /// </summary>
        [Label("图片地址")]
        [MaxLength(120)]
        public static readonly Property<string> UrlProperty = P<WipProductProcessPhoto>.Register(e => e.Url);

        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url
        {
            get { return this.GetProperty(UrlProperty); }
            set { this.SetProperty(UrlProperty, value); }
        }
        #endregion

        #region 是否不良图片 IsNg
        /// <summary>
        /// 是否不良图片
        /// </summary>
        [Label("是否不良图片")]
        public static readonly Property<bool> IsNgProperty = P<WipProductProcessPhoto>.Register(e => e.IsNg);

        /// <summary>
        /// 是否不良图片
        /// </summary>
        public bool IsNg
        {
            get { return this.GetProperty(IsNgProperty); }
            set { this.SetProperty(IsNgProperty, value); }
        }
        #endregion

        #region 产品缺陷 Defect
        /// <summary>
        /// 产品缺陷Id
        /// </summary>
        [Label("产品缺陷")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<WipProductProcessPhoto>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 产品缺陷Id
        /// </summary>
        public double? DefectId
        {
            get { return (double?)this.GetRefNullableId(DefectIdProperty); }
            set { this.SetRefNullableId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷
        /// </summary>
        public static readonly RefEntityProperty<WipProductDefect> DefectProperty =
            P<WipProductProcessPhoto>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 产品缺陷
        /// </summary>
        public WipProductDefect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 生产工序 Process
        /// <summary>
        /// 生产工序
        /// </summary>
        [Label("生产工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductProcessPhoto>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 生产工序
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 生产工序
        /// </summary>
        public static readonly RefEntityProperty<WipProductProcess> ProcessProperty = P<WipProductProcessPhoto>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 生产工序
        /// </summary>
        public WipProductProcess Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 生产产品工序图片 实体配置
    /// </summary>
    internal class WipProductProcessPhotoConfig : EntityConfig<WipProductProcessPhoto>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_PHOTO").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(WipProductProcessPhoto.UrlProperty).ColumnMeta.HasLength(480);
        }
    }
}