using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.ProductBoms;
using SIE.MetaModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 产品BOM组合替代唯一性验证
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM组合替代验证规则")]
    [System.ComponentModel.Description("产品BOM组合替代唯一性验证")]
    public class CombinationReplateUnique : EntityRule<CombinationReplate>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CombinationReplateUnique()
        {
            Scope = Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">验证规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var comReplate = entity as CombinationReplate;
            if (RT.Service.Resolve<ProductBomController>().ValidateCombinationReplate(comReplate))
            {
                e.BrokenDescription = "主物料、物料、属性组、点位的组合替代不允许重复".L10N();
            }
        }
    }

}