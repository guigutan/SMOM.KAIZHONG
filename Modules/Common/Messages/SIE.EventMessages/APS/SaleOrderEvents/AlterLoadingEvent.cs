using System.Collections.Generic;

namespace SIE.EventMessages.APS.SaleOrderEvents
{
    /// <summary>
    /// 修改工程计划与需求管理数据 eventbus
    /// </summary>
    public class AlterLoadingEvent
    {
        /// <summary>
        /// 销售订单集合
        /// </summary>
        public List<SaleOrderFlatObject> SaleOrderFlatObjectList { get; set; }
    }
}
