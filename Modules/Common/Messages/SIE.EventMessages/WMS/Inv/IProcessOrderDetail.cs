using System.Collections.Generic;

namespace SIE.EventMessages.WMS.Inv
{
    /// <summary>
    /// 流程任务单据详情接口
    /// </summary>
    public interface IProcessOrderDetail
    {
        /// <summary>
        /// 通过单号获取库存调整的单据数据
        /// </summary>
        /// <param name="OrderNo"></param>
        List<List<ProcessLableData>> GetOrderDetailByAdjust(string OrderNo);

        ///<summary>
        /// 通过单号获取库存调整的单据数据
        /// </summary>
        ///<param name = "OrderNo" ></param >
        //List < ProcessLableData > GetOrderDetailByReceipt(string OrderNo);
    }
}
