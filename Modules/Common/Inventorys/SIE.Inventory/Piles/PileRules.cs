using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 周转容器、非周转容器验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("周转容器、非周转容器验证规则")]
    [System.ComponentModel.Description("周转容器、非周转容器验证规则")]
    public class PileTurnBoxRule : EntityRule<Pile>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PileTurnBoxRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            ////var pile = entity as Pile;
            ////if (pile.TurnoverContainer)
            ////{
            ////    if (!pile.TurnoverBoxId.HasValue && pile.TurnoverBoxId <= 0)
            ////        e.BrokenDescription = "周转容器的周转箱不能为空".L10N();
            ////}
            ////else
            ////{
            ////    if (!pile.PileState.HasValue)
            ////        e.BrokenDescription = "非周转容器的垛状态不能为空".L10N();
            ////}
        }
    }
}
