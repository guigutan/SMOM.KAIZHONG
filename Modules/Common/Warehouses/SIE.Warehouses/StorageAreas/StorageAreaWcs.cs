using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 立库配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("立库配置")]
    [DisplayMember(nameof(StorageAreaWcs.Id))]
    public partial class StorageAreaWcs : DataEntity
    {
        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<StorageAreaWcs>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double StorageAreaId
        {
            get { return (double)GetRefId(StorageAreaIdProperty); }
            set { SetRefId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<StorageAreaWcs>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 批次要求1 LotRequire1
        /// <summary>
        /// 批次要求1
        /// </summary>
        [Label("批次要求1")]
        public static readonly Property<LotType?> LotRequire1Property = P<StorageAreaWcs>.Register(e => e.LotRequire1);

        /// <summary>
        /// 批次要求1
        /// </summary>
        public LotType? LotRequire1
        {
            get { return this.GetProperty(LotRequire1Property); }
            set { this.SetProperty(LotRequire1Property, value); }
        }
        #endregion

        #region 批次要求2 LotRequire2
        /// <summary>
        /// 批次要求2
        /// </summary>
        [Label("批次要求2")]
        public static readonly Property<LotType?> LotRequire2Property = P<StorageAreaWcs>.Register(e => e.LotRequire2);

        /// <summary>
        /// 批次要求2
        /// </summary>
        public LotType? LotRequire2
        {
            get { return this.GetProperty(LotRequire2Property); }
            set { this.SetProperty(LotRequire2Property, value); }
        }
        #endregion

        #region 批次要求3 LotRequire3
        /// <summary>
        /// 批次要求3
        /// </summary>
        [Label("批次要求3")]
        public static readonly Property<LotType?> LotRequire3Property = P<StorageAreaWcs>.Register(e => e.LotRequire3);

        /// <summary>
        /// 批次要求3
        /// </summary>
        public LotType? LotRequire3
        {
            get { return this.GetProperty(LotRequire3Property); }
            set { this.SetProperty(LotRequire3Property, value); }
        }
        #endregion

        #region 批次要求4 LotRequire4
        /// <summary>
        /// 批次要求4
        /// </summary>
        [Label("批次要求4")]
        public static readonly Property<LotType?> LotRequire4Property = P<StorageAreaWcs>.Register(e => e.LotRequire4);

        /// <summary>
        /// 批次要求4
        /// </summary>
        public LotType? LotRequire4
        {
            get { return this.GetProperty(LotRequire4Property); }
            set { this.SetProperty(LotRequire4Property, value); }
        }
        #endregion
    }

    /// <summary>
    /// 库区基本资料 实体配置
    /// </summary>
    internal class StorageAreaWcsConfig : EntityConfig<StorageAreaWcs>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_AREA_WCS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
