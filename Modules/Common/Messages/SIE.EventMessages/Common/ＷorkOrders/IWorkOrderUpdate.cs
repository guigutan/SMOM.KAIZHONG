using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.Common.WorkOrders
{
    /// <summary>
    /// 工单数据更新接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIWorkOrderUpdate))]
    public interface IWorkOrderUpdate
    {
        /// <summary>
        /// 更新工单BOM(领耗退料数)
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="itemIds"></param>
        void UpdateWoBomQty(double woId, List<double> itemIds);
        /// <summary>
        /// 更新工单BOM(领耗退料数)
        /// </summary>
        /// <param name="woNo"></param>
        /// <param name="itemIds"></param>
        void UpdateWoBomQty(string woNo, List<double> itemIds);

        /// <summary>
        /// 更新工单入库数量
        /// </summary>
        /// <param name="woNo"></param>
        void UpdateWorkOrderQty(string woNo);
    }

    /// <summary>
    /// 获取发运单数据
    /// </summary>
    class DefalitIWorkOrderUpdate : IWorkOrderUpdate
    {
        /// <summary>
        /// 更新工单BOM(耗料数)
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="itemIds"></param>
        public virtual void UpdateWoBomQty(double woId, List<double> itemIds) { }
        /// <summary>
        /// 更新工单BOM(耗料数)
        /// </summary>
        /// <param name="woNo"></param>
        /// <param name="itemIds"></param>
        public virtual void UpdateWoBomQty(string woNo, List<double> itemIds) { }

        /// <summary>
        /// 更新工单入库数量
        /// </summary>
        /// <param name="woNo"></param>
        public virtual void UpdateWorkOrderQty(string woNo) { }
    }
}
