using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.ViewModels
{
    /// <summary>
    /// 消息模板
    /// </summary>

    [Serializable]
    public class MessageTemplate
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

    }
}
