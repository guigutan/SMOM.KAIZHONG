using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XPApiResultAndon
    {
        /// <summary>
        /// 安灯管理对象
        /// </summary>
        public XPAndonManage AndonManage { get; set; }

        /// <summary>
        /// 操作日志
        /// </summary>
        public List<XPAndonManageOperateLog> Logs { get; set; }

        /// <summary>
        /// 消息推送
        /// </summary>
        public List<XPAndonManageMessageSend> Messages { get; set; }
    }
}
