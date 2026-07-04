using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages
{
    /// <summary>
    /// 包装规则
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("包装规则")]
    [DisplayMember(nameof(Code))]
    public partial class PackageRule : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<PackageRule>.Register(e => e.Code);

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
        [NotDuplicate]
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<PackageRule>.Register(e => e.Name);

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
        [MaxLength(80)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<PackageRule>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 包装规则明细 PackageRuleDetailList
        /// <summary>
        /// 包装规则明细
        /// </summary>
        [Label("包装规则明细")]
        public static readonly ListProperty<EntityList<PackageRuleDetail>> PackageRuleDetailListProperty = P<PackageRule>.RegisterList(e => e.PackageRuleDetailList);

        /// <summary>
        /// 包装规则明细
        /// </summary>
        public EntityList<PackageRuleDetail> PackageRuleDetailList
        {
            get { return this.GetLazyList(PackageRuleDetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 包装规则 实体配置
    /// </summary>
    internal class PackageRuleConfig : EntityConfig<PackageRule>
    {
        /// <summary>
        /// 配置实体
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PKG_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}