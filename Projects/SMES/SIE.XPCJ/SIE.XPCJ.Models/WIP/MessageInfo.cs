using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP
{
  public  class MessageInfo
    {
        /// <summary>
        /// 消息类型 0错误 1成功 2 警告
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 消息主体
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Date { get; set; }

    }
}
