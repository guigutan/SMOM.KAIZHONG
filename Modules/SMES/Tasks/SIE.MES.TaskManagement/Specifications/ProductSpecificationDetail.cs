using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Specifications
{
    /// <summary>
    /// 产品规格件清单
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("产品规格件清单")]
    public partial class ProductSpecificationDetail : DataEntity
    {
        #region 单体定额 Qty
        /// <summary>
        /// 单体定额
        /// </summary>
        [Label("单体定额")]
        public static readonly Property<decimal> QtyProperty = P<ProductSpecificationDetail>.Register(e => e.Qty);

        /// <summary>
        /// 单体定额
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 规格件清单 Specification
        /// <summary>
        /// 规格件清单Id
        /// </summary>
        public static readonly IRefIdProperty SpecificationIdProperty = P<ProductSpecificationDetail>.RegisterRefId(e => e.SpecificationId, ReferenceType.Normal);

        /// <summary>
        /// 规格件清单Id
        /// </summary>
        public double SpecificationId
        {
            get { return (double)GetRefId(SpecificationIdProperty); }
            set { SetRefId(SpecificationIdProperty, value); }
        }

        /// <summary>
        /// 规格件清单
        /// </summary>
        public static readonly RefEntityProperty<Specification> SpecificationProperty = P<ProductSpecificationDetail>.RegisterRef(e => e.Specification, SpecificationIdProperty);

        /// <summary>
        /// 规格件清单
        /// </summary>
        public Specification Specification
        {
            get { return GetRefEntity(SpecificationProperty); }
            set { SetRefEntity(SpecificationProperty, value); }
        }
        #endregion

        #region 规格件清单 ProductSpecification
        /// <summary>
        /// 规格件清单Id
        /// </summary>
        public static readonly IRefIdProperty ProductSpecificationIdProperty = P<ProductSpecificationDetail>.RegisterRefId(e => e.ProductSpecificationId, ReferenceType.Parent);

        /// <summary>
        /// 规格件清单Id
        /// </summary>
        public double ProductSpecificationId
        {
            get { return (double)GetRefId(ProductSpecificationIdProperty); }
            set { SetRefId(ProductSpecificationIdProperty, value); }
        }

        /// <summary>
        /// 规格件清单
        /// </summary>
        public static readonly RefEntityProperty<ProductSpecification> ProductSpecificationProperty = P<ProductSpecificationDetail>.RegisterRef(e => e.ProductSpecification, ProductSpecificationIdProperty);

        /// <summary>
        /// 规格件清单
        /// </summary>
        public ProductSpecification ProductSpecification
        {
            get { return GetRefEntity(ProductSpecificationProperty); }
            set { SetRefEntity(ProductSpecificationProperty, value); }
        }
        #endregion

        #region bs界面显示
        #region 规格件编码 SpecificationCode
        /// <summary>
        ///规格件编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> SpecificationCodeProperty = P<ProductSpecificationDetail>.RegisterView(e => e.SpecificationCode, p => p.Specification.Code);

        /// <summary>
        /// 规格件编码
        /// </summary>
        public string SpecificationCode
        {
            get { return this.GetProperty(SpecificationCodeProperty); }
        }
        #endregion

        #region 规格件名称 SpecificationName
        /// <summary>
        ///规格件名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> SpecificationNameProperty = P<ProductSpecificationDetail>.RegisterView(e => e.SpecificationName, p => p.Specification.Name);

        /// <summary>
        /// 规格件名称
        /// </summary>
        public string SpecificationName
        {
            get { return this.GetProperty(SpecificationNameProperty); }
        }
        #endregion

        #region 规格描述 SpecificationDescription
        /// <summary>
        ///规格描述
        /// </summary>
        [Label("规格描述")]
        public static readonly Property<string> SpecificationDescriptionProperty = P<ProductSpecificationDetail>.RegisterView(e => e.SpecificationDescription, p => p.Specification.Description);

        /// <summary>
        /// 规格描述
        /// </summary>
        public string SpecificationDescription
        {
            get { return this.GetProperty(SpecificationDescriptionProperty); }
        }
        #endregion

        #region 规格分类 SpecificationCategoryName
        /// <summary>
        ///规格分类
        /// </summary>
        [Label("规格分类")]
        public static readonly Property<string> SpecificationCategoryNameProperty = P<ProductSpecificationDetail>.RegisterView(e => e.SpecificationCategoryName, p => p.Specification.Category.Name);

        /// <summary>
        /// 规格分类
        /// </summary>
        public string SpecificationCategoryName
        {
            get { return this.GetProperty(SpecificationCategoryNameProperty); }
        }
        #endregion

        #endregion


        #region 产品规格件清单编码 ProductSpecificationCode
        /// <summary>
        /// 产品规格件清单编码
        /// </summary>
        [Label("产品规格件清单编码")]
        public static readonly Property<string> ProductSpecificationCodeProperty = P<ProductSpecificationDetail>.Register(e => e.ProductSpecificationCode);

        /// <summary>
        /// 产品规格件清单编码
        /// </summary>
        public string ProductSpecificationCode
        {
            get { return this.GetProperty(ProductSpecificationCodeProperty); }
            set { this.SetProperty(ProductSpecificationCodeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 产品规格件清单 实体配置
    /// </summary>
    internal class ProdcutSpecificationDetailConfig : EntityConfig<ProductSpecificationDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_PROD_SPEC_DTL").MapAllPropertiesExcept(ProductSpecificationDetail.ProductSpecificationCodeProperty);
            Meta.EnablePhantoms();
        }
    }
}
