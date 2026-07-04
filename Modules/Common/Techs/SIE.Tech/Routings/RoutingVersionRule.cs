using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线版本删除验证规则
    /// </summary>
    [DisplayName("工艺路线版本删除验证规则")]
    [Description("发布状态的流程不能删除")]
    public class RoutingVersionDeleteRule : EntityRule<RoutingVersion>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public RoutingVersionDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工艺路线版本</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var version = entity as RoutingVersion;
            if (version.State == RoutingState.Release)
                e.BrokenDescription = "[{0}]状态的流程版本不允许删除".L10nFormat(version.State.ToLabel().L10N());
            if (version.ReferenceQty > 0)
                e.BrokenDescription = "流程版本已被引用不允许删除".L10N();
        }
    }
}