using SIE.Common.Schdules;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 同步备件物料信息
    /// </summary>
    [Job("备件物料信息自动同步", typeof(JobParameter))]
    public class ImportSparePartItemJob : JobBase
    {
        /// <summary>
        /// 同步备件物料信息
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                RT.Service.Resolve<SparePartController>().ImportSparePartItems();
                AddLog($"备件物料信息自动同步执行成功 !");
            }
            catch (Exception msg)
            {
                AddLog($"备件物料信息自动同步执行失败，错误信息: {msg.Message}");
            }
        }
    }
}
