using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.TeamManagement;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品直通率报表自定义查询
    /// </summary>
    [QueryEntity, Serializable]
    public class ProductReportViewModelCriteria : Criteria
    {
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        public ProductReportViewModelCriteria()
        {
            CollectDate = new DateRange();
        }

        #region 产品机型 ProductModel
        /// <summary>
        /// 产品机型Id
        /// </summary>
        [Label("产品机型")]
        public static readonly IRefIdProperty ProductModelIdProperty =
            P<ProductReportViewModelCriteria>.RegisterRefId(e => e.ProductModelId, ReferenceType.Normal);

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
            P<ProductReportViewModelCriteria>.RegisterRef(e => e.ProductModel, ProductModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel ProductModel
        {
            get { return this.GetRefEntity(ProductModelProperty); }
            set { this.SetRefEntity(ProductModelProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<ProductReportViewModelCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<ProductReportViewModelCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 日期 CollectDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateRange> CollectDateProperty = P<ProductReportViewModelCriteria>.Register(e => e.CollectDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateRange CollectDate
        {
            get { return this.GetProperty(CollectDateProperty); }
            set { this.SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 类型 DateType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<DateType> DateTypeProperty = P<ProductReportViewModelCriteria>.Register(e => e.DateType);

        /// <summary>
        /// 类型
        /// </summary>
        public DateType DateType
        {
            get { return this.GetProperty(DateTypeProperty); }
            set { this.SetProperty(DateTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>实体列表</returns>
        protected override EntityList Fetch()
        {
            var modelList = new EntityList<ProductReportViewModel>();
            modelList.Add(RT.Service.Resolve<ProdReportViewModelController>().GetProdReportViewModel(this));
            return modelList;
        }
    }
}
