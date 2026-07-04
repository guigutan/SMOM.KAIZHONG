using SIE.MQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Mes.Mq.Edge
{
    /// <summary>
    /// 消息定义
    /// </summary>
    [Message(Id = "EdgeMqId", Exchange = "EdgeMqExchange", Queue = "SMOM.MES.EdgeToMes.Collect")]
    public class EdgeMessage
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get;set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get;set; }

        /// <summary>
        /// Body
        /// </summary>
        public object Body { get;set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public string InvOrg { get; set; } 

    }
}
