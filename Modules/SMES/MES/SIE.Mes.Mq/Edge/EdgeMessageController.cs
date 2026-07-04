using SIE.Domain;
using SIE.EventMessages.MES.WIP;
using SIE.MES.Edge.Models;
using System;
using System.Linq;

namespace SIE.Mes.Mq.Edge
{
    /// <summary>
    /// 边端消息控制器
    /// </summary>
    public class EdgeMessageController : DomainController, IMessageService
    {
        /// <summary>
        /// 重传失败消息
        /// </summary>
        /// <param name="msgIds">消息Ids</param>
        /// <returns>成功条数</returns>
        public virtual int ReSubmitErrorMessage(double[] msgIds)
        {
            var errMsgDataList = msgIds.SplitContains(tempIds=> {
                return Query<EdgeErrorMessage>().Where(p=> tempIds.Contains(p.Id)).ToList();
            });

            int successCount = 0;
            foreach (var errMsgData in errMsgDataList)
            {
                try
                {
                    EdgeMessage edgeMsg = new EdgeMessage();
                    edgeMsg.Id = errMsgData.MsgId;
                    edgeMsg.Body = errMsgData.Bodys;
                    edgeMsg.Name = errMsgData.Name;
                    edgeMsg.InvOrg = errMsgData.MsgInvOrg;

                    RT.Service.Resolve<ICollectDataService>().CollectData(edgeMsg);
                    //数据接收成功，更改消息状态
                    DB.Update<EdgeErrorMessage>().Set(p => p.IsError, YesNo.No).Where(p => p.Id == errMsgData.Id).Execute();
                    successCount++;
                }
                catch (System.Exception ex)
                {
                    RT.Logger.Error("边缘采集数据重传异常：".L10N() + ex.Message);
                }
            }
            return successCount;
        }
    }
}
