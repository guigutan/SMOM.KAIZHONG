using SIE.MES.Edge.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 错误消息Dao类
    /// </summary>
    public interface IEdgeErrorMessageDao
    {
        /// <summary>
        /// 创建一个错误消息并存到数据表中
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="errorContent"></param>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        EdgeErrorMessage CreateErrorMessage(string msgId, string errorContent, string name, string body);
    }
}
