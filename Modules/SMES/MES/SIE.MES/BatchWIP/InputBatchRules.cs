using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 转出批次数量验证规则
    /// </summary>
    [DisplayName("转入批次拆分数量验证规则")]
    [Description("拆分数量不能大于剩余数量")]
    public class InputBatchSplitQtyRule : PropertyRule<InputBatch>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return InputBatch.SplitQtyProperty;
            }
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">转入批次</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var batch = entity as InputBatch;
            if (batch.SplitQty > batch.RemainQty)
                e.BrokenDescription = "拆分数量不能大于剩余容量[{0}]".L10nFormat(batch.RemainQty);
        }
    }
}