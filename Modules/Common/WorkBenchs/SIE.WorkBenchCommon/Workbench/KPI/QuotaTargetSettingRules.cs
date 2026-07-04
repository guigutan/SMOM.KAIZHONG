using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标定义不能重复规则
    /// </summary>
    [DisplayName("指标分配不能重复")]
    [Description("指标分配不能重复")]
    public class QuotaTargetSettingNotDuplicateRule : EntityRule<QuotaTargetSetting>
    {
        /// <summary>
        /// 
        /// </summary>
        public QuotaTargetSettingNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 指标周期不能重复验证规则
        /// </summary>
        /// <param name="entity">指标定义明细</param>
        /// <param name="e">规则参数</param>     
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var qt = entity as QuotaTargetSetting;

            bool exist = RT.Service.Resolve<QuotaTargetSettingController>().Exist(qt);
            if (exist) {
                e.BrokenDescription = "指标分配重复{0}".L10nFormat(qt.Code);
            }
        }
    }

    /// <summary>
    /// 指标定义明细不能重复规则
    /// </summary>
    [DisplayName("指标周期不能重复")]
    [Description("指标周期不能重复")]
    public class QuotaTargetDetailNotDuplicateRule : EntityRule<QuotaTargetDetail>
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public QuotaTargetDetailNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 指标周期不能重复验证规则
        /// </summary>
        /// <param name="entity">指标定义明细</param>
        /// <param name="e">规则参数</param>     
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var quotatargetdetail = entity as QuotaTargetDetail;
            if (quotatargetdetail.State == State.Enable)
            {
                if (quotatargetdetail.DataType == DateType.YEAR)
                {
                    int num = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetDetailYear(quotatargetdetail.Id, quotatargetdetail.Year, quotatargetdetail.QuotaTargetSettingId);
                    if (num > 0)
                    {
                        e.BrokenDescription = "指标年份{0} 重复!".L10nFormat(quotatargetdetail.Year);
                    }
                }
                else if (quotatargetdetail.DataType == DateType.MONTH)
                {
                    int num = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetDetailMonth(quotatargetdetail.Id, quotatargetdetail.Year, quotatargetdetail.Month, quotatargetdetail.QuotaTargetSettingId);
                    if (num > 0)
                    {
                        e.BrokenDescription = "指标{0}年第{1}月重复!".L10nFormat(quotatargetdetail.Year, quotatargetdetail.Month);
                    }
                }
                else if (quotatargetdetail.DataType == DateType.WEEK)
                {
                    int num = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaTargetDetailWeek(quotatargetdetail.Id, quotatargetdetail.Year, quotatargetdetail.Week, quotatargetdetail.QuotaTargetSettingId);
                    if (num > 0)
                    {
                        e.BrokenDescription = "指标{0}年第{1}周重复!".L10nFormat(quotatargetdetail.Year, quotatargetdetail.Week);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 指标周期不能为空规则
    /// </summary>
    [DisplayName("周期类型不能为空")]
    [Description("周期类型不能为空")]
    class QuotaTargetDetailNotNullRule : EntityRule<QuotaTargetDetail>
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
        /// <param name="entity">指标目标设定明细</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var quotatargetdetail = entity as QuotaTargetDetail;
            if (quotatargetdetail.QuotaTargetSetting.DataType == DateType.YEAR && quotatargetdetail.Year == null)
            {
                e.BrokenDescription = "年不能为空".L10N();
            }

            if (quotatargetdetail.QuotaTargetSetting.DataType == DateType.MONTH && (quotatargetdetail.Month == null || quotatargetdetail.Year == null))
            {
                e.BrokenDescription = "年月不能为空".L10N();
            }

            if (quotatargetdetail.QuotaTargetSetting.DataType == DateType.WEEK && (quotatargetdetail.Week == null || quotatargetdetail.Year == null))
            {
                e.BrokenDescription = "年周不能为空".L10N();
            }
        }
    }

    /// <summary>
    /// 指标定义明细目标值必须大于0规则
    /// </summary>
    [DisplayName("目标值不能低于0")]
    [Description("目标值不能低于0")]
    class QuotaTargetDetailBiggerThan0Rule : EntityRule<QuotaTargetDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QuotaTargetDetailBiggerThan0Rule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">指标定义标明细</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var quotatargetdetail = entity as QuotaTargetDetail;
            if (quotatargetdetail.Target <= 0)
            {
                e.BrokenDescription = "目标值需要大于0".L10N();
            }
        }
    }
}
