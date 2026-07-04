using SIE.Andon.Andons.Enum;
using SIE.Domain;
using System;

namespace SIE.Andon.Andons.APIModel
{
    /// <summary>
    /// 安灯维护安灯类型拉取数据
    /// </summary>
    [Serializable]
    public class AndonTypeRequestInfo
    {
        /// <summary>
        /// 安灯类型安灯大类
        /// </summary>
        public AndonTypeClass AndonTypeClass { get; set; }

        /// <summary>
        /// 安灯类型推送模块
        /// </summary>
        public double? PushPlugId { get; set; }

        /// <summary>
        /// 安灯类型推送模块名称
        /// </summary>
        public string PushPlugId_Display { get; set; }

        /// <summary>
        /// 安灯类型消息推送子表
        /// </summary>
        public EntityList<AndonMessageSend> AndonMessageList { get; } = new EntityList<AndonMessageSend>();
    }
}
