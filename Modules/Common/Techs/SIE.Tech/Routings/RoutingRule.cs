using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线删除验证规则
    /// </summary>
    [DisplayName("工艺路线删除验证规则")]
    [Description("工艺路线下存在版本不能删除")]
    public class RoutingDeleteRule : EntityRule<Routing>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public RoutingDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工艺路线</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var routing = entity as Routing;
            if (routing.VersionList.Count > 0)
            {
                e.BrokenDescription = "不能删除,工艺路线下面存在 {0} 个版本".L10nFormat(routing.VersionList.Count);
            }
        }
    }
}