using SIE.EventMessages.MES.WIP.Models;
using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.WIP
{
    /// <summary>
    /// 在制数据查询接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultWipProductVersion))]
    public interface IGetWorkOrderWipInfo
    {
        /// <summary>
        /// 获取正在进行的工单列表
        /// </summary>
        /// <param name="lineIds"></param>
        /// <param name="itemIds"></param>        
        /// <returns></returns>
        List<WorkOrderWipInfo> GetWorkOrderWipInfoList(List<double> lineIds, List<double> itemIds);
    }

    /// <summary>
    /// 默认实现在制数据查询接口
    /// </summary>
    public class DefaultWipProductVersion : IGetWorkOrderWipInfo
    {
        /// <summary>
        /// 获取正在进行的工单列表
        /// </summary>
        /// <param name="lineIds"></param>
        /// <param name="itemIds"></param>        
        /// <returns></returns>
        public List<WorkOrderWipInfo> GetWorkOrderWipInfoList(List<double> lineIds, List<double> itemIds)
        {
            return new List<WorkOrderWipInfo>();
        }
    }
}
