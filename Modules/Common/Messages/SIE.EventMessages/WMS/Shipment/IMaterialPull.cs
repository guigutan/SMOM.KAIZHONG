using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 物料拉动接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultMaterialPull))]
    public interface IMaterialPull
    {
        /// <summary>
        /// 获取备料计划关联发运单管理明细的拣货数Sum
        /// </summary>
        /// <param name="noList">备料计划单号</param>
        /// <returns></returns>
        List<ShippingInfo> GetPickingQtySum(List<string> noList);

        /// <summary>
        /// 根据发运单号获取发货仓库
        /// </summary>
        /// <param name="noList">发运单号列表</param>
        /// <returns></returns>
        Dictionary<string, string> GetShippingWare(List<string> noList);
    }


    /// <summary>
    /// 物料拉动接口默认实现
    /// </summary>
    public class DefaultMaterialPull: IMaterialPull
    {
        /// <summary>
        /// 获取备料计划关联发运单管理明细的拣货数Sum默认实现
        /// </summary>
        /// <param name="noList">备料计划单号</param>
        /// <returns></returns>
        public List<ShippingInfo> GetPickingQtySum(List<string> noList)
        {
            return new List<ShippingInfo>();
        }

        /// <summary>
        /// 根据发运单号获取发货仓库
        /// </summary>
        /// <param name="noList"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetShippingWare(List<string> noList)
        {
            return new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// 发运单明细信息
    /// </summary>
    [Serializable]
    public class ShippingInfo
    {
        /// <summary>
        /// 相关单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 相关行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 拣货数
        /// </summary>
        public decimal Qty { get; set; } 
    }
}
