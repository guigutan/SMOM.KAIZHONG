using SIE.Core.Common.IService;
using SIE.Domain;
using SIE.MES.Edge.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 错误消息Dao类
    /// </summary>
    public class EdgeErrorMessageDao: IEdgeErrorMessageDao
    {
        private readonly IRepositoryFactoryService rfs;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EdgeErrorMessageDao(IRepositoryFactoryService rfs)
        {
            this.rfs = rfs;
        }

        /// <summary>
        /// 创建一个错误消息并存到数据表中
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="errorContent"></param>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public EdgeErrorMessage CreateErrorMessage(string msgId, string errorContent, string name, string body)
        {
            EdgeErrorMessage errMsg = rfs.Query<EdgeErrorMessage>().Where(t => t.MsgId == msgId).ToList().FirstOrDefault();
            if (errMsg != null)
            {
                return errMsg;
            }
            errMsg = new EdgeErrorMessage();
            errMsg.MsgId = msgId;
            errMsg.Name = name;
            errMsg.ErrorContent = errorContent;
            errMsg.Bodys = body;
            errMsg.ErrorTimes = 0;
            rfs.Save(errMsg);
            return errMsg;
        }
    }
}
