using SIE.Domain;
using SIE.EMS.Common.Entity;
using SIE.EMS.DevicePurs;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.DevicePurs.Commands
{
    /// <summary>
    /// 添加使用部门
    /// </summary>
    [JsCommand("SIE.Web.EMS.DevicePurs.Commands.SelUseDepaCommand")]
    public class SelUseDepaCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<DevicePurInfo> devicePurInfos = args.Data.ToJsonObject<List<DevicePurInfo>>();
            Check.NotNullOrEmpty(devicePurInfos, nameof(devicePurInfos));
            var savedData = new EntityList<DeviceUseDepartment>();
            foreach (var item in devicePurInfos)
            {
                var deviceDepa = new DeviceUseDepartment();
                deviceDepa.DevicePurId = item.SourceId;
                deviceDepa.EnterpriseId = item.DevicePurId;                
                savedData.Add(deviceDepa);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
