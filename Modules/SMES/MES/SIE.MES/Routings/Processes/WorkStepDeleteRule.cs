using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Routings.RoutingBoms;
using SIE.MetaModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.MES.Routings.Processes
{
    /// <summary>
    /// 工步删除验证规则
    /// </summary>
    [DisplayName("工步删除验证规则")]
    [Description("被引用的工步,不允许删除")]
    public class WorkStepDeleteRule : EntityRule<WorkStep>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkStepDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var workStep = entity as WorkStep;
            if (workStep.Id > 0)
            {
                bool result = RT.Service.Resolve<RoutingBomController>().IsRoutingBomDetailHasStep(workStep.Id);
                if (result)
                {
                    e.BrokenDescription = "工步：[{0}]被工序BOM引用，不允许删除\n".L10nFormat(workStep.Name);
                }
            }
        }
    }
}
