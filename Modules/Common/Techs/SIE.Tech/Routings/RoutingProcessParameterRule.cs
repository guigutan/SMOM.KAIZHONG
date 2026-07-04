using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Tech.Processs;
using System;
using System.ComponentModel;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序清单参数验证规则
    /// </summary>
    [DisplayName("工序清单参数新增&修改验证规则")]
    [Description("工艺路线的最后一个工序结果不能是Fail")]
    class RoutingProcessParameterRule : EntityRule<RoutingProcessParameter>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public RoutingProcessParameterRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工序清单参数</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var routingProcessParameter = entity as RoutingProcessParameter;
            if (routingProcessParameter.Type == ResultTypeForDesign.Fail && routingProcessParameter.NextProcess == null && routingProcessParameter.RoutingProcess != null)
            {
                e.BrokenDescription = "工序关系有误，最后一个采集工序的Fail结果不可以完工：{0}".L10nFormat(routingProcessParameter.RoutingProcess.Name);
            }
        }
    }
}
