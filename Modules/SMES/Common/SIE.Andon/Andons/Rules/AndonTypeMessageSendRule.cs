using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Rules
{
    /// <summary>
    /// 安灯类型消息推送节点+时间不能重复
    /// </summary>
    [System.ComponentModel.DisplayName("<节点+时间不能重复>")]
    [System.ComponentModel.Description("<节点+时间不能重复>")]
    public class NodeMinuteOnly: NotDuplicateRule<AndonTypeMessageSend>
    {
        /// <summary>
        /// 节点+时间不能重复
        /// </summary>
        public NodeMinuteOnly()
        {
            Properties.Add(AndonTypeMessageSend.AndonTypeIdProperty);
            Properties.Add(AndonTypeMessageSend.NodeProperty);
            Properties.Add(AndonTypeMessageSend.MinuteProperty);
            MessageBuilder = (e) =>
            {
                var andonTypeMessageSend = e as AndonTypeMessageSend;
                return "节点({0}),时间({1})数据已重复".L10nFormat(andonTypeMessageSend.Node.ToLabel().L10N(), andonTypeMessageSend.Minute);
            };
        }
    }
}
