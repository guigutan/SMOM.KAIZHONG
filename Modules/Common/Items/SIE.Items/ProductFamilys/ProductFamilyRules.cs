using SIE.Domain.Validation;
using System.ComponentModel;

namespace SIE.Items.ProductFamilys
{
    /// <summary>
    /// 产品族分类验证规则，被产品族引用的不能删除
    /// </summary>
    [DisplayName("产品族分类验证规则")]
    [Description("被产品族引用的不能删除")]
    public class ProductFamilyRefCategoryRule : NoReferencedRule<ProductFamilyCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductFamilyRefCategoryRule()
        {
            Properties.Add(ProductFamily.CategoryIdProperty);
        }
    }
}