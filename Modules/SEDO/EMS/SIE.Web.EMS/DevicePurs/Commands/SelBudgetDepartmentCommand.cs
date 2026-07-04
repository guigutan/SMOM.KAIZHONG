using SIE.Domain;
using SIE.EMS.Common.Entity;
using SIE.EMS.DevicePurs;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.DevicePurs.Commands
{
    /// <summary>
    /// 添加预算部门
    /// </summary>
    [JsCommand("SIE.Web.EMS.DevicePurs.Commands.SelBudgetDepartmentCommand")]
    public class SelBudgetDepartmentCommand : ViewCommand
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

            var savedData = new EntityList<DeviceBudgetDepartment>();

            foreach (var item in devicePurInfos)
            {
                var deviceBudgetDepartment = new DeviceBudgetDepartment();
                deviceBudgetDepartment.DevicePurId = item.SourceId;
                deviceBudgetDepartment.EnterpriseId = item.DevicePurId;                
                savedData.Add(deviceBudgetDepartment);
            }

            RF.Save(savedData);

            return true;
        }
    }
}
