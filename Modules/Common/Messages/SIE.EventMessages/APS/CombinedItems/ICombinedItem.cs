using System;
using System.Collections.Generic;

namespace SIE.EventMessages.APS.CombinedItems
{
    /// <summary>
    /// 组合物料接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyCombinedItem))]
    public interface ICombinedItem
    {
        /// <summary>
        /// 根据组合物料ID获取组合物料的单位用量
        /// </summary>
        /// <param name="comItemIds">组合物料ID</param>
        /// <returns>返回组合物料的单位用量</returns>
        List<CombinedItemRateInfo> GetCombinedItemRateInfos(List<double> comItemIds);
    }

    /// <summary>
    /// 组合物料接口默认实现
    /// </summary>
    public class EmptyCombinedItem : ICombinedItem
    {
        /// <summary>
        /// 根据组合物料ID获取组合物料的单位用量
        /// </summary>
        /// <param name="comItemIds">组合物料ID</param>
        /// <returns>返回组合物料的单位用量</returns>
        public List<CombinedItemRateInfo> GetCombinedItemRateInfos(List<double> comItemIds)
        {
            return new List<CombinedItemRateInfo>();
        }
    }

    /// <summary>
    /// 组合物料比例信息
    /// </summary>
    [Serializable]
    public class CombinedItemRateInfo
    {
        /// <summary>
        /// 组合物料ID
        /// </summary>
        public double CombinedItemID { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 单位用量
        /// </summary>
        public int UnitQty { get; set; }
    }
}