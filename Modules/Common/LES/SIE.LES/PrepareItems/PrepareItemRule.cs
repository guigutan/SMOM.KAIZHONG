using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.Commons;
using SIE.MetaModel;
using System;

namespace SIE.LES
{
    /// <summary>
    /// 物料类型或物料编码必填
    /// </summary>
    [System.ComponentModel.DisplayName("物料类型或物料编码必填")]
    [System.ComponentModel.Description("物料类型或物料编码必填")]
    public class ItemTypeNotNullRule : EntityRule<BasePrepareItem>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as BasePrepareItem;
            if (!pro.ItemCategoryId.HasValue && !pro.ItemId.HasValue)
                e.BrokenDescription = "物料类型或物料编码不能为空！".L10N();
        }
    }

    /// <summary>
    /// 最高存量必填
    /// </summary>
    [System.ComponentModel.DisplayName("最高存量必填")]
    [System.ComponentModel.Description("最高存量必填")]
    public class MaxStockNotNullRule : EntityRule<PrepareItemPull>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as PrepareItemPull;
            if (pro.MaxStock == null && pro.DemandType == DemandMode.MaxStock)
                e.BrokenDescription = "最高存量不能为空！".L10N();
            if (pro.TriggerType != TriggerMode.ManualModel && !pro.LowestStage.HasValue)
                e.BrokenDescription = "非手工触发，最低安全水位不能为空！".L10N();
        }
    }

    /// <summary>
    /// 固定量必填
    /// </summary>
    [System.ComponentModel.DisplayName("固定量必填")]
    [System.ComponentModel.Description("固定量必填")]
    public class FixedQuantityNotNullRule : EntityRule<PrepareItemPull>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as PrepareItemPull;
            if (pro.FixedQuantity == null && pro.DemandType == DemandMode.FixedQuantity)
                e.BrokenDescription = "固定量不能为空！".L10N();
        }
    }

    /// <summary>
    /// 提前小时必填
    /// </summary>
    [System.ComponentModel.DisplayName("提前小时必填")]
    [System.ComponentModel.Description("提前小时必填")]
    public class AdvanceHoursNotNullRule : EntityRule<PrepareItemPush>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as PrepareItemPush;
            if (pro.TriggerType != TriggerMode.ManualModel && (!pro.AdvanceHours.HasValue || pro.AdvanceHours.Value <= 0))
                e.BrokenDescription = "提前小时不能为空且必须大于0！".L10N();
        }
    }

    /// <summary>
    /// 最短满足时间必填
    /// </summary>
    [System.ComponentModel.DisplayName("最短满足时间必填")]
    [System.ComponentModel.Description("最短满足时间必填")]
    public class SatisfactionTimeNotNullRule : EntityRule<PrepareItemPush>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as PrepareItemPush;
            if (pro.SatisfactionTime == null && pro.TriggerType == TriggerMode.InvBelowSafeLevelToBeat)
                e.BrokenDescription = "最短满足时间不能为空！".L10N();
        }
    }

    /// <summary>
    /// 最小备料时间必须大于最短满足时间
    /// </summary>
    [System.ComponentModel.DisplayName("最小备料时间必须大于最短满足时间")]
    [System.ComponentModel.Description("最小备料时间必须大于最短满足时间")]
    public class TimeRule : EntityRule<PrepareItemPush>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as PrepareItemPush;
            if (pro.PreparationTime.HasValue && pro.SatisfactionTime.HasValue && pro.PreparationTime.Value <= pro.SatisfactionTime.Value)
                e.BrokenDescription = "最小备料时间必须大于最短满足时间！".L10N();
        }
    }

    /// <summary>
    /// 固定量必填
    /// </summary>
    [System.ComponentModel.DisplayName("固定量必填")]
    [System.ComponentModel.Description("固定量必填")]
    public class SafeLevelNotNullRule : EntityRule<PrepareItemPush>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as PrepareItemPush;
            if (pro.FixedQuantity == null && pro.DemandType == DemandMode.FixedQuantity)

                e.BrokenDescription = "固定量不能为空！".L10N();
        }
    }

    /// <summary>
    /// 最小备料时间必填
    /// </summary>
    [System.ComponentModel.DisplayName("最小备料时间必填")]
    [System.ComponentModel.Description("最小备料时间必填")]
    public class PreparationTimeNotNullRule : EntityRule<PrepareItemPush>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var pro = entity as PrepareItemPush;
            if (pro.PreparationTime == null && (pro.DemandType == DemandMode.StockIsSafeLevelForBeat || pro.DemandType == DemandMode.StockToSafeLevelForBeat))
                e.BrokenDescription = "最小备料时间不能为空！".L10N();
        }
    }
}
