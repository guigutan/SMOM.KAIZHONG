using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.Core.Equipments.FixedAssets
{
    /// <summary>
    /// 固定资产台账
    /// </summary>
    [RootEntity, Serializable]    
    [Label("固定资产台账")]
    public partial class FixedAssetsAccount : DataEntity
    {
        #region 资产编码 Code
        /// <summary>
        /// 资产编码
        /// </summary>
        [Label("资产编码")]
        [NotDuplicate]
        [Required]
        public static readonly Property<string> CodeProperty = P<FixedAssetsAccount>.Register(e => e.Code);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 资产名称 Name
        /// <summary>
        /// 资产名称
        /// </summary>
        [Label("资产名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<FixedAssetsAccount>.Register(e => e.Name);

        /// <summary>
        /// 资产名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 资产净值 NetAssetValue
        /// <summary>
        /// 资产净值
        /// </summary>
        [Label("资产净值")]
        public static readonly Property<decimal?> NetAssetValueProperty = P<FixedAssetsAccount>.Register(e => e.NetAssetValue);

        /// <summary>
        /// 资产净值
        /// </summary>
        public decimal? NetAssetValue
        {
            get { return GetProperty(NetAssetValueProperty); }
            set { SetProperty(NetAssetValueProperty, value); }
        }
        #endregion

        #region 资产原值 OriginalAssetsValue
        /// <summary>
        /// 资产原值
        /// </summary>
        [Label("资产原值")]
        [Required]
        public static readonly Property<decimal> OriginalAssetsValueProperty = P<FixedAssetsAccount>.Register(e => e.OriginalAssetsValue);

        /// <summary>
        /// 资产原值
        /// </summary>
        public decimal OriginalAssetsValue
        {
            get { return GetProperty(OriginalAssetsValueProperty); }
            set { SetProperty(OriginalAssetsValueProperty, value); }
        }
        #endregion

        #region 资产分类 AssetsType
        /// <summary>
        /// 资产分类(快码)
        /// </summary>
        [Label("资产分类")]
        [Required]
        public static readonly Property<string> AssetsCategoryProperty = P<FixedAssetsAccount>.Register(e => e.AssetsCategory);

        /// <summary>
        /// 资产分类
        /// </summary>
        public string AssetsCategory
        {
            get { return GetProperty(AssetsCategoryProperty); }
            set { SetProperty(AssetsCategoryProperty, value); }
        }
        #endregion

        #region 资产责任人 AssetOwner
        /// <summary>
        /// 资产责任人Id
        /// </summary>
        [Label("资产责任人")]
        public static readonly IRefIdProperty AssetOwnerIdProperty = P<FixedAssetsAccount>.RegisterRefId(e => e.AssetOwnerId, ReferenceType.Normal);

        /// <summary>
        ///资产责任人Id
        /// </summary>
        public double AssetOwnerId
        {
            get { return (double)GetRefId(AssetOwnerIdProperty); }
            set { SetRefId(AssetOwnerIdProperty, value); }
        }

        /// <summary>
        /// 资产责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AssetOwnerProperty = P<FixedAssetsAccount>.RegisterRef(e => e.AssetOwner, AssetOwnerIdProperty);

        /// <summary>
        /// 资产责任人
        /// </summary>
        public Employee AssetOwner
        {
            get { return GetRefEntity(AssetOwnerProperty); }
            set { SetRefEntity(AssetOwnerProperty, value); }
        }
        #endregion

        #region 折旧残值 DepreciationResidualValue
        /// <summary>
        /// 折旧残值
        /// </summary>
        [Label("折旧残值")]
        [Required]
        public static readonly Property<decimal> DepreciationResidualValueProperty = P<FixedAssetsAccount>.Register(e => e.DepreciationResidualValue);

        /// <summary>
        /// 折旧残值
        /// </summary>
        public decimal DepreciationResidualValue
        {
            get { return GetProperty(DepreciationResidualValueProperty); }
            set { SetProperty(DepreciationResidualValueProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 固定资产台账 实体配置
    /// </summary>
    internal class FixedAssetAccountConfig : EntityConfig<FixedAssetsAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXED_ASSETS_ACCOUNT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
