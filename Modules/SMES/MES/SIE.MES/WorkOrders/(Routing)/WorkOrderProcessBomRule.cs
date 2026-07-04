using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工序BOM验证单机定额必须大于0
    /// </summary>
    [System.ComponentModel.DisplayName("工序BOM验证规则")]
    [System.ComponentModel.Description("工序BOM单机定额必须大于0")]
    public class WorkOrderProcessBomRequired : PropertyRule<WorkOrderProcessBom>
    {
        /// <summary>
        /// 要验证的属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return WorkOrderProcessBom.SingleQtyProperty;
            }
        }

        /// <summary>
        /// 大于0验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">e</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var processBom = entity as WorkOrderProcessBom;
            if (processBom.SingleQty <= 0)
            {
                e.BrokenDescription = "物料[{0}]，工序[{1}]单位耗用量必须大于0".L10nFormat(processBom.Item?.Code, processBom.Process?.Code);
            }
        }
    }
}
