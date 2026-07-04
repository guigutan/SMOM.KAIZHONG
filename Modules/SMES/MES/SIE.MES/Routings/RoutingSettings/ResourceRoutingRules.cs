using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;

namespace SIE.MES.RoutingSettings
{
    /// <summary>
    /// 产线工艺路线验证规则
    /// </summary>
    [DisplayName("产线工艺路线验证规则")]
    [Description("同一有效期内不能存在相同产线工艺路线")]
    public class ResourceRoutingRule : EntityRule<ResourceRouting>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">产线工艺路线</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var routing = entity as ResourceRouting;
            if (RT.Service.Resolve<RoutingSettingController>().CountResourceRouting(routing) > 0)
            {
                e.BrokenDescription = "同一产线、工单类型、时间范围内产线工艺路线只能有一条".L10N();
            }
        }
    }

    /// <summary>
    /// 生产资源删除验证规则
    /// </summary>
    [DisplayName("生产资源删除验证规则")]
    [Description("产线工艺路线设置引用的生产资源不能删除")]
    public class WipResourceDeleteRuleResourceRouting : EntityRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceDeleteRuleResourceRouting()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 根据生产资源是否被产线工艺路线设置引用，判断是否能被删除
        /// </summary>
        /// <param name="entity">生产资源</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var wipResource = entity as WipResource;
            var flag = RT.Service.Resolve<RoutingSettingController>().ResourceRoutingHasUsedWipResource(wipResource.Id);
            if (flag)
            {
                e.BrokenDescription = "生产资源 [{0}] 被产线工艺路线设置引用, 不能删除!".L10nFormat(wipResource.Code);
            }
        }
    }
}
