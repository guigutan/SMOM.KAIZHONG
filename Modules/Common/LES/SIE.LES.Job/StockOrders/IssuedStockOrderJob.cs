using SIE.Common.Schdules;
using SIE.LES.StockOrders.Service;
using System;

namespace SIE.LES.Job.StockOrders
{
    /// <summary>
    /// 备料单下发调度
    /// </summary>
    [Job("备料单下发调度", typeof(JobParameter))]
    public class IssuedStockOrderJob : JobBase
    {
        /// <summary>
        /// 拉式备料调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}]，当前身份[{RT.IdentityId}]\r\n");

            try
            {
                RT.Service.Resolve<StockOrderService>().IssuedStockOrdersJob();

                AddLog($"备料单下发执行成功!");
            }
            catch (Exception exMsg)
            {
                AddLog($"备料单下发执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
