using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 产品工艺路线验证规则
    /// </summary>
    [DisplayName("产品工艺路线验证规则")]
    [Description("同一有效期内不能存在相同产品工艺路线")]
    public class ProductRoutingRule : EntityRule<ProductRouting>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public ProductRoutingRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">产品工艺路线</param>
        /// <param name="e">参数 </param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var routing = entity as ProductRouting;
            if (RT.Service.Resolve<RoutingSettingController>().CountProductRouting(routing) > 0)
            {
                e.BrokenDescription = "同一产品、工单类型、工段、项目号、时间范围内产品工艺路线只能有一条".L10N();
            }
        }
    }

    /// <summary>
    /// 物料删除验证规则，被产品工艺路线设置引用的不能删除
    /// </summary>
    [DisplayName("物料删除验证规则")]
    [Description("被产品工艺路线设置引用的不能删除")]
    public class ProductRountingRefItemRule : NoReferencedRule<Item>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductRountingRefItemRule()
        {
            Properties.Add(ProductRouting.ProductIdProperty);
        }
    }
}
