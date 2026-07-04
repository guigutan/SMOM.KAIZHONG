using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS
{
    /// <summary>
    /// EMS管理接口，实现WMS跟EMS管理通讯
    /// </summary>
    [Service(FallbackType = typeof(DefaultInventoryManage))]
    public interface IInventoryManage
    {
        /// <summary>
        /// 获取物料库存序号字典
        /// </summary>
        /// <param name="itemIds">物料Id列表</param>
        /// <returns>物料库存序号字典</returns>
        Dictionary<double, bool?> GetItemSerialNumbers(List<double> itemIds);

        /// <summary>
        /// 获取物料库存序号字典
        /// </summary>
        /// <param name="itemIds">物料Id列表</param>
        /// <returns>物料库存序号字典</returns>
        Dictionary<string, bool?> GetItemSerialNumbersByCode(List<double> itemIds);
    }

    /// <summary>
    ///  默认实现
    /// </summary>
    public class DefaultInventoryManage : IInventoryManage
    {
        /// <summary>
        /// 获取物料库存序号字典
        /// </summary>
        /// <param name="itemIds">物料Id列表</param>
        /// <returns>物料库存序号字典</returns>
        public Dictionary<double, bool?> GetItemSerialNumbers(List<double> itemIds)
        {
            return new Dictionary<double, bool?>();
        }

        /// <summary>
        /// 获取物料库存序号字典
        /// </summary>
        /// <param name="itemIds">物料Id列表</param>
        /// <returns>物料库存序号字典</returns>
        public Dictionary<string, bool?> GetItemSerialNumbersByCode(List<double> itemIds)
        {
            return new Dictionary<string, bool?>();
        }
    }
}
