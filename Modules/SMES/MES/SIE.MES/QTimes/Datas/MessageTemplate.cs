using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Datas
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
