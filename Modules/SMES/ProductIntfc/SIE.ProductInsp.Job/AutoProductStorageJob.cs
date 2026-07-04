using SIE.Common.Schdules;
using SIE.ProductIntfc.ProductStorages;
using System;

namespace SIE.ProductInsp.Job
{
    /// <summary>
    /// 成品自动入库调度
    /// </summary>
    [Job("成品自动入库", typeof(JobParameter))]
    public class AutoProductStorageJob : JobBase
    {
        /// <summary>
        /// 执行成品自动入库调度
        /// </summary>
        /// <param name="param">调度参数</param>
        /// <returns>是否执行成功</returns>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var message = RT.Service.Resolve<ProductStorageController>().AutoStorageBarcode();
                AddLog(message.IsNullOrEmpty() ? "成品自动入库执行完成" : $"成品自动入库执行失败，错误信息：{message}");
            }
            catch (Exception ex)
            {
                AddLog($"成品自动入库执行失败，错误信息：{ex.Message}");
            }
        }
    }
}