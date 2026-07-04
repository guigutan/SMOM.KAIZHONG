using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using System;

namespace SIE.MES.TaskManagement.Specifications
{
    #region 规格分类
    /// <summary>
    /// 关联规格分类不能删除--规格件清单
    /// </summary>
    [System.ComponentModel.DisplayName("规格分类删除实体验证规则")]
    [System.ComponentModel.Description("关联规格分类不能删除")]
    public class SpecificationCategoryDeleteRule : NoReferencedRule<SpecificationCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SpecificationCategoryDeleteRule()
        {
            Properties.Add(Specification.CategoryIdProperty);
            MessageBuilder = (o, e) =>
            {
                var specificationCategory = o as SpecificationCategory;
                return "规格分类[{0}]已关联规格件清单，不允许删除".L10nFormat(specificationCategory.Name);
            };
        }
    }
    #endregion

    #region 规格件清单
    /// <summary>
    /// 关联规格件清单不能删除--产品规格件清单
    /// </summary>
    [System.ComponentModel.DisplayName("规格件清单删除实体验证规则")]
    [System.ComponentModel.Description("关联规格件清单不能删除")]
    public class SpecificationDeleteRule : NoReferencedRule<Specification>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SpecificationDeleteRule()
        {
            Properties.Add(ProductSpecificationDetail.SpecificationIdProperty);
            MessageBuilder = (o, e) =>
            {
                var specification = o as Specification;
                return "规格件清单[{0}]已关联产品规格件清单，不允许删除".L10nFormat(specification.Name);
            };
        }
    }
    #endregion

    #region 物料
    /// <summary>
    /// 物料不能删除--产品规格件对照表
    /// </summary>
    [System.ComponentModel.DisplayName("物料删除实体验证规则")]
    [System.ComponentModel.Description("关联物料不能删除")]
    public class ItemOfProductSpecificationDeleteRule : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemOfProductSpecificationDeleteRule()
        {
            Properties.Add(ProductSpecification.ProductIdProperty);
            MessageBuilder = (o, e) =>
            {
                var item = o as Item;
                return "物料[{0}]已关联产品规格件对照表，不允许删除".L10nFormat(item.Name);
            };
        }
    }
    #endregion

    #region
    /// <summary>
    /// 产品规格件清单单体定额必须大于0
    /// </summary>
    [System.ComponentModel.DisplayName("产品规格件清单验证规则")]
    [System.ComponentModel.Description("产品规格件清单单体定额必须大于0")]
    public class ProductSpecificationDetailRule : EntityRule<ProductSpecificationDetail>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">IEntity</param>
        /// <param name="e">RuleArgs</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var detail = entity as ProductSpecificationDetail;
            if (detail != null && detail.Qty <= 0)
            {
                e.BrokenDescription = "产品规格件清单[{0}]单体定额必须大于0".L10nFormat(detail.Specification.Name);
            }
        }
    }
    #endregion
}
