using SIE.Common.Schdules;
using SIE.EMS.FixedAssets.Accounts;
using System;

namespace SIE.EMS.Job
{
    /// <summary>
    /// 资产折旧调度Job
    /// </summary>
    [Job("资产折旧调度Job", typeof(JobParameter))]
    public class AssetDepreciationJob : JobBase
    {
        /// <summary>
        /// 设备报警记录同步Job
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");

            try
            {
                RT.Service.Resolve<FixedAssetsAccountController>().SyncAssetDepreciation();
                AddLog($"资产折旧调度Job执行成功 !");
            }
            catch (Exception exMsg)
            {
                AddLog($"资产折旧调度Job执行失败，错误信息: {exMsg.Message}");
            }
        }
    }
}
