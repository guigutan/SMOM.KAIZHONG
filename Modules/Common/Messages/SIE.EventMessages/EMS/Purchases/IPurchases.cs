using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.Purchases
{
    /// <summary>
    /// 采购单接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultPurchases))]
    public interface IPurchases
    {
        /// <summary>
        /// 更新采购订单
        /// </summary>
        /// <returns></returns>
        void UpdatePurchasesInbound(IList<PurchasesUpdateInfo> updateInfos);

        /// <summary>
        /// 校验项目编号所关联的采购申请对应的采购订单是否全部已经关闭
        /// </summary>
        /// <returns>返回未关闭的采购订单号</returns>
        List<string> IsPurchaseOrderClose(IList<double> projectIds);
    }

    /// <summary>
    /// 新工单工治具需求清单默认实现
    /// </summary>
    public class DefaultPurchases : IPurchases
    {
        /// <summary>
        /// 更新采购单
        /// </summary>
        /// <param name="updateInfos"></param>
        /// <returns></returns>
        public virtual void UpdatePurchasesInbound(IList<PurchasesUpdateInfo> updateInfos)
        {
            Logging.LogManager.Logger.Warn("采购单入库回写接口未有具体实现。".L10N());
        }

        /// <summary>
        /// 校验项目编号所关联的采购申请对应的采购订单是否已经关闭
        /// </summary>
        /// <param name="projectIds"></param>
        /// <returns>返回未关闭的采购订单号</returns>
        public virtual List<string> IsPurchaseOrderClose(IList<double> projectIds)
        {
            return new List<string>();
        }
    }
}
