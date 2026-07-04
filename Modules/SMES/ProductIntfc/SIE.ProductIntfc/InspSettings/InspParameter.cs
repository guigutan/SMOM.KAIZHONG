using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.ProductIntfc.InspSettings
{
    /// <summary>
    /// 报检参数
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("报检参数")]
    [DisplayMember(nameof(Id))]
    public partial class InspParameter : DataEntity
    {
        #region 报检参数 InspParm
        /// <summary>
        /// 报检参数
        /// </summary>
        [Label("报检参数")]
        public static readonly Property<int> InspParmProperty = P<InspParameter>.Register(e => e.InspParm);

        /// <summary>
        /// 报检参数
        /// </summary>
        public int InspParm
        {
            get { return GetProperty(InspParmProperty); }
            set { SetProperty(InspParmProperty, value); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<InspProcess> ProcessTypeProperty = P<InspParameter>.Register(e => e.ProcessType);

        /// <summary>
        /// 工序类型
        /// </summary>
        public InspProcess ProcessType
        {
            get { return GetProperty(ProcessTypeProperty); }
            set { SetProperty(ProcessTypeProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<InspParameter>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<InspParameter>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<InspParameter>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 报检维度 InspDimension
        /// <summary>
        /// 报检维度
        /// </summary>
        [Label("报检维度")]
        public static readonly Property<InspDimension> InspDimensionProperty = P<InspParameter>.Register(e => e.InspDimension);

        /// <summary>
        /// 报检维度
        /// </summary>
        public InspDimension InspDimension
        {
            get { return GetProperty(InspDimensionProperty); }
            set { SetProperty(InspDimensionProperty, value); }
        }
        #endregion

        #region 报检工序 InspProcess
        /// <summary>
        /// 报检工序Id
        /// </summary>
        [Label("报检工序")]
        public static readonly IRefIdProperty InspProcessIdProperty = P<InspParameter>.RegisterRefId(e => e.InspProcessId, ReferenceType.Normal);

        /// <summary>
        /// 报检工序Id
        /// </summary>
        public double? InspProcessId
        {
            get { return (double?)GetRefNullableId(InspProcessIdProperty); }
            set { SetRefNullableId(InspProcessIdProperty, value); }
        }

        /// <summary>
        /// 报检工序
        /// </summary>
        public static readonly RefEntityProperty<Process> InspProcessProperty = P<InspParameter>.RegisterRef(e => e.InspProcess, InspProcessIdProperty);

        /// <summary>
        /// 报检工序
        /// </summary>
        public Process InspProcess
        {
            get { return GetRefEntity(InspProcessProperty); }
            set { SetRefEntity(InspProcessProperty, value); }
        }
        #endregion

        #region 报检类型 InspType
        /// <summary>
        /// 报检类型
        /// </summary>
        [Label("报检类型")]
        public static readonly Property<InspType> InspTypeProperty = P<InspParameter>.Register(e => e.InspType);

        /// <summary>
        /// 报检类型
        /// </summary>
        public InspType InspType
        {
            get { return GetProperty(InspTypeProperty); }
            set { SetProperty(InspTypeProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty =
            P<InspParameter>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族
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
            P<InspParameter>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return this.GetRefEntity(ProductFamilyProperty); }
            set { this.SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 产品族名称 ProductName
        /// <summary>
        /// 产品族名称
        /// </summary>
        [Label("产品族名称")]
        public static readonly Property<string> ProductFamilyNameProperty = P<InspParameter>.RegisterView(e => e.ProductFamilyName, p => p.ProductFamily.Name);

        /// <summary>
        /// 产品族名称
        /// </summary>
        public string ProductFamilyName
        {
            get { return this.GetProperty(ProductFamilyNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 报检参数 实体配置
    /// </summary>
    internal class InspParameterConfig : EntityConfig<InspParameter>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_INSP_PARM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}