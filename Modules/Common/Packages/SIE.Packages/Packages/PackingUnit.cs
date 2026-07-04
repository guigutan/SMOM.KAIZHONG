using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 包装单位
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("包装单位")]
    [DisplayMember(nameof(Code))]
    public partial class PackingUnit : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<PackingUnit>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [Label("名称")]
        [NotDuplicate]
        public static readonly Property<string> NameProperty = P<PackingUnit>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<PackingUnit>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否主单位
        /// <summary>
        /// 是否主单位
        /// </summary>
        [Label("主单位")]
        public static readonly Property<bool> IsMasterUnitProperty = P<PackingUnit>.Register(e => e.IsMasterUnit);

        /// <summary>
        /// 是否主单位
        /// </summary>
        public bool IsMasterUnit
        {
            get { return this.GetProperty(IsMasterUnitProperty); }
            set { this.SetProperty(IsMasterUnitProperty, value); }
        }
        #endregion

        #region 包装类型 PackageUnitType
        /// <summary>
        /// 包装类型
        /// </summary>
        [Label("包装类型")]
        [Required]
        public static readonly Property<PackageUnitType?> PackageUnitTypeProperty = P<PackingUnit>.Register(e => e.PackageUnitType);

        /// <summary>
        /// 包装类型
        /// </summary>
        public PackageUnitType? PackageUnitType
        {
            get { return this.GetProperty(PackageUnitTypeProperty); }
            set { this.SetProperty(PackageUnitTypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 包装单位 实体配置
    /// </summary>
    internal class PackingUnitConfig : EntityConfig<PackingUnit>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("PKG_UNIT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}