using SIE.EventMessages.MES.LoadItems.Models;
using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.LoadItems
{
    /// <summary>
    /// 直接包装采集接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultIWoCostItem))]
    public interface IWoCostItem
    {
        /// <summary>
        /// 查询工单物料耗用数据
        /// </summary>
        /// <param name="WoNo"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        List<WoCostItemInfo> GetWoCostItemDatas(string WoNo, List<double> itemIds);
    }

    /// <summary>
    /// 默认实体
    /// </summary>
    public class DefaultIWoCostItem : IWoCostItem
    {
        /// <summary>
        /// 查询工单物料耗用数据
        /// </summary>
        /// <param name="WoNo"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public List<WoCostItemInfo> GetWoCostItemDatas(string WoNo, List<double> itemIds)
        {
            return new List<WoCostItemInfo>();
        }
    }
}
