using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 复核包装规则明细
    /// </summary>
    [ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("复核包装规则明细")]
	public partial class RePackageRuleDetail : DataEntity
	{
		#region 混装品种数 ItemQty
		/// <summary>
		/// 混装品种数
		/// </summary>
		[Label("混装品种数")]
		[MinValue(1)]
		public static readonly Property<int> ItemQtyProperty = P<RePackageRuleDetail>.Register(e => e.ItemQty);

		/// <summary>
		/// 混装品种数
		/// </summary>
		public int ItemQty
		{
			get { return GetProperty(ItemQtyProperty); }
			set { SetProperty(ItemQtyProperty, value); }
		}
		#endregion

		#region 混装批次数 LotQty
		/// <summary>
		/// 混装批次数
		/// </summary>
		[Label("混装批次数")]
		[MinValue(1)]
		public static readonly Property<int> LotQtyProperty = P<RePackageRuleDetail>.Register(e => e.LotQty);

		/// <summary>
		/// 混装批次数
		/// </summary>
		public int LotQty
		{
			get { return GetProperty(LotQtyProperty); }
			set { SetProperty(LotQtyProperty, value); }
		}
        #endregion

        #region 品类混放 MixedType
        /// <summary>
        /// 品类混放
        /// </summary>
        [Label("品类混放")]
        public static readonly Property<MixedType> MixedTypeProperty = P<RePackageRuleDetail>.Register(e => e.MixedType);

        /// <summary>
        /// 品类混放
        /// </summary>
        public MixedType MixedType
        {
            get { return this.GetProperty(MixedTypeProperty); }
            set { this.SetProperty(MixedTypeProperty, value); }
        }
        #endregion

        #region 箱型 BoxType
        /// <summary>
        /// 箱型
        /// </summary>
        [Label("箱型")]
		public static readonly Property<BoxType> BoxTypeProperty = P<RePackageRuleDetail>.Register(e => e.BoxType);

		/// <summary>
		/// 箱型
		/// </summary>
		public BoxType BoxType
		{
			get { return GetProperty(BoxTypeProperty); }
			set { SetProperty(BoxTypeProperty, value); }
		}
		#endregion

		#region 分类层级 ItemCategoryLevel
		/// <summary>
		/// 分类层级Id
		/// </summary>
		[Label("分类层级")]
		public static readonly IRefIdProperty ItemCategoryLevelIdProperty = P<RePackageRuleDetail>.RegisterRefId(e => e.ItemCategoryLevelId, ReferenceType.Normal);

		/// <summary>
		/// 分类层级Id
		/// </summary>
		public double? ItemCategoryLevelId
		{
			get { return (double?)GetRefId(ItemCategoryLevelIdProperty); }
			set { SetRefId(ItemCategoryLevelIdProperty, value); }
		}

		/// <summary>
		/// 分类层级
		/// </summary>
		public static readonly RefEntityProperty<ItemCategoryLevel> ItemCategoryLevelProperty = P<RePackageRuleDetail>.RegisterRef(e => e.ItemCategoryLevel, ItemCategoryLevelIdProperty);

		/// <summary>
		/// 分类层级
		/// </summary>
		public ItemCategoryLevel ItemCategoryLevel
		{
			get { return GetRefEntity(ItemCategoryLevelProperty); }
			set { SetRefEntity(ItemCategoryLevelProperty, value); }
		}
		#endregion

		#region 规则明细 RePackageRule
		/// <summary>
		/// 规则明细Id
		/// </summary>
		public static readonly IRefIdProperty RePackageRuleIdProperty = P<RePackageRuleDetail>.RegisterRefId(e => e.RePackageRuleId, ReferenceType.Parent);

		/// <summary>
		/// 规则明细Id
		/// </summary>
		public double RePackageRuleId
		{
			get { return (double)GetRefId(RePackageRuleIdProperty); }
			set { SetRefId(RePackageRuleIdProperty, value); }
		}

		/// <summary>
		/// 规则明细
		/// </summary>
		public static readonly RefEntityProperty<RePackageRule> RePackageRuleProperty = P<RePackageRuleDetail>.RegisterRef(e => e.RePackageRule, RePackageRuleIdProperty);

		/// <summary>
		/// 规则明细
		/// </summary>
		public RePackageRule RePackageRule
		{
			get { return GetRefEntity(RePackageRuleProperty); }
			set { SetRefEntity(RePackageRuleProperty, value); }
		}
        #endregion

        #region 视图属性
        #region 装箱规则编码 RePacageRuleCode
        /// <summary>
        /// 装箱规则编码
        /// </summary>
        [Label("装箱规则编码")]
        public static readonly Property<string> RePacageRuleCodeProperty = P<RePackageRuleDetail>.RegisterView(e => e.RePacageRuleCode, p => p.RePackageRule.Code);

        /// <summary>
        /// 装箱规则编码
        /// </summary>
        public string RePacageRuleCode
        {
            get { return this.GetProperty(RePacageRuleCodeProperty); }
        }
        #endregion


        #region 分类层级编码 ItemCategoryLevelCode
        /// <summary>
        /// 分类层级编码
        /// </summary>
        [Label("分类层级编码")]
        public static readonly Property<string> ItemCategoryLevelCodeProperty = P<RePackageRuleDetail>.RegisterView(e => e.ItemCategoryLevelCode, p => p.ItemCategoryLevel.Code);

        /// <summary>
        /// 分类层级编码
        /// </summary>
        public string ItemCategoryLevelCode
        {
            get { return this.GetProperty(ItemCategoryLevelCodeProperty); }
        }
        #endregion

        #endregion
    }

	/// <summary>
	/// 复核包装规则明细 实体配置
	/// </summary>
	internal class RePackageRuleDetailConfig : EntityConfig<RePackageRuleDetail>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("RE_PKG_RULE_DTL").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}