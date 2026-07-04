using System;
using System.Collections.Generic;

namespace SIE.EventMessages.SupplyOnway
{
    /// <summary>
    /// 查询采购订单信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultSupplyOnwayInterface))]
    public interface ISupplyOnway
    {
        /// <summary>
        /// 获取采购订单信息
        /// </summary>
        /// <param name="warehouseIds">仓库ID集合</param>
        /// <param name="itemIds">物料ID集合</param>
        /// <param name="deliveryDate">到货日期</param>
        /// <returns>采购订单信息</returns>
        List<PoInfo> GetPoInfos(List<double> warehouseIds, List<double> itemIds, DateTime? deliveryDate);
    }

    /// <summary>
    /// 查询采购订单信息接口默认实现
    /// </summary>
    class DefaultSupplyOnwayInterface : ISupplyOnway
    {
        /// <summary>
        /// 获取采购订单信息
        /// </summary>
        /// <param name="warehouseIds">仓库ID集合</param>
        /// <param name="itemIds">物料ID集合</param>
        /// <param name="deliveryDate">到货日期</param>
        /// <returns>采购订单信息</returns>
        public List<PoInfo> GetPoInfos(List<double> warehouseIds, List<double> itemIds, DateTime? deliveryDate)
        {
            return new List<PoInfo>();
        }
    }
}
