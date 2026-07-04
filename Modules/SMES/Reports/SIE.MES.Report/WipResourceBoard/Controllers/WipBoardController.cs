using SIE.Api;
using SIE.MES.Report.WipResourceBoard.APIModels;
using SIE.MES.Report.WipResourceBoard.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static QRCoder.PayloadGenerator;

namespace SIE.MES.Report.WipResourceBoard.Controllers
{
    /// <summary>
    /// 产线看板控制器
    /// </summary>
    public class WipBoardController : DomainController
    {
        /// <summary>
        /// 根据产线获取工单任务信息
        /// </summary>
        /// <param name="wipId">产线Id</param>
        /// <returns></returns>
        [ApiService("根据产线获取工单任务信息")]
        [return: ApiReturn("返回工单任务信息:List<WoOrderTaskInfo>")]
        public virtual List<WoOrderTaskInfo> GetWipWorkOrderList([ApiParameter] double? wipId)
        {
            return RT.Service.Resolve<WipBoardService>().GetWipWorkOrderList(wipId);
        }

        /// <summary>
        /// 根据产线获取在制工单
        /// </summary>
        /// <param name="wipId">产线id</param>
        /// <returns></returns>
        [ApiService("根据产线获取在制工单")]
        [return: ApiReturn("返回产线获取在制工单信息: WipWorkOrder")]
        public virtual WipWorkOrder GetWipWorkOrder([ApiParameter] double? wipId)
        {
            return RT.Service.Resolve<WipBoardService>().GetWipWorkOrder(wipId);
        }

        /// <summary>
        /// 获取在制工单一次通过率
        /// </summary>
        /// <param name="woId">在制工单Id</param>
        /// <returns></returns>
        [ApiService("获取在制工单一次通过率")]
        [return: ApiReturn("返回在制工单一次通过率: WipWorkOrderPass")]

        public virtual WipWorkOrderPass GetWipProducePassRate([ApiParameter] double? woId)
        {
            return RT.Service.Resolve<WipBoardService>().GetWipProducePassRate(woId);
        }

        /// <summary>
        /// 获取时段生产效率
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <param name="wipId">产线Id</param>
        /// <returns></returns>
        [ApiService("获取时段生产效率")]
        [return: ApiReturn("返回获取时段生产效率信息: WipProductEfficiency")]
        public virtual WipProductEfficiency GetWipEfficiency([ApiParameter] List<double> woIds, [ApiParameter] double? wipId)
        {
            return RT.Service.Resolve<WipBoardService>().GetWipEfficiency(woIds, wipId);
        }

        /// <summary>
        /// 获取缺陷TOP5柏拉图
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        [ApiService("获取缺陷TOP5柏拉图")]
        [return: ApiReturn("返回获取缺陷TOP5柏拉图: DefectPlato")]
        public virtual DefectPlato GetDefectPlato([ApiParameter] List<double> woIds)
        {
            return RT.Service.Resolve<WipBoardService>().GetDefectPlato(woIds);
        }
    }
}
