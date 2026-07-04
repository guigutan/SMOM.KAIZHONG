using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// 扣除分数上限必须大于零
    /// </summary>
    [DisplayName("扣除分数上限必须大于零")]
    [Description("请输入大于 0 的数字")]
    public class TpmScoreRateRule : EntityRule<TpmWeekInspectScore>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public TpmScoreRateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            //var item = entity as TpmWeekInspectScore;
            //if (item.ScoreRate <= 0)
            //    e.BrokenDescription = "项目[{0}]的扣除分数上限必须大于0".L10nFormat(item.ProjectName);
        }
    }

    /// <summary>
    /// 相同类型的数据，不允许项目重复
    /// </summary>
    [System.ComponentModel.DisplayName("相同类型的数据，不允许项目重复")]
    [System.ComponentModel.Description("相同类型的数据，不允许项目重复")]
    public class ProjectNameRule : EntityRule<TpmWeekInspectScore>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public ProjectNameRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var obj = entity as TpmWeekInspectScore;
            if (RT.Service.Resolve<TpmController>().GetProjectName(obj.ProjectName, obj.ScoreType,obj.Id))
            {
                e.BrokenDescription = "相同类型的数据，不允许项目重复".L10N();
            }

        }
    }

}
