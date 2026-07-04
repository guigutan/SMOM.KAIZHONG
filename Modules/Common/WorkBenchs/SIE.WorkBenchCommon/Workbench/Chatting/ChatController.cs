using SIE.Domain;
using System;

namespace SIE.WorkBenchCommon.Workbench.Chatting
{
    /// <summary>
    /// 消息控制器
    /// </summary>
    public class ChatController : DomainController
    {
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<ChatRecord> ReciveChat()
        {
            var records = Query<ChatRecord>().Where(p => (p.ToId == RT.IdentityId || p.FromId == RT.IdentityId) && (p.ReciveDate == null || p.SendDate > DateTime.Now.AddHours(-4))).OrderBy(p => p.SendDate).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
            return records;
        }

        /// <summary>
        /// 是否有新消息
        /// </summary>
        /// <returns></returns>
        public virtual bool HasNewChat()
        {
            return Query<ChatRecord>().Where(p => p.ToId == RT.IdentityId && p.ReciveDate == null).Count() > 0;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public virtual void SendChat(string message)
        {
            var record = new ChatRecord();
            record.FromId = RT.IdentityId;
            record.ToId = RT.IdentityId;
            record.Content = message;
            record.SendDate = DateTime.Now;
            record.ReciveDate = DateTime.Now;
            RF.Save(record);
        }
    }
}
