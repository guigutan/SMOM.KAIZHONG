using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ScoreRecords;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.RatedItems
{
    /// <summary>
    /// 评分项目验证规则
    /// </summary>
    [DisplayName("评分项目验证规则")]
    [Description("评分项目分值验证规则")]
    public class RatedItemScoreRule : EntityRule<RatedItem>
    {
        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var ratedItem = entity as RatedItem;
            if (ratedItem == null) return;
            if ((ratedItem.Code == "G0001" || ratedItem.Code == "G0002") && !ratedItem.IsSystem)
                e.BrokenDescription = "G0001和G0002是系统评分项目编码，不能用作自定义编码".L10N();
            if (ratedItem.MinScore == 0)
                e.BrokenDescription = "最小分值不能为0".L10N();
            if (ratedItem.MaxScore == 0)
                e.BrokenDescription = "最大分值不能为0".L10N();
            if ((ratedItem.MinScore < 0 && ratedItem.MaxScore > 0) || (ratedItem.MinScore > 0 && ratedItem.MaxScore < 0))
                e.BrokenDescription = "最小分值和最大分值必须都为负数或者都为正数".L10N();
            if (ratedItem.MinScore > ratedItem.MaxScore)
                e.BrokenDescription = "最小分值不能大于最大分值".L10N();
        }
    }

    /// <summary>
    /// 评分项目分类删除验证规则
    /// </summary>
    [DisplayName("评分项目分类删除验证规则")]
    [Description("评分项目分类被分类引用不能删除")]
    public class CategoryNoRefRatedItemRule : NoReferencedRule<RatedItemCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CategoryNoRefRatedItemRule()
        {
            Properties.Add(RatedItem.CategoryIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，评分项目分类[{0}]被评分引用[{1}]次".L10nFormat((e as RatedItemCategory)?.Name, i);
            };
        }
    }


    /// <summary>
    /// 评分项目删除验证规则
    /// </summary>
    [DisplayName("评分项目删除验证规则")]
    [Description("评分项目被评分记录引用不能删除")]
    public class RatedItemNoRefRecordRule : NoReferencedRule<RatedItem>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RatedItemNoRefRecordRule()
        {
            Properties.Add(ScoreRecord.RatedItemIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，评分项目[{0}]被评分记录引用[{1}]次".L10nFormat((e as RatedItem)?.Name, i);
            };
        }
    }
}