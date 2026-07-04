using SIE.Api;
using SIE.MES.Report.WorkShopBoard.APIModels;
using SIE.MES.Report.WorkShopBoard.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Report.WorkShopBoard.Controllers
{
    /// <summary>
    /// 车间看板控制器
    /// </summary>
    public class WoShopBoardController : DomainController
    {
        /// <summary>
        /// 获取车间下的产线工单信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("获取产线信息（产线状态、相关工单信息等）")]
        [return: ApiReturn("返回产线状态、相关工单信息等 List<WoShopOrderInfo>")]
        public virtual List<WoShopOrderInfo> GetWoShopOrderInfos([ApiParameter("车间ID")] double? workShopId)
        {
            return RT.Service.Resolve<WoShopBoardService>().GetWoShopOrderInfos(workShopId);
        }

        /// <summary>
        /// 获取车间下的工单产出信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("获取车间下的工单产出信息")]
        [return: ApiReturn("返回车间产出信息 List<ProductOutput>")]
        public virtual List<ProductOutput> GetProductOutputs([ApiParameter("车间Id")] double? workShopId)
        {
            return RT.Service.Resolve<WoShopBoardService>().GetProductOutputs(workShopId);
        }

        /// <summary>
        /// 获取一次通过率
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("获取一次通过率")]
        [return: ApiReturn("返回通过率 PassRate")]
        public virtual PassRate GetPassRate([ApiParameter("车间Id")] double? workShopId)
        {
            return RT.Service.Resolve<WoShopBoardService>().GetPassRate(workShopId);
        }

        /// <summary>
        /// 获取缺陷统计
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("获取缺陷统计")]
        [return: ApiReturn("返回缺陷统计列表 List<DefectCount>")]
        public virtual List<DefectCount> GetDefectCount([ApiParameter("车间Id")] double? workShopId)
        {
            return RT.Service.Resolve<WoShopBoardService>().GetDefectCount(workShopId);
        }
    }
}
