using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 产品容量实体规则
    /// </summary>
    [DisplayName("产品容量验证")]
    [Description("载具不能重复配置相同物料的产品容量")]
    public class ProductCapacityItemRule : PropertyRule<ProductCapacity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductCapacityItemRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return ProductCapacity.ItemProperty;
            }
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var productCapacity = entity as ProductCapacity;

            if (productCapacity.TurnoverBox.CapacityList.Any(p => p.ItemId == productCapacity.ItemId && p.Id != productCapacity.Id))
                e.BrokenDescription = "载具{0}已经存在物料{1}的产品容量".L10nFormat(productCapacity.TurnoverBox.Code, productCapacity.Item.Name);
        }
    }
}
