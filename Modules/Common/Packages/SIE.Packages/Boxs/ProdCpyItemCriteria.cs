using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 产品容量添加命令物料查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class ProdCpyItemCriteria : Criteria
    {
        #region 物料编码 Code
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> CodeProperty = P<ProdCpyItemCriteria>.Register(e => e.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料名称 Name
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> NameProperty = P<ProdCpyItemCriteria>.Register(e => e.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 基本分类 Type
        /// <summary>
        /// 基本分类
        /// </summary>
        [Label("基本分类")]
        public static readonly Property<ItemType?> TypeProperty = P<ProdCpyItemCriteria>.Register(e => e.Type);

        /// <summary>
        /// 基本分类
        /// </summary>
        public ItemType? Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>   
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<ItemSourceType?> ItemSourceTypeProperty = P<ProdCpyItemCriteria>.Register(e => e.ItemSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType
        {
            get { return this.GetProperty(ItemSourceTypeProperty); }
            set { this.SetProperty(ItemSourceTypeProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty =
            P<ProdCpyItemCriteria>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double? ProductFamilyId
        {
            get { return (double?)this.GetRefNullableId(ProductFamilyIdProperty); }
            set { this.SetRefNullableId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty =
            P<ProdCpyItemCriteria>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return this.GetRefEntity(ProductFamilyProperty); }
            set { this.SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 产品机型 ProductModel
        /// <summary>
        /// 产品机型Id
        /// </summary>
        [Label("产品机型")]
        public static readonly IRefIdProperty ProductModelIdProperty =
            P<ProdCpyItemCriteria>.RegisterRefId(e => e.ProductModelId, ReferenceType.Normal);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double? ProductModelId
        {
            get { return (double?)this.GetRefNullableId(ProductModelIdProperty); }
            set { this.SetRefNullableId(ProductModelIdProperty, value); }
        }

        /// <summary>
        /// 产品机型
        /// </summary>
        public static readonly RefEntityProperty<ProductModel> ProductModelProperty =
            P<ProdCpyItemCriteria>.RegisterRef(e => e.ProductModel, ProductModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel ProductModel
        {
            get { return this.GetRefEntity(ProductModelProperty); }
            set { this.SetRefEntity(ProductModelProperty, value); }
        }
        #endregion

        #region 已添加物料Id集合 FilterIds
        /// <summary>
        /// 已添加物料Id集合
        /// </summary>
        [Label("已添加物料Id集合")]
        public static readonly Property<double[]> FilterIdsProperty = P<ProdCpyItemCriteria>.Register(e => e.FilterIds);

        /// <summary>
        /// 已添加物料Id集合
        /// </summary>
        public double[] FilterIds
        {
            get { return this.GetProperty(FilterIdsProperty); }
            set { this.SetProperty(FilterIdsProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>物料列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BoxController>().GetItems(this);

        }
    }
}
