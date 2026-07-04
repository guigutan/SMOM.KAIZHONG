using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Mes.Mq.Listener
{
    /// <summary>
    /// 消息侦听接口
    /// </summary>
    public interface IEdgeBaseListener
    {
        /// <summary>
        /// 启动
        /// </summary>
        void Start();

    }
}
