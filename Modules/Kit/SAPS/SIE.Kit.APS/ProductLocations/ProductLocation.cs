using SIE.DataAuth;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.ProductLocations
{
    /// <summary>
    /// 产品定位
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductLocationCriteria))]
    [EntityDataAuthAttribute(typeof(EmployeeEnterprise), nameof(EnterpriseId), true)]
    [Label("产品定位")]
    public partial class ProductLocation : DataEntity
    {
        /// <summary>
        /// 分类值快码
        /// </summary>
        public const string CLASSIFICATIONVALUE = "CLASSIFICATION_VALUE";

        #region 工厂 EnterpriseId
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<ProductLocation>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public double EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<ProductLocation>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 分类 Classification
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<Classification> ClassificationProperty = P<ProductLocation>.Register(e => e.Classification);

        /// <summary>
        /// 分类
        /// </summary>
        public Classification Classification
        {
            get { return GetProperty(ClassificationProperty); }
            set { SetProperty(ClassificationProperty, value); }
        }
        #endregion

        #region 分类值 TypeValue
        /// <summary>
        /// 分类值
        /// </summary>
        [Required]
        [MaxLength(80)]
        [Label("分类值")]
        public static readonly Property<string> TypeValueProperty = P<ProductLocation>.Register(e => e.TypeValue);

        /// <summary>
        /// 分类值
        /// </summary>
        public string TypeValue
        {
            get { return GetProperty(TypeValueProperty); }
            set { SetProperty(TypeValueProperty, value); }
        }
        #endregion

        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<ProductLocation>.Register(e => e.MinValue);

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 最大值  MaxValue
        /// <summary>
        /// 最大值 
        /// </summary>
        [Label("最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<ProductLocation>.Register(e => e.MaxValue);

        /// <summary>
        /// 最大值 
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProductLocation>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品定位 实体配置
    /// </summary>
    internal class ProductLocationConfig : EntityConfig<ProductLocation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_PRODUCT_LOCATION").MapAllProperties();
            Meta.Property(ProductLocation.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}