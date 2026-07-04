using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Rules
{
    /// <summary>
    /// 安灯维护消息推送重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("<节点+时间不能重复>")]
    [System.ComponentModel.Description("<节点+时间不能重复>")]
    public class MessageNodeMinuteOnly : NotDuplicateRule<AndonMessageSend>
    {
        /// <summary>
        /// 重复验证
        /// </summary>
        public MessageNodeMinuteOnly()
        {
            Properties.Add(AndonMessageSend.AndonIdProperty);
            Properties.Add(AndonMessageSend.NodeProperty);
            Properties.Add(AndonMessageSend.MinuteProperty);
            MessageBuilder = (e) =>
            {
                var andonMessageSend = e as AndonMessageSend;
                return "节点({0}),时间({1})数据已重复".L10nFormat(andonMessageSend.Node.ToLabel().L10N(), andonMessageSend.Minute);
            };
        }
    }

    /// <summary>
    /// 安灯消息推送推送模块验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("<安灯消息推送推送模块验证规则>")]
    [System.ComponentModel.Description("<安灯消息推送推送模块验证规则>")]
    public class MessageSendPushPlugRule : EntityRule<AndonMessageSend>
    {
        /// <summary>
        /// 验证操作
        /// </summary>
        public MessageSendPushPlugRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }
        /// <summary>
        /// 主表的推送模块为空时,消息推送子表的推送模块字段必输
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var andonMessageSend = entity as AndonMessageSend;
            //var andon = RF.GetById<Andon>(andonMessageSend.AndonId);
            if ( !andonMessageSend.PushPlugId.HasValue)
            {
                e.BrokenDescription = "消息推送子表的推送模块字段必输！".L10N();
            }
        }
    }
}
