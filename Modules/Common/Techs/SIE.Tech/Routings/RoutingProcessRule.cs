using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Tech.VictoryStandards;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序清单添加修改验证规则
    /// </summary>
    [DisplayName("工艺路线开始工序不能有解绑操作")]
    [Description("工艺路线开始工序不能有解绑操作")]
    public class RoutingProcessAddRule : EntityRule<RoutingProcess>
    {
        /// <summary>
        /// 工序清单添加修改验证规则
        /// </summary>
        public RoutingProcessAddRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证工艺路线开始工序是否有解绑操作
        /// </summary>
        /// <param name="entity">工序清单</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var routingProcess = entity as RoutingProcess;
            if(e == null)
            {
                return;
            }    
            if ((routingProcess.ProcessSign & RoutingProcessSign.Start) == RoutingProcessSign.Start && routingProcess.Process != null 
                && routingProcess.Process.CollectStepList.Any(p => p.IsUnbound))
            {
                e.BrokenDescription = "工艺路线第一站的工序：{0} 步骤不能有解绑状态".L10nFormat(routingProcess.Name);
            }
        }
    }

    /// <summary>
    /// 胜制方案删除验证规则
    /// </summary>
    [DisplayName("胜制方案删除验证规则")]
    [Description("被工艺路线工序清单引用的胜制方案不允许删除")]
    public class VictoryDeleteRule : NoReferencedRule<VictoryStandard>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VictoryDeleteRule()
        {
            Properties.Add(RoutingProcess.NormalVictoryIdProperty);
            Properties.Add(RoutingProcess.RepairVictoryIdProperty);
            MessageBuilder = (o, e) =>
            {
                var victory = o as VictoryStandard;
                return "胜制方案[{0}]已经被[{1}]引用{2}次，不能删除".L10nFormat(victory.Code, "工艺路线工序清单", e);
            };
        }
    }
}