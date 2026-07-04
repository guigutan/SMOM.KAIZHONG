using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Items.Items
{
    /// <summary>
    /// 产品等级验证规则
    /// </summary>
    [DisplayName("产品等级验证规则")]
    [Description("同一物料不能存在相同产品等级")]
    public class ProductGradeRule : EntityRule<ProductGrade>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public ProductGradeRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">产品等级实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var prdGrd = entity as ProductGrade;
            var count = RT.Service.Resolve<ItemController>().CountProductGrade(prdGrd.ItemId, prdGrd.Code);
            if (count > 0)
            {
                e.BrokenDescription = "同一物料不能存在相同产品等级".L10N();
            }
        }
    }
}
