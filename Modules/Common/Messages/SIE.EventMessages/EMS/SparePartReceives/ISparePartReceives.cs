using SIE.Services;
using System;

namespace SIE.EventMessages.EMS.SparePartReceives
{
    /// <summary>
    /// 备件接收单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultSparePartReceives))]
    public interface ISparePartReceives
    {
        /// <summary>
        /// 更新备件采购明细入库数量
        /// </summary>
        /// <param name="sparePartReceiveInfo">备件入库接收单更新信息</param>
        /// <returns></returns>
        void UpdatePurchaseDtlInboundQty(SparePartReceiveInfo sparePartReceiveInfo);
    }

    /// <summary>
    /// 备件接收单接口默认实现
    /// </summary>
    public class DefaultSparePartReceives : ISparePartReceives
    {
        /// <summary>
        /// 更新备件采购明细入库数量
        /// </summary>
        /// <param name="sparePartReceiveInfo">备件入库接收单更新信息</param>
        /// <returns></returns>
        public virtual void UpdatePurchaseDtlInboundQty(SparePartReceiveInfo sparePartReceiveInfo)
        {
            Logging.LogManager.Logger.Warn("更新备件采购明细入库数量接口未有具体实现。".L10N());
        }
    }
}
