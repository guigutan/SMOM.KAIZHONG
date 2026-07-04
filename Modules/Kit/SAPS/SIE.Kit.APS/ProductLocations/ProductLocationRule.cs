using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Kit.APS.ProductLocations
{
    /// <summary>
    /// 产品定位验证规则
    /// </summary>
    [DisplayName("产品定位验证")]
    [Description("产品定位不能重复")]
    public class ProductLocationRule : EntityRule<ProductLocation>
    {
        /// <summary>
        /// 验证范围
        /// </summary>
        public ProductLocationRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 验证逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var prod = entity as ProductLocation;
            var count = RT.Service.Resolve<ProductLocationController>().ValidateProductLocation(prod);
            if (!count)
            {
                e.BrokenDescription = "工厂[{0}]产品定位分类[{1}]不能重复!".L10nFormat(prod.Enterprise.Name, prod.Classification.ToLabel());
            }
        }
    }


    ///// <summary>
    ///// 产品定位最大值不能小于或等于最小值
    ///// </summary>
    //[DisplayName("产品定位最大值最小值验证")]
    //[Description("产品定位分类为特殊工艺时最大值不能小于或等于最小值")]
    //public class ProductLocationMinAndMaxRule : EntityRule<ProductLocation>
    //{
    //    /// <summary>
    //    /// 验证范围
    //    /// </summary>
    //    public ProductLocationMinAndMaxRule()
    //    {
    //        Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
    //    }
    //    /// <summary>
    //    /// 验证逻辑
    //    /// </summary>
    //    /// <param name="entity">实体</param>
    //    /// <param name="e">规则参数</param>
    //    protected override void Validate(IEntity entity, RuleArgs e)
    //    {
    //        var prod = entity as ProductLocation;
    //        if (prod.Classification == Classification.SpecialProcess && prod.MaxValue <= prod.MinValue)
    //        {
    //           e.BrokenDescription = "产品定位最大值不能小于或等于最小值".L10N();
    //        }
            
    //    }
    //}

    /// <summary>
    /// 产品定位验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("产品定位最大值最小值验证")]
    [System.ComponentModel.Description("分类为特殊工艺时最大值最小值必填")]
    public class ProductLocationMinAndMaxNullRule : PropertyRule<ProductLocation>
    {
        /// <summary>
        /// 托管属性是校验类别
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return ProductLocation.ClassificationProperty;
            }
        }
        /// <summary>
        /// 验证方法逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var prod = entity as ProductLocation;
            //if (prod.Classification == Classification.SpecialProcess && (prod.MinValue == null || prod.MaxValue == null))
            //{
            //    e.BrokenDescription = "分类为特殊工艺时,最小值最大值必填！".L10N();
            //}
            if ( prod.MinValue != null || prod.MaxValue != null)
            {
                e.BrokenDescription = "分类非特殊工艺时,最小值最大值为空！".L10N();
            }
        }
    }
}
