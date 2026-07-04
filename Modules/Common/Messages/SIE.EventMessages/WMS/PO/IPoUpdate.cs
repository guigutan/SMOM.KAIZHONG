using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.PO
{
    /// <summary>
    /// PO数据更新接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIPoUpdate))]
    public interface IPoUpdate
    {
        /// <summary>
        /// 更新PO明细行数量
        /// </summary>
        /// <param name="poDetailIds"></param>
        void UpdatePoDetailQty(List<double?> poDetailIds);
    }

    /// <summary>
    /// 获取发运单数据
    /// </summary>
    class DefalitIPoUpdate : IPoUpdate
    {
        /// <summary>
        /// 更新PO明细行数量
        /// </summary>
        /// <param name="poDetailIds"></param>
        public virtual void UpdatePoDetailQty(List<double?> poDetailIds) { }

    }
}
