using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Mes.Mq.Edge
{
    /// <summary>
    /// 采集处理基础接口
    /// </summary>
    public interface ICollectBaseService
    {

        /// <summary>
        /// 采集数据处理
        /// </summary>
        /// <param name="em">消息定义</param>
        void CollectData(EdgeMessage em);

        /// <summary>
        /// 保存异常消息
        /// </summary>
        /// <param name="em"></param>
        /// <param name="ex"></param>
        void SaveErrorMessage(EdgeMessage em, Exception ex);
    }
}
