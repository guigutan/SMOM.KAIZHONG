using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产线产能
    /// </summary>
    [RootEntity, Serializable]
    [Label("产线产能")]
    public partial class ProductModelLineCapacity : DataEntity
    {
        #region 资源工时（单位/小时） WorkingHours
        /// <summary>
        /// 资源工时（单位/小时）
        /// </summary>
        [MaxValue(36000)]
        [MinValue(0)]
        [Required]
        [Label("资源工时（单位/小时）")]
        public static readonly Property<decimal?> WorkingHoursProperty = P<ProductModelLineCapacity>.Register(e => e.WorkingHours);

        /// <summary>
        /// 资源工时（单位/小时）
        /// </summary>
        public decimal? WorkingHours
        {
            get { return GetProperty(WorkingHoursProperty); }
            set { SetProperty(WorkingHoursProperty, value); }
        }
        #endregion

        #region 产品机型 ProductModel
        /// <summary>
        /// 产品机型Id
        /// </summary>
        public static readonly IRefIdProperty ProductModelIdProperty = P<ProductModelLineCapacity>.RegisterRefId(e => e.ProductModelId, ReferenceType.Parent);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double ProductModelId
        {
            get { return (double)GetRefId(ProductModelIdProperty); }
            set { SetRefId(ProductModelIdProperty, value); }
        }

        /// <summary>
        /// 产品机型
        /// </summary>
        public static readonly RefEntityProperty<ProductModel> ProductModelProperty = P<ProductModelLineCapacity>.RegisterRef(e => e.ProductModel, ProductModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel ProductModel
        {
            get { return GetRefEntity(ProductModelProperty); }
            set { SetRefEntity(ProductModelProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<ProductModelLineCapacity>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<ProductModelLineCapacity>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<ProductModelLineCapacity>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion 
    }

    /// <summary>
    /// 产线产能 实体配置
    /// </summary>
    internal class ProductModelLineCapacityConfig : EntityConfig<ProductModelLineCapacity>
    {
        /// <summary>
        /// 子类重写此方法，并完成对实体验证规则的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add(new NotDuplicateRule()
            {
                Properties =
                {
                    ProductModelLineCapacity.ResourceIdProperty,
                    ProductModelLineCapacity.ProductModelIdProperty,
                },
                MessageBuilder = (e) =>
                 {
                     return "产品机型与产线关系不能重复添加".L10N();
                 }
            });
            base.AddValidations(rules);
        }

        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRODUCT_MODEL_LINE_CAPACITY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 扩展产品机型属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class ProductModelLineCapacityDetailProperty
    {
        /// <summary>
        /// 扩展产品机型属性
        /// </summary>
        public static readonly ListProperty<EntityList<ProductModelLineCapacity>> ProductModelLineCapacityListProperty =
            P<ProductModel>.RegisterExtensionList<EntityList<ProductModelLineCapacity>>(" ProductModelLineCapacityList", typeof(ProductModelLineCapacityDetailProperty));

        /// <summary>
        /// 获取产线产能对象
        /// </summary>
        /// <param name="me">产品机型对象</param>
        /// <returns>返回产线产能对象</returns>
        public static EntityList<ProductModelLineCapacity> GetProductModelLineCapacity(ProductModel me)
        {
            return me.GetProperty(ProductModelLineCapacityListProperty);
        }

        /// <summary>
        /// 设置产线产能对象
        /// </summary>
        /// <param name="me">产品机型对象</param>
        /// <param name="value">需要设置的产线产能对象</param>
        public static void SetProductModelLineCapacity(ProductModel me, EntityList<ProductModelLineCapacity> value)
        {
            me.SetProperty(ProductModelLineCapacityListProperty, value);
        }

        /// <summary>
        /// 扩展产品机型属性 实体配置
        /// </summary>
        internal class ProductModelLineCapacityDetailPropertyConfig : EntityConfig<ProductModel>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(ProductModelLineCapacityDetailProperty.ProductModelLineCapacityListProperty).DontMapColumn();
            }
        }
    }
}