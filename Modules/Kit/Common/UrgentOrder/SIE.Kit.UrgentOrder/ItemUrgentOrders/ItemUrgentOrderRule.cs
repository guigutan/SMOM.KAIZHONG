using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders
{
    /// <summary>
    /// 物料加急单验证规则
    /// </summary>
    [DisplayName("需求数量验证")]
    [Description("需求数量必须大于0")]
    public class ItemUrgentOrderQtyRule : EntityRule<ItemUrgentOrder>
    {
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var itemUrgentOrder = entity as ItemUrgentOrder;
            if (itemUrgentOrder.Qty <= 0)
            {
                e.BrokenDescription = "需求数量必须大于0!".L10N();
            }
        }
    }

    /// <summary>
    /// 物料加急单验证规则
    /// </summary>
    [DisplayName("需求时间验证")]
    [Description("需求时间不能为空")]
    public class ItemUrgentOrderTimeRule : EntityRule<ItemUrgentOrder>
    {
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var itemUrgentOrder = entity as ItemUrgentOrder;
            DateTime dt = Convert.ToDateTime("2000/1/1 00:00:00");
            if (itemUrgentOrder.DemandTime == dt)
            {
                e.BrokenDescription = "需求时间不能为空!".L10N();
            }
        }
    }
}
