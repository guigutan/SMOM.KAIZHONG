using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品族分类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("产品族分类")]
    [DisplayMember(nameof(Name))]
    public partial class ProductFamilyCategory : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ProductFamilyCategory>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<ProductFamilyCategory>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品族分类 实体配置
    /// </summary>
    internal class ProductFamilyCategoryEntityConfig : EntityConfig<ProductFamilyCategory>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_PROD_FAMILY_CATE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}