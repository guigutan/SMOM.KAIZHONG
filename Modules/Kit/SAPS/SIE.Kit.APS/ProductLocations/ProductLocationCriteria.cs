using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.ProductLocations
{
    /// <summary>
    /// 产品定位查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产品定位查询")]
    public class ProductLocationCriteria : Criteria
    {
        #region 工厂
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<ProductLocationCriteria>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
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
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<ProductLocationCriteria>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

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
        public static readonly Property<Classification?> ClassificationProperty = P<ProductLocationCriteria>.Register(e => e.Classification);

        /// <summary>
        /// 分类
        /// </summary>
        public Classification? Classification
        {
            get { return GetProperty(ClassificationProperty); }
            set { SetProperty(ClassificationProperty, value); }
        }
        #endregion

        #region 分类值 TypeValue
        /// <summary>
        /// 分类值
        /// </summary>
        [Label("分类值")]
        public static readonly Property<string> TypeValueProperty = P<ProductLocationCriteria>.Register(e => e.TypeValue);

        /// <summary>
        /// 分类值
        /// </summary>
        public string TypeValue
        {
            get { return GetProperty(TypeValueProperty); }
            set { SetProperty(TypeValueProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProductLocationController>().GetProductLocationList(this);
        }
    }
}
