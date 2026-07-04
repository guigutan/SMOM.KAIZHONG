using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品族
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("产品族")]
    [DisplayMember(nameof(ProductFamily.Code))]
    public partial class ProductFamily : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ProductFamily>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<ProductFamily>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 产品族分类 Category
        /// <summary>
        /// 产品族分类Id
        /// </summary> 
        [Label("产品族分类")]
        public static readonly IRefIdProperty CategoryIdProperty = P<ProductFamily>.RegisterRefId(e => e.CategoryId, ReferenceType.Normal);

        /// <summary>
        /// 产品族分类Id
        /// </summary>
        public double CategoryId
        {
            get { return (double)GetRefId(CategoryIdProperty); }
            set { SetRefId(CategoryIdProperty, value); }
        }

        /// <summary>
        /// 产品族分类
        /// </summary>
		public static readonly RefEntityProperty<ProductFamilyCategory> CategoryProperty = P<ProductFamily>.RegisterRef(e => e.Category, CategoryIdProperty);

        /// <summary>
        /// 产品族分类
        /// </summary>
        public ProductFamilyCategory Category
        {
            get { return GetRefEntity(CategoryProperty); }
            set { SetRefEntity(CategoryProperty, value); }
        }
        #endregion

        #region 族分类编码 CategoryCode
        /// <summary>
        /// 族分类编码
        /// </summary>
        [Label("族分类编码")]
        public static readonly Property<string> CategoryCodeProperty = P<ProductFamily>.RegisterView(e => e.CategoryCode, p => p.Category.Code);

        /// <summary>
        /// 族分类编码
        /// </summary>
        public string CategoryCode
        {
            get { return this.GetProperty(CategoryCodeProperty); }
        }
        #endregion 
    }

    /// <summary>
    /// 产品族 实体配置
    /// </summary>
    internal class ProductFamilyEntityConfig : EntityConfig<ProductFamily>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROD_FAMILY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}