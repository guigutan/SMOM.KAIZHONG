using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.WorkBenchCommon.Workbench.KPI;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 指标定义明细不能重复规则
    /// </summary>
    [DisplayName("指标分配不能重复")]
    [Description("指标分配不能重复")]
    public class TargetSettingNotDuplicateRule : NotDuplicateRule<TargetWarnSetting>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public TargetSettingNotDuplicateRule()
        {
            Properties.Add(TargetWarnSetting.CodeProperty);
            Properties.Add(TargetWarnSetting.NameProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as TargetWarnSetting;
                return "{0}重复".L10nFormat(r.Name);
            };
        }
    }

    /// <summary>
    /// 目标条件验证规则
    /// </summary>
    [DisplayName("目标条件不重复规则")]
    [Description("目标条件不重复")]

    public class TargetWarnTargetOpetatorsNotRepeat : NotDuplicateRule<TargetWarnDetail>
    {
        /// <summary>
        /// 目标条件验证规则 
        /// </summary>
        public TargetWarnTargetOpetatorsNotRepeat()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            Properties.Add(TargetWarnDetail.TargetWarnSettingIdProperty);
            Properties.Add(TargetWarnDetail.TargetOpetatorsProperty);
            this.MessageBuilder = e =>
            {
                return "达成率目标设定条件重复".L10N();
            };
        }
    }

    /// <summary>
    /// 达成率目标值大于等于0，小于等于100
    /// </summary>
    [DisplayName("达成率目标值大于等于0，小于等于100规则")]
    [Description("达成率目标值大于等于0，小于等于100")]
    public class TargetWarnDetailValueLimitRulers : EntityRule<TargetWarnDetail>
    {
        /// <summary>
        /// 保存或添加时验证规则
        /// </summary>
        public TargetWarnDetailValueLimitRulers()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 达成率目标值大于等于0，小于等于100
        /// </summary>
        /// <param name="entity">达成率目标区间实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var targetwarnset = entity as TargetWarnDetail;
            if (targetwarnset.MinValue < 0 || targetwarnset.MinValue > 100)
            {
                e.BrokenDescription = "达成率目标值需大于等于0，小于等于100".L10N();
            }
            else if (targetwarnset.MaxValue < 0 || targetwarnset.MaxValue > 100)
            {
                e.BrokenDescription = "达成率目标值需大于等于0，小于等于100".L10N();
            }
        }
    }

    /// <summary>
    /// 达成率目标值区间不能重合
    /// </summary>
    [DisplayName("达成率目标值区间不能重合规则")]
    [Description("达成率目标值区间不能重合")]
    public class TargetWarnDetailMaxandMinValueRulers : EntityRule<TargetWarnSetting>
    {
        /// <summary>
        /// 保存或添加时验证规则
        /// </summary>
        public TargetWarnDetailMaxandMinValueRulers()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 达成率目标值区间不能重合
        /// </summary>
        /// <param name="entity">达成率目标区间实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var targetwarnset = entity as TargetWarnSetting;
            if (targetwarnset == null) return;

            var detail = targetwarnset.TargetWarnDetailList;
            if (detail.Count < 2) return;

            if (detail?.Count(p => p.MinValue.HasValue && p.MaxValue.HasValue && p.MaxValue <= p.MinValue) > 0)
            {
                e.BrokenDescription = "目标最大值必须大于最小值".L10N();
                return;
            }

            TargetWarnDetail lessTWDetail = targetwarnset.TargetWarnDetailList.FirstOrDefault(p => p.TargetOpetators == TargetOpetators.LessOrEqual);
            TargetWarnDetail betweenTWDetail = targetwarnset.TargetWarnDetailList.FirstOrDefault(p => p.TargetOpetators == TargetOpetators.Between);
            TargetWarnDetail greaterTWDetail = targetwarnset.TargetWarnDetailList.FirstOrDefault(p => p.TargetOpetators == TargetOpetators.GreaterOrEqual);
            if (lessTWDetail != null && ((betweenTWDetail != null && lessTWDetail.MinValue > betweenTWDetail.MinValue) || (greaterTWDetail != null && lessTWDetail.MinValue >= greaterTWDetail.MaxValue)))
            {
                e.BrokenDescription = "达成率目标值区间不能重合".L10N();
                return;
            }

            if (betweenTWDetail != null && greaterTWDetail != null && betweenTWDetail.MaxValue > greaterTWDetail.MaxValue)
            {
                e.BrokenDescription = "达成率目标值区间不能重合".L10N();
            }
        }
    }

    /// <summary>
    /// 指标预警设定最小值、最大值不能为空规则
    /// </summary>
    [DisplayName("最小值、最大值不能为空")]
    [Description("最小值、最大值不能为空")]
    class QuotaTargetDetailNotNullRule : EntityRule<TargetWarnDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QuotaTargetDetailNotNullRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">预警目标设定明细</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var targetwarndetail = entity as TargetWarnDetail;

            if (targetwarndetail.TargetOpetators == TargetOpetators.Between)
            {
                if (targetwarndetail.MaxValue == null || targetwarndetail.MinValue == null)
                {
                    e.BrokenDescription = "值1或值2不能为空".L10N();
                }
            }
            else if (targetwarndetail.TargetOpetators == TargetOpetators.LessOrEqual)
            {
                if (targetwarndetail.MinValue == null)
                {
                    e.BrokenDescription = "值1不能为空".L10N();
                }
            }
            else
            {
                if (targetwarndetail.MaxValue == null)
                {
                    e.BrokenDescription = "值2不能为空".L10N();
                }
            }
        }
    }


  
}
