using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 复核包装规则明细不能拥有重复的箱型
    /// </summary>
    [DisplayName("空客户验证规则")]
    [Description("只能存在行空客户数据验证规则")]
    public class EmptyCustomerRule : EntityRule<RePackageRule>
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var rule = entity as RePackageRule;
            if (rule.CustomerId.HasValue && rule.CustomerId > 0)
                return;
            var count = RT.Service.Resolve<RePackageController>().GetNullCustomerCount(rule.Id);
            if (count > 0)
                e.BrokenDescription = "只能存在一笔客户为空的规则".L10N();
        }
    }

    /// <summary>
    /// 复核包装规则明细不能拥有重复的箱型
    /// </summary>
    [DisplayName("复核包装规则明细验证规则")]
    [Description("复核包装规则不能拥有重复的箱型")]
    public class RePackageRuleDetailNotDuplicateRule : NotDuplicateRule<RePackageRuleDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RePackageRuleDetailNotDuplicateRule()
        {
            Properties.Add(RePackageRuleDetail.RePackageRuleIdProperty);
            Properties.Add(RePackageRuleDetail.BoxTypeProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as RePackageRuleDetail;
                return "已经存在箱型[{0}]".L10nFormat(entity.BoxType.ToLabel());
            };
        }
    }

    /// <summary>
    /// 品类混放验证规则
    /// </summary>
    [DisplayName("品类混放验证规则")]
    [Description("不允许品类混放时，分类层级必填据验证规则")]
    public class MixedTypeRule : EntityRule<RePackageRuleDetail>
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var dtl = entity as RePackageRuleDetail;
            if (dtl.MixedType == MixedType.NoAllow && dtl.ItemCategoryLevelId.Value <= 0)
            {
                e.BrokenDescription = "不允许品类混放时，分类层级不可为空的规则".L10N();
            }
        }
    }
}
