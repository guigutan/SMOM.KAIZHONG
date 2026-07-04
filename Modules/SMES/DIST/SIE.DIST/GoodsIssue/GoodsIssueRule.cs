using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;

namespace SIE.DIST
{
    /// <summary>
    /// 工单发料属性值不能相同
    /// </summary>
    [System.ComponentModel.DisplayName("工单发料属性值验证规则")]
    [System.ComponentModel.Description("属性值不能相同")]
    public class GoodsIssuePropertyValueNotDuplicateRule : NotDuplicateRule<GoodsIssuePropertyValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GoodsIssuePropertyValueNotDuplicateRule()
        {
            Properties.Add(GoodsIssuePropertyValue.DefinitionIdProperty);
            Properties.Add(GoodsIssuePropertyValue.ValueProperty);
            Properties.Add(GoodsIssuePropertyValue.GoodsIssueIdProperty);
            MessageBuilder = (e) =>
            {
                return "该发料属性的属性值已经存在".L10N();
            };
        }
    }

    /// <summary>
    /// 发料单配送数量必须大于0
    /// </summary>
    [System.ComponentModel.DisplayName("配送数量验证规则")]
    [System.ComponentModel.Description("配送数量必须大于0")]
    public class GoodsIssueDistributionQtyRule : EntityRule<GoodsIssue>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var goodsIssue = entity as GoodsIssue;
            if (goodsIssue != null && goodsIssue.Qty <= 0)
            {
                e.BrokenDescription = "配送数量必须大于0".L10N();
            }
        }
    }
}
