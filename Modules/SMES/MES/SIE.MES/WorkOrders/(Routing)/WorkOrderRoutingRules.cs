using SIE.Domain.Validation;
using SIE.Tech.VictoryStandards;
using System;
using System.ComponentModel;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 胜制方案删除验证规则
    /// </summary>
    [DisplayName("胜制方案删除验证规则")]
    [Description("被工单工艺路线工序清单引用的胜制方案不允许删除")]
    public class VictoryDeleteRule : NoReferencedRule<VictoryStandard>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VictoryDeleteRule()
        {
            Properties.Add(WorkOrderRoutingProcess.NormalVictoryIdProperty);
            Properties.Add(WorkOrderRoutingProcess.RepairVictoryIdProperty);
            MessageBuilder = (o, e) =>
            {
                var victory = o as VictoryStandard;
                return "胜制方案[{0}]已经被[{1}]引用{2}次，不能删除".L10nFormat(victory.Code, "工单工艺路线工序清单", e);
            };
        }
    }
}