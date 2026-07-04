using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.Fixtures
{
    /// <summary>
    /// 新工单工治具需求清单实现接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultUpdateFixtureDemand))]
    public interface IUpdateFixtureDemand
    {
        /// <summary>
        /// 根据工治具台账Id列表保存新工单工治具需求清单
        /// </summary>
        /// <param name="toWoId">待转产工单Id</param>
        /// <param name="woId">工单Id</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="processSurface">工艺面</param>
        /// <param name="fixtureAccountIds">工治具台账Id列表</param>
        /// <param name="equipAccountIds">转产工单工单料站表的设备台帐Id列表</param>
        /// <returns>错误信息</returns>
        string SaveFixtureDemandList(double toWoId, double woId, double workShopId, double resourceId, int? processSurface, List<double> fixtureAccountIds, List<double> equipAccountIds);
    }

    /// <summary>
    /// 新工单工治具需求清单默认实现
    /// </summary>
    public class DefaultUpdateFixtureDemand : IUpdateFixtureDemand
    {
        /// <summary>
        /// 根据工治具台账Id列表保存新工单工治具需求清单
        /// </summary>
        /// <param name="toWoId">待转产工单Id</param>
        /// <param name="woId">工单Id</param>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="processSurface">工艺面</param>
        /// <param name="fixtureAccountIds">工治具台账Id列表</param>
        /// <param name="equipAccountIds">转产工单工单料站表的设备台帐Id列表</param>
        /// <returns>错误信息</returns>
        public string SaveFixtureDemandList(double toWoId, double woId, double workShopId, double resourceId, int? processSurface, List<double> fixtureAccountIds, List<double> equipAccountIds)
        {
            return string.Empty;
        }
    }
}
