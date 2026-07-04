using SIE.Domain.Validation;
using SIE.Items;
using System.ComponentModel;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 产品族分类验证规则，被工艺路线引用的不能删除
    /// </summary>
    [DisplayName("产品族分类验证规则")]
    [Description("被工艺路线引用的不能删除")]
    public class RoutingRefProductFamilyCategoryRule : NoReferencedRule<ProductFamilyCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutingRefProductFamilyCategoryRule()
        {
            Properties.Add(Routing.CategoryIdProperty);
        }
    }
}