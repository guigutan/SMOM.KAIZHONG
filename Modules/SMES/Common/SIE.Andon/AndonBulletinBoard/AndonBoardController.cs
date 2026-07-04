using SIE.Andon.AndonBulletinBoard.APIModels;
using SIE.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.AndonBulletinBoard
{
    /// <summary>
    /// 安灯看板控制器
    /// </summary>
    public class AndonBoardController : DomainController
    {
        /// <summary>
        /// 根据车间获取安灯状态统计
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("根据车间获取安灯状态统计")]
        [return: ApiReturn("返回车间安灯状态统计类:List<AndonBoardStateInfo>")]
        public virtual List<AndonBoardStateInfo> GetAndonStateData([ApiParameter] double workShopId)
        {
            return RT.Service.Resolve<AndonBoardService>().GetAndonStateData(workShopId);
        }

        /// <summary>
        /// 根据车间获取安灯大类统计
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("根据车间获取安灯大类统计")]
        [return: ApiReturn("返回车间安灯大类统计类:List<AndonBoardClassInfo>")]
        public virtual List<AndonBoardClassInfo> GetAndonClassData([ApiParameter] double workShopId)
        {
            return RT.Service.Resolve<AndonBoardService>().GetAndonClassData(workShopId);
        }

        /// <summary>
        /// 获取安灯停线信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("获取安灯停线信息")]
        [return: ApiReturn("返回安灯停线信息类:AndonLineStop")]
        public virtual AndonLineStop GetAndonLineStop([ApiParameter] double workShopId)
        {
            return RT.Service.Resolve<AndonBoardService>().GetAndonLineStop(workShopId);
        }

        /// <summary>
        /// 获取安灯类型柏拉图统计
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("获取安灯类型柏拉图统计")]
        [return: ApiReturn("返回安灯柏拉图统计类:AndonTypePlato")]
        public virtual AndonTypePlato GetAndonTypePlato([ApiParameter] double workShopId)
        {
            return RT.Service.Resolve<AndonBoardService>().GetAndonTypePlato(workShopId);
        }

        /// <summary>
        /// 获取安灯管理信息
        /// </summary>
        /// <param name="workShopId"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        [ApiService("获取安灯管理信息")]
        [return: ApiReturn("返回获取安灯管理信息类: List<AndonManageInfo>")]
        public virtual List<AndonManageInfo> GetAndonManageListByWS([ApiParameter] double workShopId, [ApiParameter] int requestType)
        {
            return RT.Service.Resolve<AndonBoardService>().GetAndonManageListByWS(workShopId, requestType);
        }

        /// <summary>
        /// 根据产线获取安灯管理信息
        /// </summary>
        /// <param name="wipId"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        [ApiService("根据产线获取安灯管理信息")]
        [return: ApiReturn("返回获取安灯管理信息类: List<AndonManageInfo>")]
        public virtual List<AndonManageInfo> GetAndonManageListByWip([ApiParameter] double wipId, [ApiParameter] int requestType)
        {
            return RT.Service.Resolve<AndonBoardService>().GetAndonManageListByWip(wipId, requestType);

        }

        /// <summary>
        /// 根据产线获取安灯工位信息
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        [ApiService("根据产线获取安灯工位信息")]
        [return: ApiReturn("返回获取安灯工位信息类: List<AndonStationInfo>")]
        public virtual List<AndonStationInfo> GetAndonManageStation([ApiParameter] double wipId)
        {
            return RT.Service.Resolve<AndonBoardService>().GetAndonManageStation(wipId);
        }
    }
}
