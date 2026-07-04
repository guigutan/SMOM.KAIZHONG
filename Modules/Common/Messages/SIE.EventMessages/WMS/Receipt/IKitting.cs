using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.Receipt
{
    /// <summary>
    /// 备料计划/发货计划获取库存接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIKittingInterface))]
    public interface IKitting
    {
        /// <summary>
        /// 备料计划/发货计划获取库存
        /// </summary>
        /// <param name="warehouseIds">仓库ID集合</param>
        /// <param name="itemIds">物料ID集合</param>
        /// <param name="isBuyOnWay">是否考虑在途</param>
        /// <param name="isMakeOnWay">是否考虑在制</param>
        /// <param name="lastDate">最后时间</param>
        /// <param name="isAllocate">是否调拨</param>
        /// <param name="storeCode">货主</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <returns></returns>
        List<AnalysOnhandData> SetOnhandList(List<double> warehouseIds, List<double> itemIds, bool isBuyOnWay, bool isMakeOnWay, DateTime? lastDate, bool isAllocate = false, string storeCode = "", string projectNo = "", string taskNo = "");
    }

    class DefalitIKittingInterface : IKitting
    {
        public List<AnalysOnhandData> SetOnhandList(List<double> warehouseIds, List<double> itemIds, bool isBuyOnWay, bool isMakeOnWay, DateTime? lastDate, bool isAllocate = false, string storeCode = "", string projectNo = "", string taskNo = "")
        {
            return new List<AnalysOnhandData>();
        }
    }
}
