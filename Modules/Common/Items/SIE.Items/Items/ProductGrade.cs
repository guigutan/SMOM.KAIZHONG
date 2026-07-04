using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items
{
    /// <summary>
	/// 产品等级
	/// </summary>
	[RootEntity, Serializable]
    ////[CriteriaQuery]
    [ConditionQueryType(typeof(ProductGradeCriteria))]
    [Label("产品等级")]
    public partial class ProductGrade : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        ////[NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ProductGrade>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<ProductGrade>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Describe
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescribeProperty = P<ProductGrade>.Register(e => e.Describe);

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe
        {
            get { return GetProperty(DescribeProperty); }
            set { SetProperty(DescribeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ProductGrade>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ProductGrade>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品等级 实体配置
    /// </summary>
    internal class ProductGradeConfig : EntityConfig<ProductGrade>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRODUCT_GRADE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}